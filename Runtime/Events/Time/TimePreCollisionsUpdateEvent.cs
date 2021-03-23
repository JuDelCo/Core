// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Time
{
	public struct TimePreCollisionsUpdateEvent : ITimeDeltaEvent
	{
		public float DeltaTime { get; private set; }

		public TimePreCollisionsUpdateEvent(float fixedDeltaTime)
		{
			DeltaTime = fixedDeltaTime;
		}
	}
}
