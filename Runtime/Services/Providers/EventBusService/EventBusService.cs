using System;
using System.Collections.Generic;
using EventAction = System.Object;
using EventType = System.Type;

namespace Ju
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
		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private Dictionary<EventType, List<EventHandleActionPair>> actions = null;
		private uint callStackCounter = 0;

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

		public void Fire<T>(T data)
		{
			var type = typeof(T);

			if (!actions.ContainsKey(type))
			{
				return;
			}

			var actionList = actions[type];

			for (int i = actionList.Count - 1; i >= 0; --i)
			{
				if (callStackCounter > 0)
				{
					OnLogWarning("An event action has fired another event. This can lead to stackoverflow issues.");
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
						OnLogError("Exception running action for {0} event: {1}\n{2}", typeof(T).ToString(), e.Message, e.StackTrace);
					}
				}

				callStackCounter--;
			}
		}
	}
}
