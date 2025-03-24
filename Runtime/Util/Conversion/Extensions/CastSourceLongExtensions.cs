// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Data.Conversion
{
	public static class CastSourceLongExtensions
	{
		public static string AsString(this CastSource<long> source)
		{
			return source.value.ToString();
		}

		public static string AsTimeStringFromSeconds(this CastSource<long> source)
		{
			var s = source.value;
			var m = System.Math.Floor((float) s / 60);
			var h = System.Math.Floor((float) m / 60);
			var d = System.Math.Floor((float) h / 24);
			var w = System.Math.Floor((float) d / 7);

			string result;

			if (s < 60)
			{
				result = string.Format("{0} seconds", s);
			}
			else if (m < 60)
			{
				result = string.Format("{1} minutes, {0} seconds", s % 60, m);
			}
			else if (h < 48)
			{
				result = string.Format("{1} hours and {0} minutes", m % 60, h);
			}
			else if (d < 7)
			{
				result = string.Format("{2} days, {1} hours and {0} minutes", m % 60, h % 24, d % 7);
			}
			else
			{
				result = string.Format("{3} weeks, {2} days, {1} hours and {0} minutes", m % 60, h % 24, d % 7, w);
			}

			return result;
		}

		public static string AsTimeStringFromSecondsShort(this CastSource<long> source)
		{
			var s = source.value;
			var m = System.Math.Floor((float) s / 60);
			var h = System.Math.Floor((float) m / 60);
			var d = System.Math.Floor((float) h / 24);
			var w = System.Math.Floor((float) d / 7);

			string result;

			if (s < 60)
			{
				result = string.Format("{0}s", s);
			}
			else if (m < 60)
			{
				result = string.Format("{1}m {0}s", s % 60, m);
			}
			else if (h < 48)
			{
				result = string.Format("{1}h {0}m", m % 60, h);
			}
			else if (d < 7)
			{
				result = string.Format("{2}d {1}h {0}m", m % 60, h % 24, d % 7);
			}
			else
			{
				result = string.Format("{3}w {2}d {1}h {0}m", m % 60, h % 24, d % 7, w);
			}

			return result;
		}

		public static DateTime AsDateTimeFromUnixTimeStamp(this CastSource<long> source)
		{
			var unixEpochInstance = CastSourceDateTimeExtensions.unixEpoch;
			return unixEpochInstance.AddSeconds(source.value).ToUniversalTime();
		}
	}
}
