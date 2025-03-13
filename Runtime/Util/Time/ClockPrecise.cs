// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

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

		public Span Reset(float elapsedSeconds = 0f)
		{
			var elapsed = GetElapsedTime();

			startTime = Span.Now() - Span.Seconds(elapsedSeconds);

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

		public static bool operator <(ClockPrecise clock, float seconds)
		{
			return clock.GetElapsedTime().seconds < seconds;
		}

		public static bool operator <=(ClockPrecise clock, float seconds)
		{
			return clock.GetElapsedTime().seconds <= seconds;
		}

		public static bool operator >(ClockPrecise clock, float seconds)
		{
			return clock.GetElapsedTime().seconds > seconds;
		}

		public static bool operator >=(ClockPrecise clock, float seconds)
		{
			return clock.GetElapsedTime().seconds >= seconds;
		}

		public static bool operator <(ClockPrecise a, ClockPrecise b)
		{
			return a.GetElapsedTime() < b.GetElapsedTime();
		}

		public static bool operator <=(ClockPrecise a, ClockPrecise b)
		{
			return a.GetElapsedTime() <= b.GetElapsedTime();
		}

		public static bool operator >(ClockPrecise a, ClockPrecise b)
		{
			return a.GetElapsedTime() > b.GetElapsedTime();
		}

		public static bool operator >=(ClockPrecise a, ClockPrecise b)
		{
			return a.GetElapsedTime() >= b.GetElapsedTime();
		}
	}
}
