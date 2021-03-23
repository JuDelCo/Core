// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Time
{
	public struct TimePostFixedUpdateEvent : ITimeDeltaEvent
	{
		public float DeltaTime { get; private set; }

		public TimePostFixedUpdateEvent(float fixedDeltaTime)
		{
			DeltaTime = fixedDeltaTime;
		}
	}
}
