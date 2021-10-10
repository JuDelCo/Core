// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.Gradient
{
	using Ju.Color;

	[Serializable]
	public struct ColorGradientKey
	{
		public float time;
		public Color color;

		public ColorGradientKey(float time, Color color)
		{
			this.time = time;
			this.color = color;
		}
	}

	[Serializable]
	public class ColorGradient
	{
		private List<ColorGradientKey> keys = new List<ColorGradientKey>(2);
		private float lowerTime = 0f;
		private float upperTime = 1f;
		private Func<float, float> easingFunction = null;

		public void SetEasing(Func<float, float> easingFunction)
		{
			this.easingFunction = easingFunction;
		}

		public void AddKey(float time, Color color)
		{
			AddKey(new ColorGradientKey(time, color));
		}

		public void AddKey(ColorGradientKey colorKey)
		{
			foreach (var key in keys)
			{
				if (key.time == colorKey.time)
				{
					return;
				}
			}

			if (lowerTime > colorKey.time)
			{
				lowerTime = colorKey.time;
			}

			if (upperTime < colorKey.time)
			{
				upperTime = colorKey.time;
			}

			keys.Add(colorKey);
		}

		public Color Evaluate(float time)
		{
			if (keys.Count <= 0)
			{
				return default(Color);
			}

			var lowerKey = GetKey(lowerTime);
			var upperKey = GetKey(upperTime);

			foreach (var key in keys)
			{
				if (time <= key.time)
				{
					if (upperKey.time > key.time)
					{
						upperKey = key;
					}
				}
				else
				{
					if (lowerKey.time < key.time)
					{
						lowerKey = key;
					}
				}
			}

			var calculatedUpperTime = (upperKey.time - lowerKey.time) == 0f ? upperKey.time + float.Epsilon : upperKey.time;
			var t = Remap(lowerKey.time, calculatedUpperTime, 0f, 1f, time);
			var tt = easingFunction == null ? t : easingFunction(t);

			return Color.Lerp(lowerKey.color, upperKey.color, tt);
		}

		public void ClearKeys()
		{
			keys.Clear();
		}

		private ColorGradientKey GetKey(float time)
		{
			foreach (var key in keys)
			{
				if (key.time == time)
				{
					return key;
				}
			}

			return new ColorGradientKey(time, default(Color));
		}

		private static float Lerp(float a, float b, float alpha)
		{
			return a + (alpha * (b - a));
		}

		private static float LerpInverse(float a, float b, float value)
		{
			return (value - a) / (b - a);
		}

		private static float Remap(float oldMin, float oldMax, float newMin, float newMax, float value)
		{
			var alpha = LerpInverse(oldMin, oldMax, value);
			return Lerp(newMin, newMax, alpha);
		}
	}
}
