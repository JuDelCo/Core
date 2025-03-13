// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Data.Conversion
{
	public static class CastSourceCharExtensions
	{
		public static string AsString(this CastSource<char> source)
		{
			return source.value.ToString();
		}
	}
}
