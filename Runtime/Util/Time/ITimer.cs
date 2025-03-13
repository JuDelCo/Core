// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Time
{
	public interface ITimer : IDisposable
	{
		void Reset();
		void Reset(Span elapsed);
		void Stop();
		Span GetDuration();
		Span GetElapsedTime();
		Span GetTimeLeft();
	}
}
