// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using Ju.FSM;
using Ju.Promises;
using Ju.Services;
using Ju.Services.Internal;
using Ju.Time;
using ChannelId = System.Byte;

public static class StateUtilitiesExtensions
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

	public static Coroutine CoroutineStart(this State state, IEnumerator routine)
	{
		return ServiceCache.Coroutine.StartCoroutine(new StateLinkHandler(state), routine);
	}

	public static IPromise WaitUntil(this State state, Func<bool> condition)
	{
		return ServiceCache.Task.WaitUntil(new StateLinkHandler(state), condition);
	}

	public static IPromise WaitWhile(this State state, Func<bool> condition)
	{
		return ServiceCache.Task.WaitWhile(new StateLinkHandler(state), condition);
	}

	public static IPromise WaitForSeconds<T>(this State state, float seconds) where T : ITimeDeltaEvent
	{
		return ServiceCache.Task.WaitForSeconds<T>(new StateLinkHandler(state), seconds);
	}

	public static IPromise WaitForSeconds(this State state, float seconds)
	{
		return ServiceCache.Task.WaitForSeconds<TimeUpdateEvent>(new StateLinkHandler(state), seconds);
	}

	public static IPromise WaitForTicks<T>(this State state, int ticks) where T : ITimeEvent
	{
		return ServiceCache.Task.WaitForTicks<T>(new StateLinkHandler(state), ticks);
	}

	public static IPromise WaitForNextUpdate(this State state)
	{
		return ServiceCache.Task.WaitForNextUpdate(new StateLinkHandler(state));
	}

	public static IPromise WaitForNextFixedUpdate(this State state)
	{
		return ServiceCache.Task.WaitForNextFixedUpdate(new StateLinkHandler(state));
	}

	public static IClock NewClock<T>(this State state) where T : ITimeDeltaEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock<T>(() => linkHandler.IsActive);
	}

	public static IClock NewClock<T>(this State state, float elapsedSeconds) where T : ITimeDeltaEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
	}

	public static ITimer NewTimer<T>(this State state, float seconds, Action onCompleted) where T : ITimeDeltaEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
	}

	public static IFrameTimer NewFrameTimer<T>(this State state, int frames, Action onCompleted) where T : ITimeEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
	}
}
