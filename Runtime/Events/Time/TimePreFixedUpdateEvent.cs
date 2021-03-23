// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Time
{
	public struct TimePreFixedUpdateEvent : ITimeDeltaEvent
	{
		public float DeltaTime { get; private set; }

		public TimePreFixedUpdateEvent(float fixedDeltaTime)
		{
			DeltaTime = fixedDeltaTime;
		}
	}
}
