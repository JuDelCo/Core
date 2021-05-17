// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Data.Conversion
{
	public static class CastSourceBoolExtensions
	{
		public static string AsString(this CastSource<bool> source)
		{
			return source.value ? "True" : "False";
		}

		public static string AsStringLowercase(this CastSource<bool> source)
		{
			return source.value ? "true" : "false";
		}
	}
}
