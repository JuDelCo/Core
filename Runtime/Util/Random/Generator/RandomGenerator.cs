// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Random
{
	public partial class RandomGenerator
	{
		private System.Random random = new System.Random();

		private RandomGenerator()
		{
		}

		public RandomGenerator(int seed)
		{
			ChangeSeed(seed);
		}

		public void ChangeSeed(int seed)
		{
			random = new System.Random(seed);
		}
	}
}
