// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

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
		private Dictionary<EventType, List<SubscriberData>>[] subscribers = null;
		private List<SubscriberData>[] cachedSubscribers = null;
		private List<Action> asyncEvents = null;
		private List<Action> cachedAsyncEvents = null;
		private bool[] cancelEventStatus = null;
		private Dictionary<EventType, EventPayload> stickyData = null;

		private uint callStackCounter = 0;
		private Stack<EventType> stackEventTypes = null;
		private const int MAX_EVENT_STACK_LIMIT = 128;
		private Thread mainThread;
		private EventType eventFiredEventType;
		private bool eventFiredEventFired;

		public void Load()
		{
			subscribers = new Dictionary<EventType, List<SubscriberData>>[Byte.MaxValue];
			cachedSubscribers = new List<SubscriberData>[MAX_EVENT_STACK_LIMIT];
			asyncEvents = new List<Action>();
			cachedAsyncEvents = new List<Action>();
			stackEventTypes = new Stack<EventType>();
			cancelEventStatus = new bool[MAX_EVENT_STACK_LIMIT];
			stickyData = new Dictionary<EventType, EventPayload>();
			eventFiredEventType = typeof(EventFiredEvent);

			mainThread = Thread.CurrentThread;

			this.EventSubscribe<TimePostUpdateEvent>(ProcessAsyncEvents);
		}

		public void Subscribe<T>(ChannelId channel, ILinkHandler handle, Action<T> action, int priority = 0)
		{
			var type = typeof(T);

			if (action is null)
			{
				Log.Exception($"Subscriber action of event type {type} can't be null", new ArgumentNullException("action"));
				return;
			}

			lock (subscribers)
			{
				if (subscribers[channel] is null)
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
					Log.Exception($"Max eventbus stack reached, subscriber won't get the sticky event of type {type}", new StackOverflowException());
					return;
				}

				DispatchEvent(type, action, (T)stickyData[type]);
			}
		}

		public void Fire<T>(ChannelId channel, T data)
		{
			if (Thread.CurrentThread != mainThread)
			{
				Log.Exception("Synchronous event Fire method can't be called from outside of the Main Thread", new InvalidOperationException());
				return;
			}

			var type = typeof(T);

			var channelSubscribers = subscribers[channel];

			if (channelSubscribers is null || !channelSubscribers.ContainsKey(type))
			{
				FireDebugEvent(channel, type, data, 0);
				return;
			}

			if (callStackCounter > MAX_EVENT_STACK_LIMIT)
			{
				Log.Exception($"Max eventbus stack reached, ignoring firing of an event of type {type}", new StackOverflowException());
				return;
			}

			if (cachedSubscribers[callStackCounter] is null)
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
			lock (stickyData)
			{
				cancelEventStatus[callStackCounter] = true;
			}
		}

		private void FireDebugEvent<T>(ChannelId channel, EventType type, T data, int subscribersCount)
		{
			if (!eventFiredEventFired && type != eventFiredEventType && !(subscribers[0] is null) && subscribers[0].ContainsKey(eventFiredEventType))
			{
				string serializedData = null;

				if (typeof(ISerializableEvent).IsAssignableFrom(type))
				{
					serializedData = (data as ISerializableEvent).Serialize();
				}

				eventFiredEventFired = true;
				++callStackCounter;

				this.Fire(0, new EventFiredEvent(channel, type, serializedData, subscribersCount));

				--callStackCounter;
				eventFiredEventFired = false;
			}
		}

		private void ProcessAsyncEvents()
		{
			cachedAsyncEvents.AddRange(asyncEvents);
			asyncEvents.Clear();

			foreach (var asyncEvent in cachedAsyncEvents)
			{
				asyncEvent();
			}

			cachedAsyncEvents.Clear();
		}

		private void DispatchEvent<T>(EventType type, Action<T> action, T data)
		{
			if (stackEventTypes.Contains(type))
			{
				Log.Warning($"An event subscriber of type {type} has fired another event of the same type. This can lead to stackoverflow issues.");
			}

			stackEventTypes.Push(type);
			++callStackCounter;

			try
			{
				action(data);
			}
			catch (Exception e)
			{
				Log.Exception($"Uncaught event exception (Type: {type})", e);
			}

			--callStackCounter;
			stackEventTypes.Pop();
		}
	}
}
