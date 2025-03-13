// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.Gradient
{
	[Serializable]
	public struct GradientKey<T>
	{
		public float time;
		public T value;

		public GradientKey(float time, T value)
		{
			this.time = time;
			this.value = value;
		}
	}

	[Serializable]
	public class Gradient<T>
	{
		private List<GradientKey<T>> keys = new List<GradientKey<T>>(2);
		private float lowerTime = 0f;
		private float upperTime = 1f;
		private Func<float, float> easingFunction = null;
		private Func<T, T, float, T> lerpFunction = null;

		public void SetEasing(Func<float, float> easingFunction)
		{
			this.easingFunction = easingFunction;
		}

		public void SetLerp(Func<T, T, float, T> lerpFunction)
		{
			this.lerpFunction = lerpFunction;
		}

		public void AddKey(float time, T value)
		{
			AddKey(new GradientKey<T>(time, value));
		}

		public void AddKey(GradientKey<T> valueKey)
		{
			foreach (var key in keys)
			{
				if (key.time == valueKey.time)
				{
					return;
				}
			}

			if (lowerTime > valueKey.time)
			{
				lowerTime = valueKey.time;
			}

			if (upperTime < valueKey.time)
			{
				upperTime = valueKey.time;
			}

			keys.Add(valueKey);
		}

		public T Evaluate(float time)
		{
			if (keys.Count <= 0)
			{
				return default(T);
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

			return lerpFunction(lowerKey.value, upperKey.value, tt);
		}

		public void ClearKeys()
		{
			keys.Clear();
		}

		private GradientKey<T> GetKey(float time)
		{
			foreach (var key in keys)
			{
				if (key.time == time)
				{
					return key;
				}
			}

			return new GradientKey<T>(time, default(T));
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
