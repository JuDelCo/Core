// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Time
{
	public interface IFrameTimer : IDisposable
	{
		void Reset();
		void Stop();
		int GetDuration();
		int GetElapsedFrames();
		int GetFramesLeft();
	}
}
