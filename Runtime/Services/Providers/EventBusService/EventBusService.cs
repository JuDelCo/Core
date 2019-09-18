using System;
using System.Collections.Generic;
using Handle = System.Object;
using EventAction = System.Object;
using EventType = System.Type;

namespace Ju
{
	public class EventBusService : IEventBusService
	{
		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private Dictionary<Handle, List<Tuple<EventType, EventAction>>> suscribers = null;
		private Dictionary<EventType, List<EventAction>> actions = null;
		private Dictionary<EventType, List<EventAction>> actionsDisabled = null;
		private uint callStackCounter = 0;

		public void Setup()
		{
			suscribers = new Dictionary<Handle, List<Tuple<EventType, EventAction>>>();
			actions = new Dictionary<EventType, List<EventAction>>();
			actionsDisabled = new Dictionary<EventType, List<EventAction>>();
		}

		public void Start()
		{
		}

		public void Suscribe<T>(Handle handle, Action<T> action)
		{
			var type = typeof(T);

			if (action == null)
			{
				OnLogError("Suscribe action of type {0} can't be null", type.ToString());
				return;
			}

			if (!suscribers.ContainsKey(handle))
			{
				suscribers.Add(handle, new List<Tuple<EventType, EventAction>>());
			}
			suscribers[handle].Add(new Tuple<EventType, Handle>(type, action));

			if (!actions.ContainsKey(type))
			{
				actions.Add(type, new List<EventAction>());
			}

			if (!actionsDisabled.ContainsKey(type))
			{
				actionsDisabled.Add(type, new List<EventAction>());
			}

			actions[type].Add(action);
		}

		public void Enable(Handle handle)
		{
			if (suscribers.ContainsKey(handle))
			{
				foreach (var tuple in suscribers[handle])
				{
					if (actionsDisabled[tuple.Item1].Remove(tuple.Item2))
					{
						actions[tuple.Item1].Add(tuple.Item2);
					}
				}
			}
		}

		public void Disable(Handle handle)
		{
			if (suscribers.ContainsKey(handle))
			{
				foreach (var tuple in suscribers[handle])
				{
					if (actions[tuple.Item1].Remove(tuple.Item2))
					{
						actionsDisabled[tuple.Item1].Add(tuple.Item2);
					}
				}
			}
		}

		public void UnSuscribe(Handle handle)
		{
			if (suscribers.ContainsKey(handle))
			{
				foreach (var tuple in suscribers[handle])
				{
					actions[tuple.Item1].Remove(tuple.Item2);
					actionsDisabled[tuple.Item1].Remove(tuple.Item2);
				}

				suscribers[handle].Clear();
				suscribers.Remove(handle);
			}
		}

		public void Fire<T>(T data)
		{
			var type = typeof(T);

			if (actions.ContainsKey(type))
			{
				var actionList = actions[type];

				for (int i = actionList.Count - 1; i >= 0; --i)
				{
					if (callStackCounter > 0)
					{
						OnLogWarning("An event action has fired another event. This can lead to stackoverflow issues.");
					}

					callStackCounter++;

					if (System.Diagnostics.Debugger.IsAttached)
					{
						((Action<T>)actionList[i])(data);
					}
					else
					{
						try
						{
							((Action<T>)actionList[i])(data);
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
}
