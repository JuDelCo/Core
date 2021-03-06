// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using ChannelId = System.Byte;
using EventType = System.Type;

namespace Ju.Services
{
	public delegate void EventBusServiceFiredEvent(ChannelId channel, EventType type, object obj, int actionCount);

	public interface IEventBusService
	{
		event EventBusServiceFiredEvent OnEventFired;

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
