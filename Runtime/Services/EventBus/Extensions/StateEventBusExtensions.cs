// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.FSM;
using Ju.Services.Internal;
using ChannelId = System.Byte;

public static class StateEventBusExtensions
{
	public static void EventSubscribe<T>(this State state, Action<T> action, int priority = 0)
	{
		ServiceCache.EventBus.Subscribe(new StateLinkHandler(state), action, priority);
	}

	public static void EventSubscribe<T>(this State state, Action action, int priority = 0)
	{
		ServiceCache.EventBus.Subscribe(new StateLinkHandler(state), (T _) => action(), priority);
	}

	public static void EventSubscribe<T>(this State state, Action<T> action, Func<T, bool> filter, int priority = 0)
	{
		ServiceCache.EventBus.Subscribe(new StateLinkHandler(state), action, filter, priority);
	}

	public static void EventSubscribe<T>(this State state, Action action, Func<T, bool> filter, int priority = 0)
	{
		ServiceCache.EventBus.Subscribe(new StateLinkHandler(state), action, filter, priority);
	}

	public static void EventSubscribe<T>(this State state, ChannelId channel, Action<T> action, int priority = 0)
	{
		ServiceCache.EventBus.Subscribe(channel, new StateLinkHandler(state), action, priority);
	}

	public static void EventSubscribe<T>(this State state, ChannelId channel, Action action, int priority = 0)
	{
		ServiceCache.EventBus.Subscribe(channel, new StateLinkHandler(state), (T _) => action(), priority);
	}

	public static void EventSubscribe<T>(this State state, ChannelId channel, Action<T> action, Func<T, bool> filter, int priority = 0)
	{
		ServiceCache.EventBus.Subscribe(channel, new StateLinkHandler(state), action, filter, priority);
	}

	public static void EventSubscribe<T>(this State state, ChannelId channel, Action action, Func<T, bool> filter, int priority = 0)
	{
		ServiceCache.EventBus.Subscribe(channel, new StateLinkHandler(state), action, filter, priority);
	}
}
