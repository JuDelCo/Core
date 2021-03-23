// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using ChannelId = System.Byte;

namespace Ju.Services
{
	public interface IEventBusService
	{
		void Subscribe<T>(ChannelId channel, ILinkHandler handle, Action<T> action, int priority = 0);

		void Fire<T>(ChannelId channel, T data);
		void FireAsync<T>(ChannelId channel, T data);

		void FireSticky<T>(ChannelId channel, T data);
		void FireStickyAsync<T>(ChannelId channel, T data);

		T GetSticky<T>(ChannelId channel);
		void ClearSticky<T>(ChannelId channel);
		void ClearAllSticky();

		void StopCurrentEventPropagation();
	}
}
