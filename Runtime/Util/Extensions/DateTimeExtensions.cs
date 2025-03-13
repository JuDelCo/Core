// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Extensions
{
	using Ju.Data.Conversion;

	public static class DateTimeExtensions
	{
		public static string AsString(this DateTime value, string format = "yyyyMMdd_HHmmss")
		{
			return Cast.This(value).AsString(format);
		}

		public static double AsUnixTimeStamp(this DateTime value)
		{
			return Cast.This(value).AsUnixTimeStamp();
		}

		public static string AsUnixTimeStampString(this DateTime value)
		{
			return Cast.This(value).AsUnixTimeStampString();
		}
	}
}
