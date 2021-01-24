// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Handlers;
using Ju.Promises;
using Ju.Services.Extensions;
using Ju.Time;

namespace Ju.Services
{
	internal class TaskPromise
	{
		public ILinkHandler handle;
		public Func<bool> condition;
		public IPromise promise;

		public TaskPromise(ILinkHandler handle, Func<bool> condition, IPromise promise)
		{
			this.handle = handle;
			this.condition = condition;
			this.promise = promise;
		}
	}

	internal delegate void TaskUpdateEvent(float deltaTime);

	public class TaskService : ITaskService
	{
		internal event TaskUpdateEvent OnTick = delegate { };

		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private readonly List<TaskPromise> actions = new List<TaskPromise>();
		private readonly List<TaskPromise> actionsRunner = new List<TaskPromise>();

		public void Setup()
		{
		}

		public void Start()
		{
			this.EventSubscribe<LoopUpdateEvent>(e => Tick(e.DeltaTime));
		}

		private void Tick(float deltaTime)
		{
			TickActions();
			OnTick(deltaTime);
		}

		private void TickActions()
		{
			lock (actions)
			{
				if (actions.Count > 0)
				{
					for (int i = actions.Count - 1; i >= 0; --i)
					{
						if (actions[i].handle.IsDestroyed)
						{
							actions.RemoveAt(i);
						}
						else if (actions[i].handle.IsActive)
						{
							if (actions[i].condition())
							{
								actionsRunner.Add(actions[i]);
								actions.RemoveAt(i);
							}
						}
					}
				}
			}

			if (actionsRunner.Count > 0)
			{
				for (int i = actionsRunner.Count - 1; i >= 0; --i)
				{
					if (System.Diagnostics.Debugger.IsAttached)
					{
						actionsRunner[i].promise.Resolve();
					}
					else
					{
						try
						{
							actionsRunner[i].promise.Resolve();
						}
						catch (Exception e)
						{
							OnLogError("Uncaught task exception", e);
						}
					}
				}

				actionsRunner.Clear();
			}
		}

		public void RunOnMainThread(Action action, float delay = 0f)
		{
			if (delay <= 0f)
			{
				lock (actions)
				{
					actions.Add(new TaskPromise(new KeepLinkHandler(), () => true, new Promise().Then(action)));
				}
			}
			else
			{
				WaitForSeconds(new KeepLinkHandler(), delay).Then(action);
			}
		}

		public IPromise WaitUntil(ILinkHandler handle, Func<bool> condition)
		{
			var promise = new Promise();

			lock (actions)
			{
				actions.Add(new TaskPromise(handle, condition, promise));
			}

			return promise;
		}

		public IPromise WaitWhile(ILinkHandler handle, Func<bool> condition)
		{
			return WaitUntil(handle, () => !condition());
		}

		public IPromise WaitForSeconds(ILinkHandler handle, float delay)
		{
			var timer = 0f;

			void eventHandler(float dt)
			{
				timer += dt;
			}

			OnTick += eventHandler;

			var promise = WaitUntil(handle, () => timer >= delay);

			promise.Finally(() => OnTick -= eventHandler);

			return promise;
		}
	}
}
