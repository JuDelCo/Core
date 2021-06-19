// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Services.Internal;

namespace Ju.Time
{
	[Serializable]
	public struct TimeSinceUnscaled : IEquatable<TimeSinceUnscaled>
	{
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeField]
#endif
		private float time;

		public static implicit operator float(TimeSinceUnscaled timeSince)
		{
			return (ServiceCache.Time.UnscaledTime - timeSince.time);
		}

		public static implicit operator TimeSinceUnscaled(float seconds)
		{
			return new TimeSinceUnscaled { time = (ServiceCache.Time.UnscaledTime - seconds) };
		}

		public bool Equals(TimeSinceUnscaled other)
		{
			return (time == other.time);
		}
	}
}
