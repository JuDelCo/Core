using System;
using Ju.Handlers;
using Ju.Services;

public static class IEventBusServiceExtensions
{
	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action action)
	{
		eventBusService.Subscribe<T>(handle, (T _) => action());
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action<T> action, Func<T, bool> filter)
	{
		eventBusService.Subscribe<T>(handle, (e) =>
		{
			if (filter(e))
			{
				action(e);
			}
		});
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action action, Func<T, bool> filter)
	{
		eventBusService.Subscribe<T>(handle, (T _) => action(), filter);
	}
}
