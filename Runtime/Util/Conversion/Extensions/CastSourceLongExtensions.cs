// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Data.Conversion
{
	public static class CastSourceLongExtensions
	{
		public static string AsString(this CastSource<long> source)
		{
			return source.value.ToString();
		}

		public static DateTime AsDateTimeFromUnixTimeStamp(this CastSource<long> source)
		{
			var unixEpochInstance = CastSourceDateTimeExtensions.unixEpoch;
			return unixEpochInstance.AddSeconds(source.value).ToUniversalTime();
		}
	}
}
