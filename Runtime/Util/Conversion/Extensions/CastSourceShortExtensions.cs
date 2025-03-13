// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Data.Conversion
{
	public static class CastSourceShortExtensions
	{
		public static string AsString(this CastSource<short> source)
		{
			return source.value.ToString();
		}
	}
}
