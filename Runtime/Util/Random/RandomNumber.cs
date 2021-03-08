// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using Ju.Extensions;

namespace Ju.Random
{
	public static partial class Random
	{
		public static bool Bool(System.Random random = null)
		{
			return Float01(random) < 0.5f;
		}

		public static bool Bool(float probability01, System.Random random = null)
		{
			return Float01(random) < probability01;
		}

		public static int Int(System.Random random = null)
		{
			random ??= defaultRandom;

			return random.Next();
		}

		public static int Int(int max, System.Random random = null)
		{
			random ??= defaultRandom;

			return random.Next(max);
		}

		public static int Int(int min, int max, System.Random random = null)
		{
			random ??= defaultRandom;

			return random.Next(min, max);
		}

		public static float Float01(System.Random random = null)
		{
			random ??= defaultRandom;

			return (float)random.NextDouble();
		}

		public static float Float(float max, System.Random random = null)
		{
			random ??= defaultRandom;

			return (float)random.NextDouble() * max;
		}

		public static float Float(float min, float max, System.Random random = null)
		{
			return min + Float(max - min, random);
		}
	}
}
