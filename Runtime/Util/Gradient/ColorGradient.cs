// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Gradient
{
	using Ju.Color;

	[Serializable]
	public class ColorGradient : Gradient<Color>
	{
		public ColorGradient()
		{
			SetLerp(LerpClamped);
		}

		private static Color LerpClamped(Color a, Color b, float alpha)
		{
			alpha = Clamp01(alpha);

			return a * (1f - alpha) + b * alpha;
		}

		private static float Clamp01(float value)
		{
			return Math.Min(1f, Math.Max(0f, value));
		}
	}
}
