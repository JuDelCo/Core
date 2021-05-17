// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using System.Collections;
using Ju.Data;
using Ju.Handlers;
using Ju.Promises;
using Ju.Services.Internal;
using Ju.Time;
using UnityEngine;
using ChannelId = System.Byte;

namespace Ju.Extensions
{
	public static class BehaviourExtensions
	{
		public static Vector3 GetPosition(this Behaviour behaviour)
		{
			return behaviour.transform.position;
		}

		public static Vector3 GetLocalPosition(this Behaviour behaviour)
		{
			return behaviour.transform.localPosition;
		}

		public static Quaternion GetRotation(this Behaviour behaviour)
		{
			return behaviour.transform.rotation;
		}

		public static Quaternion GetLocalRotation(this Behaviour behaviour)
		{
			return behaviour.transform.localRotation;
		}

		public static Vector3 GetScale(this Behaviour behaviour)
		{
			return behaviour.transform.localScale;
		}
	}
}

namespace Ju.Extensions
{
	public static class BehaviourUtilitiesExtensions
	{
		public static void EventSubscribe<T>(this Behaviour behaviour, Action<T> action, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action, priority);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action action, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), (T _) => action(), priority);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action<T> action, Func<T, bool> filter, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action, filter, priority);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action action, Func<T, bool> filter, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action, filter, priority);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action<T> action, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action action, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), (T _) => action());
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action<T> action, Func<T, bool> filter, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action, filter);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action action, Func<T, bool> filter, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action, filter);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, ChannelId channel, Action<T> action, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(channel, new BehaviourLinkHandler(behaviour, alwaysActive), action, priority);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, ChannelId channel, Action action, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(channel, new BehaviourLinkHandler(behaviour, alwaysActive), (T _) => action(), priority);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, ChannelId channel, Action<T> action, Func<T, bool> filter, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(channel, new BehaviourLinkHandler(behaviour, alwaysActive), action, filter, priority);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, ChannelId channel, Action action, Func<T, bool> filter, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(channel, new BehaviourLinkHandler(behaviour, alwaysActive), action, filter, priority);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, ChannelId channel, Action<T> action, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(channel, new BehaviourLinkHandler(behaviour, alwaysActive), action);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, ChannelId channel, Action action, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(channel, new BehaviourLinkHandler(behaviour, alwaysActive), (T _) => action());
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, ChannelId channel, Action<T> action, Func<T, bool> filter, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(channel, new BehaviourLinkHandler(behaviour, alwaysActive), action, filter);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, ChannelId channel, Action action, Func<T, bool> filter, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(channel, new BehaviourLinkHandler(behaviour, alwaysActive), action, filter);
		}

		public static Services.Coroutine CoroutineStart(this Behaviour behaviour, IEnumerator routine, bool alwaysActive = true)
		{
			return ServiceCache.Coroutine.StartCoroutine(new BehaviourLinkHandler(behaviour, alwaysActive), routine);
		}

		public static IPromise WaitUntil(this Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitUntil(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitWhile(this Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitWhile(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitForSeconds<T>(this Behaviour behaviour, float seconds, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			return ServiceCache.Task.WaitForSeconds<T>(new BehaviourLinkHandler(behaviour, alwaysActive), seconds);
		}

		public static IPromise WaitForSeconds(this Behaviour behaviour, float seconds, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitForSeconds<TimeUpdateEvent>(new BehaviourLinkHandler(behaviour, alwaysActive), seconds);
		}

		public static IPromise WaitForTicks<T>(this Behaviour behaviour, int ticks, bool alwaysActive = false) where T : ITimeEvent
		{
			return ServiceCache.Task.WaitForTicks<T>(new BehaviourLinkHandler(behaviour, alwaysActive), ticks);
		}

		public static IPromise WaitForNextUpdate(this Behaviour behaviour, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitForNextUpdate(new BehaviourLinkHandler(behaviour, alwaysActive));
		}

		public static IPromise WaitForNextFixedUpdate(this Behaviour behaviour, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitForNextFixedUpdate(new BehaviourLinkHandler(behaviour, alwaysActive));
		}

		public static IClock NewClock<T>(this Behaviour behaviour, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Clock<T>(() => linkHandler.IsActive);
		}

		public static IClock NewClock<T>(this Behaviour behaviour, float elapsedSeconds, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
		}

		public static ITimer NewTimer<T>(this Behaviour behaviour, float seconds, Action onCompleted, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
		}

		public static IFrameTimer NewFrameTimer<T>(this Behaviour behaviour, int frames, Action onCompleted, bool alwaysActive = false) where T : ITimeEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
		}

		public static void NodeSubscribe(this Behaviour behaviour, JNode node, Action<JNode> action, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			node.Subscribe(linkHandler, action);
		}

		public static void NodeSubscribe(this Behaviour behaviour, JNode node, Action action, bool alwaysActive = false)
		{
			behaviour.NodeSubscribe(node, (_) => action(), alwaysActive);
		}

		public static void DataSubscribe<T>(this Behaviour behaviour, JData<T> node, Action<JData<T>> action, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			node.Subscribe(linkHandler, action);
		}

		public static Action<T> DataBind<T>(this Behaviour behaviour, JData<T> node, Action<JData<T>> action, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return node.Bind(linkHandler, action);
		}

		public static Action<TRemote> DataBind<T, TRemote>(this Behaviour behaviour, JData<T> node, Action<JData<T>> action, Func<TRemote, T> converter, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return node.Bind(linkHandler, action, converter);
		}
	}
}

#endif
