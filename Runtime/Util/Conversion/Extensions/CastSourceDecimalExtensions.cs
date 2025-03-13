// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Data.Conversion
{
	public static class CastSourceDecimalExtensions
	{
		public static string AsString(this CastSource<decimal> source)
		{
			return source.value.ToString("0.#############################"); // 28-29 digits of precision
		}
	}
}
