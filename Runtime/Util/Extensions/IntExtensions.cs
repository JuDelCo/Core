// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Extensions
{
	public static class IntExtensions
	{
		public static bool IsWithin(this int value, float min, float max)
		{
			return (value >= min && value <= max);
		}

		public static bool IsBetween(this int value, float min, float max)
		{
			return (value > min && value < max);
		}
	}
}
