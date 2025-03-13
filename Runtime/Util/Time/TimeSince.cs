// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Services.Internal;

namespace Ju.Time
{
	[Serializable]
	public struct TimeSince : IEquatable<TimeSince>
	{
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeField]
#endif
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
