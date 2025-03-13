// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Random
{
	using Ju.Color;

	public static partial class Random
	{
		public static Color Color(System.Random random = null)
		{
			return Color(0f, 1f, 1f, 1f, 0.5f, 0.5f, 1f, 1f, random);
		}

		public static Color Color(float hueMin, float hueMax, System.Random random = null)
		{
			return Color(hueMin, hueMax, 1f, 1f, 0.5f, 0.5f, 1f, 1f, random);
		}

		public static Color Color(float hueMin, float hueMax, float saturationMin, float saturationMax, System.Random random = null)
		{
			return Color(hueMin, hueMax, saturationMin, saturationMax, 0.5f, 0.5f, 1f, 1f, random);
		}

		public static Color Color(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, System.Random random = null)
		{
			return Color(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, 1f, 1f, random);
		}

		public static Color Color(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax, System.Random random = null)
		{
			float hue = Clamp01(Float(hueMin, hueMax, random));
			float saturation = Clamp01(Float(saturationMin, saturationMax, random));
			float brightness = Clamp01(Float(valueMin, valueMax, random));
			float alpha = Clamp01(Float(alphaMin, alphaMax, random));

			return Ju.Color.Color.FromHSV(hue, saturation, brightness, alpha);
		}

		public static Color32 Color32(System.Random random = null)
		{
			return Color32(0f, 1f, 1f, 1f, 0.5f, 0.5f, 1f, 1f, random);
		}

		public static Color32 Color32(float hueMin, float hueMax, System.Random random = null)
		{
			return Color32(hueMin, hueMax, 1f, 1f, 0.5f, 0.5f, 1f, 1f, random);
		}

		public static Color32 Color32(float hueMin, float hueMax, float saturationMin, float saturationMax, System.Random random = null)
		{
			return Color32(hueMin, hueMax, saturationMin, saturationMax, 0.5f, 0.5f, 1f, 1f, random);
		}

		public static Color32 Color32(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, System.Random random = null)
		{
			return Color32(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, 1f, 1f, random);
		}

		public static Color32 Color32(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax, System.Random random = null)
		{
			float hue = Clamp01(Float(hueMin, hueMax, random));
			float saturation = Clamp01(Float(saturationMin, saturationMax, random));
			float brightness = Clamp01(Float(valueMin, valueMax, random));
			float alpha = Clamp01(Float(alphaMin, alphaMax, random));

			return Ju.Color.Color32.FromHSV(hue, saturation, brightness, alpha);
		}

		private static float Clamp01(float value)
		{
			return System.Math.Min(1f, System.Math.Max(0f, value));
		}
	}
}
