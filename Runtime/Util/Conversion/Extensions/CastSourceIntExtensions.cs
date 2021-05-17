// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Data.Conversion
{
	public static class CastSourceIntExtensions
	{
		public static string AsString(this CastSource<int> source)
		{
			return source.value.ToString();
		}

		public static DateTime AsDateTimeFromUnixTimeStamp(this CastSource<int> source)
		{
			var unixEpochInstance = CastSourceDateTimeExtensions.unixEpoch;
			return unixEpochInstance.AddSeconds(source.value).ToUniversalTime();
		}
	}
}
