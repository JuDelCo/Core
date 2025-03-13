// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using System.Threading;
using Ju.Extensions;
using Ju.Handlers;
using Ju.Services.Extensions;
using Ju.Time;
using ChannelId = System.Byte;
using EventAction = System.Delegate;
using EventPayload = System.Object;
using EventType = System.Type;

namespace Ju.Services
{
	using Ju.Log;

	internal class SubscriberData
	{
		public ILinkHandler handle;
		public EventAction action;
		public int priority;

		public SubscriberData(ILinkHandler handle, EventAction action, int priority)
		{
			this.handle = handle;
			this.action = action;
			this.priority = priority;
		}
	}

	public class EventBusService : IEventBusService, IServiceLoad
	{
		private Dictionary<EventType, List<SubscriberData>>[] subscribers = new Dictionary<EventType, List<SubscriberData>>[Byte.MaxValue];
		private List<SubscriberData>[] cachedSubscribers = new List<SubscriberData>[MAX_EVENT_STACK_LIMIT];
		private List<Action> asyncEvents = new List<Action>();
		private List<Action> cachedAsyncEvents = new List<Action>();
		private bool[] cancelEventStatus = new bool[MAX_EVENT_STACK_LIMIT];
		private Dictionary<EventType, EventPayload> stickyData = new Dictionary<EventType, EventPayload>();

		private uint callStackCounter = 0;
		private Stack<EventType> stackEventTypes = new Stack<EventType>();
		private const int MAX_EVENT_STACK_LIMIT = 128;
		private Thread mainThread;
		private EventType eventFiredEventType = typeof(EventFiredEvent);
		private bool suppressDebugEvent;

		public void Load()
		{
			mainThread = Thread.CurrentThread;

			this.EventSubscribe<TimePostUpdateEvent>(() =>
			{
				cachedAsyncEvents.AddRange(asyncEvents);
				asyncEvents.Clear();

				foreach (var asyncEvent in cachedAsyncEvents)
				{
					asyncEvent();
				}

				cachedAsyncEvents.Clear();
			});
		}

		public void Subscribe<T>(ChannelId channel, ILinkHandler handle, Action<T> action, int priority = 0)
		{
			var type = typeof(T);

			if (action == null)
			{
				Log.Exception($"Subscriber action of event type {type.GetFriendlyName()} can't be null", new ArgumentNullException("action"));
				return;
			}

			lock (subscribers)
			{
				if (subscribers[channel] == null)
				{
					subscribers[channel] = new Dictionary<EventType, List<SubscriberData>>();
				}

				var subscriberList = subscribers[channel].GetOrInsertNew(type);
				var subscriberIndex = 0;

				for (int i = (subscriberList.Count - 1); i >= 0; --i)
				{
					if (subscriberList[i].priority <= priority)
					{
						subscriberIndex = (i + 1);
						break;
					}
				}

				subscriberList.Insert(subscriberIndex, new SubscriberData(handle, action, priority));
			}

			if (stickyData.ContainsKey(type))
			{
				if (Thread.CurrentThread != mainThread)
				{
					Log.Exception("Synchronous sticky event can't be auto fired from outside of the Main Thread", new InvalidOperationException());
					return;
				}

				if (callStackCounter > MAX_EVENT_STACK_LIMIT)
				{
					Log.Exception($"Max eventbus stack reached, subscriber won't get the sticky event of type {type.GetFriendlyName()}", new StackOverflowException());
					return;
				}

				DispatchEvent(type, action, (T)stickyData[type]);
			}
		}

		public void Fire<T>(ChannelId channel, T data)
		{
			if (Thread.CurrentThread != mainThread)
			{
				if (!suppressDebugEvent)
				{
					suppressDebugEvent = true;
					Log.Exception("Synchronous event Fire method can't be called from outside of the Main Thread", new InvalidOperationException());
					suppressDebugEvent = false;
				}
				return;
			}

			var type = typeof(T);

			var channelSubscribers = subscribers[channel];

			if (channelSubscribers == null || !channelSubscribers.ContainsKey(type))
			{
				FireDebugEvent(channel, type, data, 0);
				return;
			}

			if (callStackCounter > MAX_EVENT_STACK_LIMIT)
			{
				if (!suppressDebugEvent)
				{
					suppressDebugEvent = true;
					Log.Exception($"Max eventbus stack reached, ignoring firing of an event of type {type.GetFriendlyName()}", new StackOverflowException());
					suppressDebugEvent = false;
				}
				return;
			}

			if (cachedSubscribers[callStackCounter] == null)
			{
				cachedSubscribers[callStackCounter] = new List<SubscriberData>();
			}

			var subscriberList = cachedSubscribers[callStackCounter];
			subscriberList.AddRange(channelSubscribers[type]);
			var subscribersCount = subscriberList.Count;

			cancelEventStatus[callStackCounter] = false;

			FireDebugEvent(channel, type, data, subscribersCount);

			for (int i = 0; i < subscribersCount; ++i)
			{
				var handle = subscriberList[i].handle;

				if (!handle.IsActive)
				{
					if (handle.IsDestroyed)
					{
						channelSubscribers[type].Remove(subscriberList[i]);
					}

					continue;
				}

				DispatchEvent(type, (Action<T>)subscriberList[i].action, data);

				if (cancelEventStatus[callStackCounter])
				{
					break;
				}
			}

			subscriberList.Clear();
		}

		public void FireAsync<T>(ChannelId channel, T data)
		{
			lock (asyncEvents)
			{
				void lambda() => Fire(channel, data);
				asyncEvents.Add(lambda);
			}
		}

		public void FireSticky<T>(ChannelId channel, T data)
		{
			lock (stickyData)
			{
				stickyData[typeof(T)] = data;
			}

			Fire(channel, data);
		}

		public void FireStickyAsync<T>(ChannelId channel, T data)
		{
			lock (asyncEvents)
			{
				void lambda() => FireSticky(channel, data);
				asyncEvents.Add(lambda);
			}
		}

		public T GetSticky<T>(ChannelId channel)
		{
			var type = typeof(T);
			var data = default(T);

			lock (stickyData)
			{
				if (stickyData.ContainsKey(type))
				{
					data = (T)stickyData[type];
				}
			}

			return data;
		}

		public void ClearSticky<T>(ChannelId channel)
		{
			lock (stickyData)
			{
				stickyData.Remove(typeof(T));
			}
		}

		public void ClearAllSticky()
		{
			lock (stickyData)
			{
				stickyData.Clear();
			}
		}

		public void StopCurrentEventPropagation()
		{
			lock (cancelEventStatus)
			{
				if (callStackCounter > 0)
				{
					cancelEventStatus[callStackCounter - 1] = true;
				}
			}
		}

		private void FireDebugEvent<T>(ChannelId channel, EventType type, T data, int subscribersCount)
		{
			if (!suppressDebugEvent && type != eventFiredEventType && subscribers[0] != null && subscribers[0].ContainsKey(eventFiredEventType))
			{
				string serializedData = null;

				if (typeof(ISerializableEvent).IsAssignableFrom(type))
				{
					serializedData = (data as ISerializableEvent).Serialize();
				}

				suppressDebugEvent = true;
				++callStackCounter;

				this.Fire(0, new EventFiredEvent(channel, type, serializedData, subscribersCount));

				--callStackCounter;
				suppressDebugEvent = false;
			}
		}

		private void DispatchEvent<T>(EventType type, Action<T> action, T data)
		{
			if (!suppressDebugEvent && stackEventTypes.Contains(type))
			{
				suppressDebugEvent = true;
				Log.Warning($"An event subscriber of type {type.GetFriendlyName()} has fired another event of the same type. This can lead to stackoverflow issues.");
				suppressDebugEvent = false;
			}

			stackEventTypes.Push(type);
			++callStackCounter;

			try
			{
				action(data);
			}
			catch (Exception e)
			{
				if (!suppressDebugEvent)
				{
					suppressDebugEvent = true;
					Log.Exception($"Uncaught event exception (Event type: {type.GetFriendlyName()})", e);
					suppressDebugEvent = false;
				}
			}

			--callStackCounter;
			stackEventTypes.Pop();
		}
	}
}
