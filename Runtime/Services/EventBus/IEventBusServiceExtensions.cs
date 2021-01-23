using System;
using Ju.Handlers;
using Ju.Services;
using ChannelId = System.Byte;

public static class IEventBusServiceExtensions
{
	private const ChannelId DEFAULT_CHANNEL = 0;

	public static void Fire<T>(this IEventBusService eventBusService, T data)
	{
		eventBusService.Fire<T>(DEFAULT_CHANNEL, data);
	}

	public static void Fire<T>(this IEventBusService eventBusService) where T : struct
	{
		eventBusService.Fire<T>(DEFAULT_CHANNEL);
	}

	public static void Fire<T>(this IEventBusService eventBusService, ChannelId channel) where T : struct
	{
		eventBusService.Fire(channel, default(T));
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action<T> action)
	{
		eventBusService.Subscribe<T>(DEFAULT_CHANNEL, handle, action);
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action action)
	{
		eventBusService.Subscribe<T>(DEFAULT_CHANNEL, handle, (T _) => action());
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action<T> action, Func<T, bool> filter)
	{
		eventBusService.Subscribe<T>(DEFAULT_CHANNEL, handle, (e) =>
		{
			if (filter(e))
			{
				action(e);
			}
		});
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action action, Func<T, bool> filter)
	{
		eventBusService.Subscribe(handle, (T _) => action(), filter);
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ChannelId channel, ILinkHandler handle, Action action)
	{
		eventBusService.Subscribe<T>(channel, handle, (T _) => action());
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ChannelId channel, ILinkHandler handle, Action<T> action, Func<T, bool> filter)
	{
		eventBusService.Subscribe<T>(channel, handle, (e) =>
		{
			if (filter(e))
			{
				action(e);
			}
		});
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ChannelId channel, ILinkHandler handle, Action action, Func<T, bool> filter)
	{
		eventBusService.Subscribe<T>(channel, handle, (T _) => action(), filter);
	}
}
