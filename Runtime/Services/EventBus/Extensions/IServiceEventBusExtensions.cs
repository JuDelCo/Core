// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.Services.Internal;
using ChannelId = System.Byte;

namespace Ju.Services.Extensions
{
	public static class IServiceEventBusExtensions
	{
		public static void EventSubscribe<T>(this IService service, Action<T> action, int priority = 0)
		{
			ServiceCache.EventBus.Subscribe(new ObjectLinkHandler<IService>(service), action, priority);
		}

		public static void EventSubscribe<T>(this IService service, Action action, int priority = 0)
		{
			ServiceCache.EventBus.Subscribe(new ObjectLinkHandler<IService>(service), (T _) => action(), priority);
		}

		public static void EventSubscribe<T>(this IService service, Action<T> action, Func<T, bool> filter, int priority = 0)
		{
			ServiceCache.EventBus.Subscribe(new ObjectLinkHandler<IService>(service), action, filter, priority);
		}

		public static void EventSubscribe<T>(this IService service, Action action, Func<T, bool> filter, int priority = 0)
		{
			ServiceCache.EventBus.Subscribe(new ObjectLinkHandler<IService>(service), (T _) => action(), filter, priority);
		}

		public static void EventSubscribe<T>(this IService service, ChannelId channel, Action<T> action, int priority = 0)
		{
			ServiceCache.EventBus.Subscribe(channel, new ObjectLinkHandler<IService>(service), action, priority);
		}

		public static void EventSubscribe<T>(this IService service, ChannelId channel, Action action, int priority = 0)
		{
			ServiceCache.EventBus.Subscribe(channel, new ObjectLinkHandler<IService>(service), (T _) => action(), priority);
		}

		public static void EventSubscribe<T>(this IService service, ChannelId channel, Action<T> action, Func<T, bool> filter, int priority = 0)
		{
			ServiceCache.EventBus.Subscribe(channel, new ObjectLinkHandler<IService>(service), action, filter, priority);
		}

		public static void EventSubscribe<T>(this IService service, ChannelId channel, Action action, Func<T, bool> filter, int priority = 0)
		{
			ServiceCache.EventBus.Subscribe(channel, new ObjectLinkHandler<IService>(service), (T _) => action(), filter, priority);
		}
	}
}
