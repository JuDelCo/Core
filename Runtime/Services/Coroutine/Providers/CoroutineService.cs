// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using System.Collections.Generic;
using Ju.Handlers;
using Ju.Services.Extensions;
using Ju.Time;

namespace Ju.Services
{
	using Ju.Log;

	internal class CoroutineHandlePair
	{
		public ILinkHandler handle;
		public IEnumerator coroutine;

		public CoroutineHandlePair(ILinkHandler handle, IEnumerator coroutine)
		{
			this.handle = handle;
			this.coroutine = coroutine;
		}
	}

	public class CoroutineService : ICoroutineService, IServiceLoad
	{
		private List<CoroutineHandlePair> coroutines = new List<CoroutineHandlePair>();
		private List<CoroutineHandlePair> coroutinesRunner = new List<CoroutineHandlePair>();

		public void Load()
		{
			this.EventSubscribe<TimeUpdateEvent>(Tick);
		}

		private void Tick()
		{
			lock (coroutines)
			{
				if (coroutines.Count > 0)
				{
					for (int i = 0, count = coroutines.Count; i < count; ++i)
					{
						coroutinesRunner.Add(coroutines[i]);
					}

					coroutines.Clear();
				}
			}

			if (coroutinesRunner.Count > 0)
			{
				for (int i = (coroutinesRunner.Count - 1); i >= 0; --i)
				{
					if (coroutinesRunner[i].handle.IsDestroyed)
					{
						coroutinesRunner.RemoveAt(i);
						continue;
					}
					else if (!coroutinesRunner[i].handle.IsActive)
					{
						continue;
					}

					if (System.Diagnostics.Debugger.IsAttached)
					{
						if (TickCoroutine(coroutinesRunner[i].coroutine))
						{
							coroutinesRunner.RemoveAt(i);
						}
					}
					else
					{
						try
						{
							if (TickCoroutine(coroutinesRunner[i].coroutine))
							{
								coroutinesRunner.RemoveAt(i);
							}
						}
						catch (Exception e)
						{
							Log.Exception("Uncaught coroutine exception", e);
						}
					}
				}

				if (coroutinesRunner.Count > 0)
				{
					for (int i = 0, count = coroutinesRunner.Count; i < count; ++i)
					{
						coroutines.Add(coroutinesRunner[i]);
					}

					coroutinesRunner.Clear();
				}
			}
		}

		private bool TickCoroutine(IEnumerator routine)
		{
			var isFinished = false;

			if (routine.Current is Coroutine coroutine)
			{
				if (!coroutine.KeepWaiting)
				{
					if (!routine.MoveNext())
					{
						isFinished = true;
					}
				}
			}
			else if (routine.Current is YieldInstruction yieldInstruction)
			{
				if (!yieldInstruction.MoveNext())
				{
					if (!routine.MoveNext())
					{
						isFinished = true;
					}
				}
			}
			else
			{
				if (!routine.MoveNext())
				{
					isFinished = true;
				}
			}

			return isFinished;
		}

		public Coroutine StartCoroutine(ILinkHandler handle, IEnumerator routine)
		{
			var coroutine = new Coroutine(routine);

			lock (coroutines)
			{
				coroutines.Add(new CoroutineHandlePair(handle, coroutine));
			}

			return coroutine;
		}
	}
}
