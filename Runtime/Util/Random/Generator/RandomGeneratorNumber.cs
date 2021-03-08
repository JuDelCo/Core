// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Random
{
	public partial class RandomGenerator
	{
		public bool Bool()
		{
			return Random.Bool(random);
		}

		public bool Bool(float probability01)
		{
			return Random.Bool(probability01, random);
		}

		public int Int()
		{
			return Random.Int(random);
		}

		public int Int(int max)
		{
			return Random.Int(max, random);
		}

		public int Int(int min, int max)
		{
			return Random.Int(min, max, random);
		}

		public float Float01()
		{
			return Random.Float01(random);
		}

		public float Float(float max)
		{
			return Random.Float(max, random);
		}

		public float Float(float min, float max)
		{
			return Random.Float(min, max, random);
		}
	}
}
