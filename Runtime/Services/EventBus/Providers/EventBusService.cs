using System;
using System.Collections.Generic;
using Ju.Handlers;
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
		public event EventBusServiceFiredEvent OnEventFired = delegate { };

		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private Dictionary<EventType, List<EventHandleActionPair>> actions = null;

		private uint callStackCounter = 0;
		private Type firstEventType = null;
		private const int MAX_EVENT_STACK_LIMIT = 999;

		public virtual void Setup()
		{
			actions = new Dictionary<EventType, List<EventHandleActionPair>>();
		}

		public void Start()
		{
		}

		public void Subscribe<T>(ILinkHandler handle, Action<T> action)
		{
			var type = typeof(T);

			if (action == null)
			{
				OnLogError("Subscribe action of type {0} can't be null", type.ToString());
				return;
			}

			if (!actions.ContainsKey(type))
			{
				actions.Add(type, new List<EventHandleActionPair>());
			}

			actions[type].Add(new EventHandleActionPair(handle, action));
		}

		public void Fire<T>() where T : struct
		{
			Fire(default(T));
		}

		public void Fire<T>(T data)
		{
			var type = typeof(T);

			if (!actions.ContainsKey(type))
			{
				OnEventFired(type, data, 0);
				return;
			}

			var actionList = actions[type];

			if (callStackCounter == 0)
			{
				firstEventType = typeof(T);
			}
			else if (callStackCounter > MAX_EVENT_STACK_LIMIT)
			{
				OnLogError("Max event stack reached, ignoring event of type {0}", firstEventType.Name);
				return;
			}

			OnEventFired(type, data, actionList.Count);

			for (int i = actionList.Count - 1; i >= 0; --i)
			{
				if (callStackCounter > 0 && firstEventType == typeof(T))
				{
					OnLogWarning("An event action of type {0} has fired another event of the same type. This can lead to stackoverflow issues.", firstEventType.Name);
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
						OnLogError("Uncaught event exception (Type: {0})", typeof(T).ToString(), e);
					}
				}

				callStackCounter--;
			}
		}
	}
}
