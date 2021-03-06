// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using System.Threading;
using Ju.Handlers;
using Ju.Services.Extensions;
using Ju.Time;
using ChannelId = System.Byte;
using EventAction = System.Delegate;
using EventPayload = System.Object;
using EventType = System.Type;

namespace Ju.Services
{
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

	public class EventBusService : IEventBusService
	{
		public event EventBusServiceFiredEvent OnEventFired;

		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

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

		public void Setup()
		{
			subscribers = new Dictionary<EventType, List<SubscriberData>>[Byte.MaxValue];
			cachedSubscribers = new List<SubscriberData>[MAX_EVENT_STACK_LIMIT];
			asyncEvents = new List<Action>();
			cachedAsyncEvents = new List<Action>();
			stackEventTypes = new Stack<EventType>();
			cancelEventStatus = new bool[MAX_EVENT_STACK_LIMIT];
			stickyData = new Dictionary<EventType, EventPayload>();

			mainThread = Thread.CurrentThread;

			this.EventSubscribe<LoopPostUpdateEvent>(ProcessAsyncEvents);
		}

		public void Subscribe<T>(ChannelId channel, ILinkHandler handle, Action<T> action, int priority = 0)
		{
			var type = typeof(T);

			if (action == null)
			{
				OnLogError("Subscriber action of event type {0} can't be null", type.ToString(), new ArgumentNullException("action"));
				return;
			}

			lock (subscribers)
			{
				if (subscribers[channel] == null)
				{
					subscribers[channel] = new Dictionary<EventType, List<SubscriberData>>();
				}

				var channelSubscribers = subscribers[channel];

				if (!channelSubscribers.ContainsKey(type))
				{
					channelSubscribers.Add(type, new List<SubscriberData>());
				}

				var subscriberList = channelSubscribers[type];
				var subscriberIndex = 0;

				for (int i = (subscriberList.Count - 1); i > 0; i--)
				{
					if (subscriberList[i].priority <= priority)
					{
						subscriberIndex = (i + 1);
						break;
					}
				}

				channelSubscribers[type].Insert(subscriberIndex, new SubscriberData(handle, action, priority));
			}

			if (stickyData.ContainsKey(type))
			{
				if (Thread.CurrentThread != mainThread)
				{
					OnLogError("Synchronous sticky event can't be auto fired from outside of the Main Thread", new InvalidOperationException());
					return;
				}

				if (callStackCounter > MAX_EVENT_STACK_LIMIT)
				{
					OnLogError("Max eventbus stack reached, subscriber won't get the sticky event of type {0}", type.ToString(), new StackOverflowException());
					return;
				}

				DispatchEvent(type, action, (T)stickyData[type]);
			}
		}

		public void Fire<T>(ChannelId channel, T data)
		{
			if (Thread.CurrentThread != mainThread)
			{
				OnLogError("Synchronous event Fire method can't be called from outside of the Main Thread", new InvalidOperationException());
				return;
			}

			var type = typeof(T);

			var channelSubscribers = subscribers[channel];

			if (channelSubscribers == null || !channelSubscribers.ContainsKey(type))
			{
				if (OnEventFired != null)
				{
					OnEventFired(channel, type, data, 0);
				}

				return;
			}

			if (callStackCounter > MAX_EVENT_STACK_LIMIT)
			{
				OnLogError("Max eventbus stack reached, ignoring firing of an event of type {0}", type.ToString(), new StackOverflowException());
				return;
			}

			if (cachedSubscribers[callStackCounter] == null)
			{
				cachedSubscribers[callStackCounter] = new List<SubscriberData>();
			}

			var subscriberList = cachedSubscribers[callStackCounter];
			subscriberList.AddRange(channelSubscribers[type]);
			var subscriberCount = subscriberList.Count;

			cancelEventStatus[callStackCounter] = false;

			if (OnEventFired != null)
			{
				callStackCounter++;
				OnEventFired(channel, type, data, subscriberCount);
				callStackCounter--;
			}

			for (int i = 0; i < subscriberCount; ++i)
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
				OnLogWarning("An event subscriber of type {0} has fired another event of the same type. This can lead to stackoverflow issues.", type.ToString());
			}

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
					OnLogError("Uncaught event exception (Type: {0})", type.ToString(), e);
				}
			}

			callStackCounter--;
			stackEventTypes.Pop();
		}
	}
}
