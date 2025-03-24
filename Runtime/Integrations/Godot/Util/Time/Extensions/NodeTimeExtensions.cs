// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Ju.Handlers;
using Ju.Time;
using Godot;

namespace Ju.Extensions
{
	public static class NodeTimeExtensions
	{
		public static IClock NewClock<T>(this Node node, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			return new Clock<T>(() => linkHandler.IsActive);
		}

		public static IClock NewClock<T>(this Node node, float elapsedSeconds, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
		}

		public static ITimer NewTimer<T>(this Node node, float seconds, Action onCompleted, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
		}

		public static IFrameTimer NewFrameTimer<T>(this Node node, int frames, Action onCompleted, bool alwaysActive = false) where T : ITimeEvent
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
		}
	}
}

#endif
