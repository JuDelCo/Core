// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.FSM;
using Ju.Promises;
using Ju.Services.Internal;
using Ju.Time;

public static class StateTaskExtensions
{
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
}
