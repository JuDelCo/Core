// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.Time;

namespace Ju.Services.Extensions
{
	public static class IServiceTimeExtensions
	{
		public static IClock NewClock<T>(this IService service) where T : ITimeDeltaEvent
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			return new Clock<T>(() => linkHandler.IsActive);
		}

		public static IClock NewClock<T>(this IService service, float elapsedSeconds) where T : ITimeDeltaEvent
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
		}

		public static ITimer NewTimer<T>(this IService service, float seconds, Action onCompleted) where T : ITimeDeltaEvent
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
		}

		public static IFrameTimer NewFrameTimer<T>(this IService service, int frames, Action onCompleted) where T : ITimeEvent
		{
			var linkHandler = new ObjectLinkHandler<IService>(service);
			return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
		}
	}
}
