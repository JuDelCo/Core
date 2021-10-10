// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Data.Conversion
{
	public static class CastSourceDateTimeExtensions
	{
		public static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public static string AsString(this CastSource<DateTime> source, string format = "yyyyMMdd_HHmmss")
		{
			return source.value.ToUniversalTime().ToString(format);
		}

		public static double AsUnixTimeStamp(this CastSource<DateTime> source)
		{
			return Math.Truncate((source.value.ToUniversalTime() - unixEpoch).TotalSeconds);
		}

		public static string AsUnixTimeStampString(this CastSource<DateTime> source)
		{
			return source.AsUnixTimeStamp().ToString();
		}
	}
}
