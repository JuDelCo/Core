// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Gradient
{
	[Serializable]
	public class ValueGradient : Gradient<float>
	{
		public ValueGradient()
		{
			SetLerp(LerpClamped);
		}

		private static float LerpClamped(float a, float b, float alpha)
		{
			alpha = Clamp01(alpha);

			return a + (alpha * (b - a));
		}

		private static float Clamp01(float value)
		{
			return Math.Min(1f, Math.Max(0f, value));
		}
	}
}
