using System;
using System.Collections.Generic;
using Handle = System.Object;
using EventAction = System.Object;
using MessageType = System.Type;

namespace Ju
{
public class MessageBusService : IService, ILoggableService
{
	public event LogMessageEvent OnLogDebug = delegate {};
	public event LogMessageEvent OnLogInfo = delegate { };
	public event LogMessageEvent OnLogNotice = delegate { };
	public event LogMessageEvent OnLogWarning = delegate { };
	public event LogMessageEvent OnLogError = delegate { };

    private Dictionary<Handle, List<Tuple<MessageType, EventAction>>> suscribers = null;
    private Dictionary<MessageType, List<EventAction>> actions = null;
    private Dictionary<MessageType, List<EventAction>> actionsDisabled = null;
    private uint callStackCounter = 0;

	public void Setup()
	{
	    suscribers = new Dictionary<Handle, List<Tuple<MessageType, EventAction>>>();
		actions = new Dictionary<MessageType, List<EventAction>>();
		actionsDisabled = new Dictionary<MessageType, List<EventAction>>();
	}

	public void Start()
	{
	}

	public void Suscribe<T>(Handle handle, Action<T> action)
	{
		var type = typeof(T);

        if(action == null)
        {
            OnLogError("Suscribe action of type {0} can't be null", type.ToString());
            return;
        }

		if (!suscribers.ContainsKey(handle))
		{
			suscribers.Add(handle, new List<Tuple<MessageType, EventAction>>());
		}
		suscribers[handle].Add(new Tuple<MessageType, Handle>(type, action));

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
                if(actionsDisabled[tuple.Item1].Remove(tuple.Item2))
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

	public void Fire<T>() where T : new()
	{
        Fire<T>(new T());
    }

    public void Fire<T>(T message)
    {
		var type = typeof(T);

		if (actions.ContainsKey(type))
		{
            var actionList = actions[type];

            for (int i = actionList.Count - 1; i >= 0; --i)
            {
                if(callStackCounter > 0)
                {
                    OnLogWarning("An event action has fired another event. This can lead to stackoverflow issues.");
                }

				callStackCounter++;

				try
				{
                    ((Action<T>)actionList[i])(message);
				}
				catch (Exception e)
				{
					OnLogError("Exception running action for {0} event: {1}\n{2}", typeof(T).ToString(), e.Message, e.StackTrace);
				}

                callStackCounter--;
            }
		}
    }
}
}
