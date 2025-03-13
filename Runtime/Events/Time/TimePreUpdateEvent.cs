// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Time
{
	public struct TimePreUpdateEvent : ITimeDeltaEvent
	{
		public float DeltaTime { get; private set; }

		public TimePreUpdateEvent(float deltaTime)
		{
			DeltaTime = deltaTime;
		}
	}
}
