// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using Ju.Handlers;
using Ju.Time;
using UnityEngine;

namespace Ju.Extensions
{
	public static class BehaviourTimeExtensions
	{
		public static IClock NewClock<T>(this Behaviour behaviour, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Clock<T>(() => linkHandler.IsActive);
		}

		public static IClock NewClock<T>(this Behaviour behaviour, float elapsedSeconds, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
		}

		public static ITimer NewTimer<T>(this Behaviour behaviour, float seconds, Action onCompleted, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
		}

		public static IFrameTimer NewFrameTimer<T>(this Behaviour behaviour, int frames, Action onCompleted, bool alwaysActive = false) where T : ITimeEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
		}
	}
}

#endif
