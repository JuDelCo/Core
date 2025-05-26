// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.Promises;
using Ju.Services;
using Ju.Services.Internal;
using Ju.Time;

public static class IServiceTaskExtensions
{
	public static IPromise WaitUntil(this IService service, Func<bool> condition)
	{
		return ServiceCache.Task.WaitUntil(new ObjectLinkHandler<IService>(service), condition);
	}

	public static IPromise WaitWhile(this IService service, Func<bool> condition)
	{
		return ServiceCache.Task.WaitWhile(new ObjectLinkHandler<IService>(service), condition);
	}

	public static IPromise WaitForSeconds<T>(this IService service, float seconds) where T : ITimeDeltaEvent
	{
		return ServiceCache.Task.WaitForSeconds<T>(new ObjectLinkHandler<IService>(service), seconds);
	}

	public static IPromise WaitForSeconds(this IService service, float seconds)
	{
		return ServiceCache.Task.WaitForSeconds<TimeUpdateEvent>(new ObjectLinkHandler<IService>(service), seconds);
	}

	public static IPromise WaitForTicks<T>(this IService service, int ticks) where T : ITimeEvent
	{
		return ServiceCache.Task.WaitForTicks<T>(new ObjectLinkHandler<IService>(service), ticks);
	}

	public static IPromise WaitForNextUpdate(this IService service)
	{
		return ServiceCache.Task.WaitForNextUpdate(new ObjectLinkHandler<IService>(service));
	}

	public static IPromise WaitForNextFixedUpdate(this IService service)
	{
		return ServiceCache.Task.WaitForNextFixedUpdate(new ObjectLinkHandler<IService>(service));
	}
}
