// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using Ju.Handlers;
using Ju.Promises;
using Ju.Time;
using ChannelId = System.Byte;

namespace Ju.Services.Extensions
{
	public static class IServiceUtilitiesExtensions
	{
		public static void EventSubscribe<T>(this IService service, Action<T> action, int priority = 0)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new ObjectLinkHandler<IService>(service), action, priority);
		}

		public static void EventSubscribe<T>(this IService service, Action action, int priority = 0)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new ObjectLinkHandler<IService>(service), (T _) => action(), priority);
		}

		public static void EventSubscribe<T>(this IService service, Action<T> action, Func<T, bool> filter, int priority = 0)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new ObjectLinkHandler<IService>(service), action, filter, priority);
		}

		public static void EventSubscribe<T>(this IService service, Action action, Func<T, bool> filter, int priority = 0)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new ObjectLinkHandler<IService>(service), (T _) => action(), filter, priority);
		}

		public static void EventSubscribe<T>(this IService service, ChannelId channel, Action<T> action, int priority = 0)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(channel, new ObjectLinkHandler<IService>(service), action, priority);
		}

		public static void EventSubscribe<T>(this IService service, ChannelId channel, Action action, int priority = 0)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(channel, new ObjectLinkHandler<IService>(service), (T _) => action(), priority);
		}

		public static void EventSubscribe<T>(this IService service, ChannelId channel, Action<T> action, Func<T, bool> filter, int priority = 0)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(channel, new ObjectLinkHandler<IService>(service), action, filter, priority);
		}

		public static void EventSubscribe<T>(this IService service, ChannelId channel, Action action, Func<T, bool> filter, int priority = 0)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(channel, new ObjectLinkHandler<IService>(service), (T _) => action(), filter, priority);
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

		public static IPromise WaitForSeconds<T>(this IService service, float seconds) where T : ILoopTimeEvent
		{
			return ServiceContainer.Get<ITaskService>().WaitForSeconds<T>(new ObjectLinkHandler<IService>(service), seconds);
		}

		public static IPromise WaitForSeconds(this IService service, float seconds)
		{
			return ServiceContainer.Get<ITaskService>().WaitForSeconds<LoopUpdateEvent>(new ObjectLinkHandler<IService>(service), seconds);
		}

		public static IPromise WaitForTicks<T>(this IService service, int ticks) where T : ILoopEvent
		{
			return ServiceContainer.Get<ITaskService>().WaitForTicks<T>(new ObjectLinkHandler<IService>(service), ticks);
		}

		public static IPromise WaitForNextUpdate(this IService service)
		{
			return ServiceContainer.Get<ITaskService>().WaitForNextUpdate(new ObjectLinkHandler<IService>(service));
		}

		public static IPromise WaitForNextFixedUpdate(this IService service)
		{
			return ServiceContainer.Get<ITaskService>().WaitForNextFixedUpdate(new ObjectLinkHandler<IService>(service));
		}

		public static IClock NewClock<T>(this IService service) where T : ILoopTimeEvent
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			return new Clock<T>(() => linkHandler.IsActive);
		}

		public static IClock NewClock<T>(this IService service, float elapsedSeconds) where T : ILoopTimeEvent
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
		}

		public static ITimer NewTimer<T>(this IService service, float seconds, Action onCompleted) where T : ILoopTimeEvent
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
		}

		public static IFrameTimer NewFrameTimer<T>(this IService service, int frames, Action onCompleted) where T : ILoopEvent
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
		}
	}
}
