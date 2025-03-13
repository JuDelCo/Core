// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Data.Conversion
{
	public static class CastSourceSByteExtensions
	{
		public static string AsString(this CastSource<sbyte> source)
		{
			return source.value.ToString();
		}
	}
}
