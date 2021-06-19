// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Color
{
	[Serializable]
	public partial struct Color : IEquatable<Color>
	{
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeField]
#endif
		private float rValue;
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeField]
#endif
		private float gValue;
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeField]
#endif
		private float bValue;
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeField]
#endif
		private float aValue;

		public Color(float r, float g, float b)
		{
			rValue = r;
			gValue = g;
			bValue = b;
			aValue = 1f;
		}

		public Color(float r, float g, float b, float a)
		{
			rValue = r;
			gValue = g;
			bValue = b;
			aValue = a;
		}

#pragma warning disable IDE1006

		public float r
		{
			get { return rValue; }
			set { rValue = value; }
		}

		public float g
		{
			get { return gValue; }
			set { gValue = value; }
		}

		public float b
		{
			get { return bValue; }
			set { bValue = value; }
		}

		public float a
		{
			get { return aValue; }
			set { aValue = value; }
		}

#pragma warning restore IDE1006

		public static Color operator +(Color a, Color b)
		{
			return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
		}

		public static Color operator -(Color a, Color b)
		{
			return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		public static Color operator *(Color a, Color b)
		{
			return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
		}

		public static Color operator *(Color c, int v)
		{
			return new Color(c.r * v, c.g * v, c.b * v, c.a * v);
		}

		public static Color operator *(int v, Color c)
		{
			return new Color(c.r * v, c.g * v, c.b * v, c.a * v);
		}

		public static Color operator *(Color c, float v)
		{
			return new Color(c.r * v, c.g * v, c.b * v, c.a * v);
		}

		public static Color operator *(float v, Color c)
		{
			return new Color(c.r * v, c.g * v, c.b * v, c.a * v);
		}

		public static Color operator /(Color a, Color b)
		{
			return new Color(a.r / b.r, a.g / b.g, a.b / b.b, a.a / b.a);
		}

		public static Color operator /(Color c, int v)
		{
			return new Color(c.r / v, c.g / v, c.b / v, c.a / v);
		}

		public static Color operator /(int v, Color c)
		{
			return new Color(v / c.r, v / c.g, v / c.b, v / c.a);
		}

		public static Color operator /(Color c, float v)
		{
			return new Color(c.r / v, c.g / v, c.b / v, c.a / v);
		}

		public static Color operator /(float v, Color c)
		{
			return new Color(v / c.r, v / c.g, v / c.b, v / c.a);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + rValue.GetHashCode();
				hash = hash * 23 + gValue.GetHashCode();
				hash = hash * 23 + bValue.GetHashCode();
				hash = hash * 23 + aValue.GetHashCode();
				return hash;
			}
		}

		public bool Equals(Color other)
		{
			return (this == other);
		}

		public override bool Equals(object obj)
		{
			return (obj is Color color && (this == color));
		}

		public static bool operator ==(Color a, Color b)
		{
			return (a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a);
		}

		public static bool operator !=(Color a, Color b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			return $"[ {r} , {g} , {b} , {a} ]";
		}
	}
}
