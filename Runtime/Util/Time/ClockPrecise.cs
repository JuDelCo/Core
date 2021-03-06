// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Time
{
	public class ClockPrecise : IClock
	{
		private Span startTime;

		public ClockPrecise()
		{
			Reset();
		}

		public ClockPrecise(float elapsedSeconds)
		{
			startTime = Span.Seconds(elapsedSeconds);
		}

		public Span Reset()
		{
			var elapsed = GetElapsedTime();

			startTime = Span.Now();

			return elapsed;
		}

		public Span GetElapsedTime()
		{
			return Span.Now() - startTime;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
