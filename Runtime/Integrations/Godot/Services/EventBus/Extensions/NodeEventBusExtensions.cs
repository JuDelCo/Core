// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Ju.Handlers;
using Ju.Services.Internal;
using Godot;
using ChannelId = System.Byte;

namespace Ju.Extensions
{
	public static class NodeEventBusExtensions
	{
		public static void EventSubscribe<T>(this Node node, Action<T> action, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(new NodeLinkHandler(node, alwaysActive), action, priority);
		}

		public static void EventSubscribe<T>(this Node node, Action action, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(new NodeLinkHandler(node, alwaysActive), (T _) => action(), priority);
		}

		public static void EventSubscribe<T>(this Node node, Action<T> action, Func<T, bool> filter, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(new NodeLinkHandler(node, alwaysActive), action, filter, priority);
		}

		public static void EventSubscribe<T>(this Node node, Action action, Func<T, bool> filter, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(new NodeLinkHandler(node, alwaysActive), action, filter, priority);
		}

		public static void EventSubscribe<T>(this Node node, Action<T> action, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(new NodeLinkHandler(node, alwaysActive), action);
		}

		public static void EventSubscribe<T>(this Node node, Action action, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(new NodeLinkHandler(node, alwaysActive), (T _) => action());
		}

		public static void EventSubscribe<T>(this Node node, Action<T> action, Func<T, bool> filter, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(new NodeLinkHandler(node, alwaysActive), action, filter);
		}

		public static void EventSubscribe<T>(this Node node, Action action, Func<T, bool> filter, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(new NodeLinkHandler(node, alwaysActive), action, filter);
		}

		public static void EventSubscribe<T>(this Node node, ChannelId channel, Action<T> action, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(channel, new NodeLinkHandler(node, alwaysActive), action, priority);
		}

		public static void EventSubscribe<T>(this Node node, ChannelId channel, Action action, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(channel, new NodeLinkHandler(node, alwaysActive), (T _) => action(), priority);
		}

		public static void EventSubscribe<T>(this Node node, ChannelId channel, Action<T> action, Func<T, bool> filter, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(channel, new NodeLinkHandler(node, alwaysActive), action, filter, priority);
		}

		public static void EventSubscribe<T>(this Node node, ChannelId channel, Action action, Func<T, bool> filter, int priority = 0, bool alwaysActive = false)
		{
			ServiceCache.EventBus.Subscribe(channel, new NodeLinkHandler(node, alwaysActive), action, filter, priority);
		}

		public static void EventSubscribe<T>(this Node node, ChannelId channel, Action<T> action, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(channel, new NodeLinkHandler(node, alwaysActive), action);
		}

		public static void EventSubscribe<T>(this Node node, ChannelId channel, Action action, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(channel, new NodeLinkHandler(node, alwaysActive), (T _) => action());
		}

		public static void EventSubscribe<T>(this Node node, ChannelId channel, Action<T> action, Func<T, bool> filter, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(channel, new NodeLinkHandler(node, alwaysActive), action, filter);
		}

		public static void EventSubscribe<T>(this Node node, ChannelId channel, Action action, Func<T, bool> filter, bool alwaysActive)
		{
			ServiceCache.EventBus.Subscribe(channel, new NodeLinkHandler(node, alwaysActive), action, filter);
		}
	}
}

#endif
