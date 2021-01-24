// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Time
{
	public interface IFrameClock
	{
		int Reset();
		int GetElapsedFrames();
	}
}
