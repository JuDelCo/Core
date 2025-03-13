// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

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

		// (>= 0)
		public static int Int(System.Random random = null)
		{
			if (random == null)
			{
				random = defaultRandom;
			}

			return random.Next();
		}

		// (>= 0) && (< max)
		public static int Int(int max, System.Random random = null)
		{
			if (random == null)
			{
				random = defaultRandom;
			}

			return random.Next(max);
		}

		// (>= min) && (< max)
		public static int Int(int min, int max, System.Random random = null)
		{
			if (random == null)
			{
				random = defaultRandom;
			}

			return random.Next(min, max);
		}

		// (>= 0) && (< 1)
		public static float Float01(System.Random random = null)
		{
			if (random == null)
			{
				random = defaultRandom;
			}

			return (float)random.NextDouble();
		}

		// (>= 0) && (< max)
		public static float Float(float max, System.Random random = null)
		{
			if (random == null)
			{
				random = defaultRandom;
			}

			return (float)random.NextDouble() * max;
		}

		// (>= min) && (< max)
		public static float Float(float min, float max, System.Random random = null)
		{
			return min + Float(max - min, random);
		}
	}
}
