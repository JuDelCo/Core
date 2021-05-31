// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Services.Internal;

namespace Ju.Time
{
	public struct TimeUntil : IEquatable<TimeUntil>
	{
		private float time;

		public static implicit operator float(TimeUntil timeUntil)
		{
			return (timeUntil.time - ServiceCache.Time.Time);
		}

		public static implicit operator TimeUntil(float seconds)
		{
			return new TimeUntil { time = (ServiceCache.Time.Time + seconds) };
		}

		public bool Equals(TimeUntil other)
		{
			return (time == other.time);
		}
	}
}
