// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Random
{
	public static partial class Random
	{
		private static System.Random defaultRandom = new System.Random();

		public static void SetDefaultSeed(int seed)
		{
			defaultRandom = new System.Random(seed);
		}

		public static RandomGenerator NewGenerator(int seed)
		{
			return new RandomGenerator(seed);
		}
	}
}
