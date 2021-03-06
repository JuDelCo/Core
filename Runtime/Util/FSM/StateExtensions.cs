// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using Ju.FSM;
using Ju.Promises;
using Ju.Services;
using Ju.Time;
using ChannelId = System.Byte;

public static class StateUtilitiesExtensions
{
	public static void EventSubscribe<T>(this State state, Action<T> action, int priority = 0)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action, priority);
	}

	public static void EventSubscribe<T>(this State state, Action action, int priority = 0)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), (T _) => action(), priority);
	}

	public static void EventSubscribe<T>(this State state, Action<T> action, Func<T, bool> filter, int priority = 0)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action, filter, priority);
	}

	public static void EventSubscribe<T>(this State state, Action action, Func<T, bool> filter, int priority = 0)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action, filter, priority);
	}

	public static void EventSubscribe<T>(this State state, ChannelId channel, Action<T> action, int priority = 0)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action, priority);
	}

	public static void EventSubscribe<T>(this State state, ChannelId channel, Action action, int priority = 0)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), (T _) => action(), priority);
	}

	public static void EventSubscribe<T>(this State state, ChannelId channel, Action<T> action, Func<T, bool> filter, int priority = 0)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action, filter, priority);
	}

	public static void EventSubscribe<T>(this State state, ChannelId channel, Action action, Func<T, bool> filter, int priority = 0)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action, filter, priority);
	}

	public static Coroutine CoroutineStart(this State state, IEnumerator routine)
	{
		return ServiceContainer.Get<ICoroutineService>().StartCoroutine(new StateLinkHandler(state), routine);
	}

	public static IPromise WaitUntil(this State state, Func<bool> condition)
	{
		return ServiceContainer.Get<ITaskService>().WaitUntil(new StateLinkHandler(state), condition);
	}

	public static IPromise WaitWhile(this State state, Func<bool> condition)
	{
		return ServiceContainer.Get<ITaskService>().WaitWhile(new StateLinkHandler(state), condition);
	}

	public static IPromise WaitForSeconds<T>(this State state, float seconds) where T : ILoopTimeEvent
	{
		return ServiceContainer.Get<ITaskService>().WaitForSeconds<T>(new StateLinkHandler(state), seconds);
	}

	public static IPromise WaitForSeconds(this State state, float seconds)
	{
		return ServiceContainer.Get<ITaskService>().WaitForSeconds<LoopUpdateEvent>(new StateLinkHandler(state), seconds);
	}

	public static IPromise WaitForTicks<T>(this State state, int ticks) where T : ILoopEvent
	{
		return ServiceContainer.Get<ITaskService>().WaitForTicks<T>(new StateLinkHandler(state), ticks);
	}

	public static IPromise WaitForNextUpdate(this State state)
	{
		return ServiceContainer.Get<ITaskService>().WaitForNextUpdate(new StateLinkHandler(state));
	}

	public static IPromise WaitForNextFixedUpdate(this State state)
	{
		return ServiceContainer.Get<ITaskService>().WaitForNextFixedUpdate(new StateLinkHandler(state));
	}

	public static IClock NewClock<T>(this State state) where T : ILoopTimeEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock<T>(() => linkHandler.IsActive);
	}

	public static IClock NewClock<T>(this State state, float elapsedSeconds) where T : ILoopTimeEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
	}

	public static ITimer NewTimer<T>(this State state, float seconds, Action onCompleted) where T : ILoopTimeEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
	}

	public static IFrameTimer NewFrameTimer<T>(this State state, int frames, Action onCompleted) where T : ILoopEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
	}
}
