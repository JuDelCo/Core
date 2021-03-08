// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Extensions
{
	public static class FloatExtensions
	{
		public static bool IsWithin(this float value, float min, float max)
		{
			return (value >= min && value <= max);
		}

		public static bool IsBetween(this float value, float min, float max)
		{
			return (value > min && value < max);
		}
	}
}
