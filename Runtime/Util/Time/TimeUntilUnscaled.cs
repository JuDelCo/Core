// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Services.Internal;

namespace Ju.Time
{
	[Serializable]
	public struct TimeUntilUnscaled : IEquatable<TimeUntilUnscaled>
	{
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeField]
#endif
		private float time;

		public static implicit operator float(TimeUntilUnscaled timeUntil)
		{
			return (timeUntil.time - ServiceCache.Time.UnscaledTime);
		}

		public static implicit operator TimeUntilUnscaled(float seconds)
		{
			return new TimeUntilUnscaled { time = (ServiceCache.Time.UnscaledTime + seconds) };
		}

		public bool Equals(TimeUntilUnscaled other)
		{
			return (time == other.time);
		}
	}
}
