// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using Ju.FSM;
using Ju.Promises;
using Ju.Services;
using Ju.Time;

public static class StateUtilitiesExtensions
{
	public static void EventSubscribe<T>(this State state, Action<T> action)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action);
	}

	public static void EventSubscribe<T>(this State state, Action action)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), (T _) => action());
	}

	public static void EventSubscribe<T>(this State state, Action<T> action, Func<T, bool> filter)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action, filter);
	}

	public static void EventSubscribe<T>(this State state, Action action, Func<T, bool> filter)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action, filter);
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

	public static IPromise WaitForSeconds(this State state, float delay)
	{
		return ServiceContainer.Get<ITaskService>().WaitForSeconds(new StateLinkHandler(state), delay);
	}

	public static Clock<T> NewClock<T>(this State state) where T : ILoopTimeEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock<T>(() => linkHandler.IsActive);
	}

	public static Clock<T> NewClock<T>(this State state, float elapsedSeconds) where T : ILoopTimeEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
	}

	public static Timer<T> NewTimer<T>(this State state, float seconds, Action onCompleted) where T : ILoopTimeEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
	}

	public static FrameTimer<T> NewFrameTimer<T>(this State state, int frames, Action onCompleted) where T : ILoopEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
	}
}
