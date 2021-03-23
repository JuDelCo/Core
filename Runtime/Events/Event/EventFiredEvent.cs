// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using ChannelId = System.Byte;
using EventType = System.Type;

namespace Ju.Services
{
	public struct EventFiredEvent
	{
		public ChannelId Channel { get; private set; }
		public EventType Type { get; private set; }
		public string Data { get; private set; }
		public int SubscribersCount { get; private set; }

		public EventFiredEvent(ChannelId channel, EventType type, string data, int subscribersCount)
		{
			Channel = channel;
			Type = type;
			Data = data;
			SubscribersCount = subscribersCount;
		}
	}
}
