// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Time
{
	public struct TimePostUpdateEvent : ITimeDeltaEvent
	{
		public float DeltaTime { get; private set; }

		public TimePostUpdateEvent(float deltaTime)
		{
			DeltaTime = deltaTime;
		}
	}
}
