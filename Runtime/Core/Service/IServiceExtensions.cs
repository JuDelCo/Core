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
			ServiceContainer.Get<IEventBusService>().Subscribe(new KeepLinkHandler(), action);
		}

		public static void EventSubscribe<T>(this IService service, Action action)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new KeepLinkHandler(), (T _) => action());
		}

		public static void EventSubscribe<T>(this IService service, Action<T> action, Func<T, bool> filter)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new KeepLinkHandler(), action, filter);
		}

		public static void EventSubscribe<T>(this IService service, Action action, Func<T, bool> filter)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new KeepLinkHandler(), (T _) => action(), filter);
		}

		public static Coroutine CoroutineStart(this IService service, IEnumerator routine)
		{
			return ServiceContainer.Get<ICoroutineService>().StartCoroutine(new KeepLinkHandler(), routine);
		}

		public static IPromise WaitUntil(this IService service, Func<bool> condition)
		{
			return ServiceContainer.Get<ITaskService>().WaitUntil(new KeepLinkHandler(), condition);
		}

		public static IPromise WaitWhile(this IService service, Func<bool> condition)
		{
			return ServiceContainer.Get<ITaskService>().WaitWhile(new KeepLinkHandler(), condition);
		}

		public static IPromise WaitForSeconds(this IService service, float delay)
		{
			return ServiceContainer.Get<ITaskService>().WaitForSeconds(new KeepLinkHandler(), delay);
		}
	}
}
