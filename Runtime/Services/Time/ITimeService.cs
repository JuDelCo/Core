// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Services
{
	public interface ITimeService
	{
		float TimeScale { get; }

		float Time { get; }
		float FixedTime { get; }

		float DeltaTime { get; }
		float DeltaTimeSmooth { get; }
		float FixedDeltaTime { get; }

		float UnscaledTime { get; }
		float UnscaledFixedTime { get; }

		float UnscaledDeltaTime { get; }
		float UnscaledDeltaTimeSmooth { get; }
		float UnscaledFixedDeltaTime { get; }

		uint FrameCount { get; }

		void SetTimeScale(float timeScale);
	}
}
