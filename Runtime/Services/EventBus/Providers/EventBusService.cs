// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Handlers;
using ChannelId = System.Byte;
using EventAction = System.Object;
using EventType = System.Type;

namespace Ju.Services
{
	internal class EventHandleActionPair
	{
		public ILinkHandler handle;
		public EventAction action;

		public EventHandleActionPair(ILinkHandler handle, EventAction action)
		{
			this.handle = handle;
			this.action = action;
		}
	}

	public class EventBusService : IEventBusService
	{
		public event EventBusServiceFiredEvent OnEventFired;

		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private Dictionary<EventType, List<EventHandleActionPair>>[] actions = null;

		private uint callStackCounter = 0;
		private Stack<EventType> stackEventTypes = null;
		private const int MAX_EVENT_STACK_LIMIT = 999;

		public virtual void Setup()
		{
			actions = new Dictionary<EventType, List<EventHandleActionPair>>[Byte.MaxValue];
			stackEventTypes = new Stack<EventType>();
		}

		public void Start()
		{
		}

		public void Subscribe<T>(ChannelId channel, ILinkHandler handle, Action<T> action)
		{
			var type = typeof(T);

			if (action == null)
			{
				OnLogError("Subscribe action of type {0} can't be null", type.ToString());
				return;
			}

			if (actions[channel] == null)
			{
				actions[channel] = new Dictionary<EventType, List<EventHandleActionPair>>();
			}

			var channelActions = actions[channel];

			if (!channelActions.ContainsKey(type))
			{
				channelActions.Add(type, new List<EventHandleActionPair>());
			}

			channelActions[type].Add(new EventHandleActionPair(handle, action));
		}

		public void Fire<T>(ChannelId channel, T data)
		{
			var type = typeof(T);

			var channelActions = actions[channel];

			if (channelActions == null || !channelActions.ContainsKey(type))
			{
				if (OnEventFired != null)
				{
					OnEventFired(channel, type, data, 0);
				}

				return;
			}

			if (callStackCounter > MAX_EVENT_STACK_LIMIT)
			{
				OnLogError("Max event stack reached, ignoring event of type {0}", type.Name);
				return;
			}

			var actionList = channelActions[type];

			if (OnEventFired != null)
			{
				OnEventFired(channel, type, data, actionList.Count);
			}

			for (int i = actionList.Count - 1; i >= 0; --i)
			{
				if (stackEventTypes.Contains(type))
				{
					OnLogWarning("An event action of type {0} has fired another event of the same type. This can lead to stackoverflow issues.", type.Name);
				}

				var handle = actionList[i].handle;

				if (handle.IsDestroyed)
				{
					actionList.RemoveAt(i);

					continue;
				}

				if (!handle.IsActive)
				{
					continue;
				}

				var action = (Action<T>)actionList[i].action;

				stackEventTypes.Push(type);
				callStackCounter++;

				if (System.Diagnostics.Debugger.IsAttached)
				{
					action(data);
				}
				else
				{
					try
					{
						action(data);
					}
					catch (Exception e)
					{
						OnLogError("Uncaught event exception (Type: {0})", type.Name, e);
					}
				}

				callStackCounter--;
				stackEventTypes.Pop();
			}
		}
	}
}
