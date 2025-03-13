// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.Services;
using ChannelId = System.Byte;

public static class IEventBusServiceExtensions
{
	private const ChannelId DEFAULT_CHANNEL = 0;

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action<T> action, int priority = 0)
	{
		eventBusService.Subscribe(DEFAULT_CHANNEL, handle, action, priority);
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action action, int priority = 0)
	{
		eventBusService.Subscribe(DEFAULT_CHANNEL, handle, (T _) => action(), priority);
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action<T> action, Func<T, bool> filter, int priority = 0)
	{
		eventBusService.Subscribe<T>(DEFAULT_CHANNEL, handle, (e) =>
		{
			if (filter(e))
			{
				action(e);
			}
		}, priority);
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ILinkHandler handle, Action action, Func<T, bool> filter, int priority = 0)
	{
		eventBusService.Subscribe(DEFAULT_CHANNEL, handle, (T _) => action(), filter, priority);
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ChannelId channel, ILinkHandler handle, Action<T> action, int priority = 0)
	{
		eventBusService.Subscribe(channel, handle, action, priority);
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ChannelId channel, ILinkHandler handle, Action action, int priority = 0)
	{
		eventBusService.Subscribe(channel, handle, (T _) => action(), priority);
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ChannelId channel, ILinkHandler handle, Action<T> action, Func<T, bool> filter, int priority = 0)
	{
		eventBusService.Subscribe<T>(channel, handle, (e) =>
		{
			if (filter(e))
			{
				action(e);
			}
		}, priority);
	}

	public static void Subscribe<T>(this IEventBusService eventBusService, ChannelId channel, ILinkHandler handle, Action action, Func<T, bool> filter, int priority = 0)
	{
		eventBusService.Subscribe(channel, handle, (T _) => action(), filter, priority);
	}

	public static void Fire<T>(this IEventBusService eventBusService, T data)
	{
		eventBusService.Fire(DEFAULT_CHANNEL, data);
	}

	public static void Fire<T>(this IEventBusService eventBusService) where T : struct
	{
		eventBusService.Fire(DEFAULT_CHANNEL, default(T));
	}

	public static void Fire<T>(this IEventBusService eventBusService, ChannelId channel) where T : struct
	{
		eventBusService.Fire(channel, default(T));
	}

	public static void FireAsync<T>(this IEventBusService eventBusService, T data)
	{
		eventBusService.FireAsync(DEFAULT_CHANNEL, data);
	}

	public static void FireAsync<T>(this IEventBusService eventBusService) where T : struct
	{
		eventBusService.FireAsync(DEFAULT_CHANNEL, default(T));
	}

	public static void FireAsync<T>(this IEventBusService eventBusService, ChannelId channel) where T : struct
	{
		eventBusService.FireAsync(channel, default(T));
	}

	public static void FireSticky<T>(this IEventBusService eventBusService, T data)
	{
		eventBusService.FireSticky(DEFAULT_CHANNEL, data);
	}

	public static void FireSticky<T>(this IEventBusService eventBusService) where T : struct
	{
		eventBusService.FireSticky(DEFAULT_CHANNEL, default(T));
	}

	public static void FireSticky<T>(this IEventBusService eventBusService, ChannelId channel) where T : struct
	{
		eventBusService.FireSticky(channel, default(T));
	}

	public static void FireStickyAsync<T>(this IEventBusService eventBusService, T data)
	{
		eventBusService.FireStickyAsync(DEFAULT_CHANNEL, data);
	}

	public static void FireStickyAsync<T>(this IEventBusService eventBusService) where T : struct
	{
		eventBusService.FireStickyAsync(DEFAULT_CHANNEL, default(T));
	}

	public static void FireStickyAsync<T>(this IEventBusService eventBusService, ChannelId channel) where T : struct
	{
		eventBusService.FireStickyAsync(channel, default(T));
	}

	public static T GetSticky<T>(this IEventBusService eventBusService)
	{
		return eventBusService.GetSticky<T>(DEFAULT_CHANNEL);
	}

	public static void ClearSticky<T>(this IEventBusService eventBusService)
	{
		eventBusService.GetSticky<T>(DEFAULT_CHANNEL);
	}
}
