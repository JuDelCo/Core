// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using Ju.Handlers;
using Ju.Promises;

namespace Ju.Services.Extensions
{
	public static class IServiceUtilitiesExtensions
	{
		public static void EventSubscribe<T>(this IService service, Action<T> action)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new ObjectLinkHandler<IService>(service), action);
		}

		public static void EventSubscribe<T>(this IService service, Action action)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new ObjectLinkHandler<IService>(service), (T _) => action());
		}

		public static void EventSubscribe<T>(this IService service, Action<T> action, Func<T, bool> filter)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new ObjectLinkHandler<IService>(service), action, filter);
		}

		public static void EventSubscribe<T>(this IService service, Action action, Func<T, bool> filter)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new ObjectLinkHandler<IService>(service), (T _) => action(), filter);
		}

		public static Coroutine CoroutineStart(this IService service, IEnumerator routine)
		{
			return ServiceContainer.Get<ICoroutineService>().StartCoroutine(new ObjectLinkHandler<IService>(service), routine);
		}

		public static IPromise WaitUntil(this IService service, Func<bool> condition)
		{
			return ServiceContainer.Get<ITaskService>().WaitUntil(new ObjectLinkHandler<IService>(service), condition);
		}

		public static IPromise WaitWhile(this IService service, Func<bool> condition)
		{
			return ServiceContainer.Get<ITaskService>().WaitWhile(new ObjectLinkHandler<IService>(service), condition);
		}

		public static IPromise WaitForSeconds(this IService service, float delay)
		{
			return ServiceContainer.Get<ITaskService>().WaitForSeconds(new ObjectLinkHandler<IService>(service), delay);
		}
	}
}
