// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using Ju.Handlers;
using Ju.Services.Internal;
using UnityEngine;
using ChannelId = System.Byte;

namespace Ju.Extensions
{
	public static class BehaviourEventBusExtensions
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
	}
}

#endif
