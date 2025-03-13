// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Random
{
	using Ju.Color;

	public partial class RandomGenerator
	{
		public Color Color()
		{
			return Random.Color(random);
		}

		public Color Color(float hueMin, float hueMax)
		{
			return Random.Color(hueMin, hueMax, random);
		}

		public Color Color(float hueMin, float hueMax, float saturationMin, float saturationMax)
		{
			return Random.Color(hueMin, hueMax, saturationMin, saturationMax, random);
		}

		public Color Color(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
		{
			return Random.Color(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, random);
		}

		public Color Color(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
		{
			return Random.Color(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax, random);
		}

		public Color32 Color32()
		{
			return Random.Color32(random);
		}

		public Color32 Color32(float hueMin, float hueMax)
		{
			return Random.Color32(hueMin, hueMax, random);
		}

		public Color32 Color32(float hueMin, float hueMax, float saturationMin, float saturationMax)
		{
			return Random.Color32(hueMin, hueMax, saturationMin, saturationMax, random);
		}

		public Color32 Color32(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
		{
			return Random.Color32(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, random);
		}

		public Color32 Color32(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
		{
			return Random.Color32(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax, random);
		}
	}
}
