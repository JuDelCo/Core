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

		private Dictionary<Handle, List<Tuple<EventType, EventAction>>> subscribers = null;
		private Dictionary<EventType, List<Tuple<Handle, EventAction>>> actions = null;
		private Dictionary<EventType, List<EventAction>> actionsDisabled = null;
		private Func<Handle, bool> invalidHandleTest;
		private Func<Handle, bool> enabledHandleTest;
		private uint callStackCounter = 0;

		public virtual void Setup()
		{
			subscribers = new Dictionary<Handle, List<Tuple<EventType, EventAction>>>();
			actions = new Dictionary<EventType, List<Tuple<Handle, EventAction>>>();
			actionsDisabled = new Dictionary<EventType, List<EventAction>>();

			invalidHandleTest = delegate (Handle handle) { return (handle == null); };
			enabledHandleTest = delegate (Handle handle) { return true; };
		}

		public void Start()
		{
		}

		public void SetInvalidHandleTest(Func<Handle, bool> invalidHandleTest)
		{
			this.invalidHandleTest = invalidHandleTest;
		}

		public void SetEnabledHandleTest(Func<Handle, bool> enabledHandleTest)
		{
			this.enabledHandleTest = enabledHandleTest;
		}

		public void Subscribe<T>(Handle handle, Action<T> action)
		{
			var type = typeof(T);

			if (action == null)
			{
				OnLogError("Subscribe action of type {0} can't be null", type.ToString());
				return;
			}

			if (!subscribers.ContainsKey(handle))
			{
				subscribers.Add(handle, new List<Tuple<EventType, EventAction>>());
			}
			subscribers[handle].Add(new Tuple<EventType, Handle>(type, action));

			if (!actions.ContainsKey(type))
			{
				actions.Add(type, new List<Tuple<Handle, EventAction>>());
			}

			if (!actionsDisabled.ContainsKey(type))
			{
				actionsDisabled.Add(type, new List<EventAction>());
			}

			actions[type].Add(new Tuple<Handle, EventAction>(handle, action));
		}

		public void Enable(Handle handle)
		{
			if (subscribers.ContainsKey(handle))
			{
				foreach (var tuple in subscribers[handle])
				{
					if (actionsDisabled[tuple.Item1].Remove(tuple.Item2))
					{
						actions[tuple.Item1].Add(new Tuple<Handle, EventAction>(handle, tuple.Item2));
					}
				}
			}
		}

		public void Disable(Handle handle)
		{
			if (subscribers.ContainsKey(handle))
			{
				foreach (var tuple in subscribers[handle])
				{
					foreach (var t in actions[tuple.Item1])
					{
						if (t.Item1 == handle)
						{
							actionsDisabled[tuple.Item1].Add(t.Item2);
						}
					}

					actions[tuple.Item1].RemoveIf((t) =>
					{
						return t.Item1 == handle;
					});
				}
			}
		}

		public void UnSubscribe(Handle handle)
		{
			if (subscribers.ContainsKey(handle))
			{
				foreach (var tuple in subscribers[handle])
				{
					actions[tuple.Item1].RemoveIf((t) =>
					{
						return t.Item1 == handle;
					});

					actionsDisabled[tuple.Item1].Remove(tuple.Item2);
				}

				subscribers[handle].Clear();
				subscribers.Remove(handle);
			}
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

				var handle = actionList[i].Item1;

				if (invalidHandleTest(handle))
				{
					actionList.RemoveAt(i);

					continue;
				}

				if (!enabledHandleTest(handle))
				{
					continue;
				}

				var action = (Action<T>)actionList[i].Item2;

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
