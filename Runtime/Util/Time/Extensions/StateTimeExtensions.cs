// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.FSM;
using Ju.Time;

public static class StateTimeExtensions
{
	public static IClock NewClock<T>(this State state) where T : ITimeDeltaEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock<T>(() => linkHandler.IsActive);
	}

	public static IClock NewClock<T>(this State state, float elapsedSeconds) where T : ITimeDeltaEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
	}

	public static Ju.Time.ITimer NewTimer<T>(this State state, float seconds, Action onCompleted) where T : ITimeDeltaEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
	}

	public static IFrameTimer NewFrameTimer<T>(this State state, int frames, Action onCompleted) where T : ITimeEvent
	{
		var linkHandler = new StateLinkHandler(state);
		return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
	}
}
