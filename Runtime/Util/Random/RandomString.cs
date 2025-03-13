// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Random
{
	public static partial class Random
	{
		public static string Guid()
		{
			return System.Guid.NewGuid().ToString();
		}
	}
}
