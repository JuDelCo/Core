// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Services.Internal;

namespace Ju.Time
{
	public struct TimeSince : IEquatable<TimeSince>
	{
		private float time;

		public static implicit operator float(TimeSince timeSince)
		{
			return (ServiceCache.Time.Time - timeSince.time);
		}

		public static implicit operator TimeSince(float seconds)
		{
			return new TimeSince { time = (ServiceCache.Time.Time - seconds) };
		}

		public bool Equals(TimeSince other)
		{
			return (time == other.time);
		}
	}
}
