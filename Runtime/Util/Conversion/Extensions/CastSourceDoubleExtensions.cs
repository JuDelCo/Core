// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Data.Conversion;

public static class CastSourceDoubleExtensions
{
	public static string AsString(this CastSource<double> source)
	{
		return source.value.ToString("0.#################"); // 15-17 digits of precision
	}

	public static DateTime AsDateTimeFromUnixTimeStamp(this CastSource<double> source)
	{
		var unixEpochInstance = CastSourceDateTimeExtensions.unixEpoch;
		return unixEpochInstance.AddSeconds(source.value).ToUniversalTime();
	}
}
