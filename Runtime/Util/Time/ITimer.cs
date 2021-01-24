// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Time
{
	public interface ITimer
	{
		void Reset();
		void Stop();
		Span GetDuration();
		Span GetElapsedTime();
		Span GetTimeLeft();
	}
}
