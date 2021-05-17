// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Data.Conversion
{
	public static class CastSourceEnumExtensions
	{
		public static string AsString<T>(this CastSource<T> source) where T : Enum
		{
			return source.value.ToString();
		}

		public static string AsStringLowercase<T>(this CastSource<T> source) where T : Enum
		{
			return source.value.ToString().ToLower();
		}
	}
}
