// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Data.Conversion
{
	public static class CastSourceFloatExtensions
	{
		public static string AsString(this CastSource<float> source)
		{
			return source.value.ToString("0.#########"); // 6-9 digits of precision
		}
	}
}
