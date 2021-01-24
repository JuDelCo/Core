// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Time
{
	public interface IFrameTimer
	{
		void Reset();
		void Stop();
		int GetDuration();
		int GetElapsedFrames();
		int GetFramesLeft();
	}
}
