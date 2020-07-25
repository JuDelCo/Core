using System;
using System.Collections;
using Ju;
using Ju.Handlers;
using Ju.Promises;
using Ju.Services;

public static class IServiceUtilitiesExtensions
{
	public static void EventSubscribe<T>(this IService service, Action<T> action)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new KeepLinkHandler(), action);
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
