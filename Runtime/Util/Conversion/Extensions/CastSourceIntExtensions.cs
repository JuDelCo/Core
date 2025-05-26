// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Data.Conversion;

public static class CastSourceIntExtensions
{
	public static string AsString(this CastSource<int> source)
	{
		return source.value.ToString();
	}

	public static string AsTimeStringFromSeconds(this CastSource<int> source)
	{
		return Cast.This((long) source.value).AsTimeStringFromSeconds();
	}

	public static string AsTimeStringFromSecondsShort(this CastSource<int> source)
	{
		return Cast.This((long) source.value).AsTimeStringFromSecondsShort();
	}

	public static DateTime AsDateTimeFromUnixTimeStamp(this CastSource<int> source)
	{
		var unixEpochInstance = CastSourceDateTimeExtensions.unixEpoch;
		return unixEpochInstance.AddSeconds(source.value).ToUniversalTime();
	}
}
