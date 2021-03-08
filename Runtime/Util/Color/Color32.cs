// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Color
{
	public partial struct Color32
	{
		private byte rValue;
		private byte gValue;
		private byte bValue;
		private byte aValue;

		public Color32(byte r, byte g, byte b)
		{
			rValue = Clamp(r);
			gValue = Clamp(g);
			bValue = Clamp(b);
			aValue = 255;
		}

		public Color32(byte r, byte g, byte b, byte a)
		{
			rValue = Clamp(r);
			gValue = Clamp(g);
			bValue = Clamp(b);
			aValue = Clamp(a);
		}

#pragma warning disable IDE1006

		public byte r
		{
			get { return rValue; }
			set { rValue = Clamp(value); }
		}

		public byte g
		{
			get { return gValue; }
			set { gValue = Clamp(value); }
		}

		public byte b
		{
			get { return bValue; }
			set { bValue = Clamp(value); }
		}

		public byte a
		{
			get { return aValue; }
			set { aValue = Clamp(value); }
		}

#pragma warning restore IDE1006

		public static Color32 operator +(Color32 a, Color32 b)
		{
			return new Color32((byte)(a.r + b.r), (byte)(a.g + b.g), (byte)(a.b + b.b), (byte)(a.a + b.a));
		}

		public static Color32 operator -(Color32 a, Color32 b)
		{
			return new Color32((byte)(a.r - b.r), (byte)(a.g - b.g), (byte)(a.b - b.b), (byte)(a.a - b.a));
		}

		public static Color32 operator *(Color32 a, Color32 b)
		{
			return new Color32((byte)(a.r * (b.r / 255f)), (byte)(a.g * (b.g / 255f)), (byte)(a.b * (b.b / 255f)), (byte)(a.a * (b.a / 255f)));
		}

		public static Color32 operator *(Color32 c, byte v)
		{
			float value = (v / 255f);
			return new Color32((byte)(c.r * value), (byte)(c.g * value), (byte)(c.b * value), (byte)(c.a * value));
		}

		public static Color32 operator *(byte v, Color32 c)
		{
			float value = (v / 255f);
			return new Color32((byte)(c.r * value), (byte)(c.g * value), (byte)(c.b * value), (byte)(c.a * value));
		}

		public static Color32 operator *(Color32 c, float v)
		{
			float value = Clamp01(v);
			return new Color32((byte)(c.r * value), (byte)(c.g * value), (byte)(c.b * value), (byte)(c.a * value));
		}

		public static Color32 operator *(float v, Color32 c)
		{
			float value = Clamp01(v);
			return new Color32((byte)(c.r * value), (byte)(c.g * value), (byte)(c.b * value), (byte)(c.a * value));
		}

		public static Color32 operator /(Color32 a, Color32 b)
		{
			return new Color32((byte)(a.r / (b.r / 255f)), (byte)(a.g / (b.g / 255f)), (byte)(a.b / (b.b / 255f)), (byte)(a.a / (b.a / 255f)));
		}

		public static Color32 operator /(Color32 c, byte v)
		{
			float value = (v / 255f);
			return new Color32((byte)(c.r / value), (byte)(c.g / value), (byte)(c.b / value), (byte)(c.a / value));
		}

		public static Color32 operator /(byte v, Color32 c)
		{
			float value = (v / 255f);
			return new Color32((byte)(value / c.r), (byte)(value / c.g), (byte)(value / c.b), (byte)(value / c.a));
		}

		public static Color32 operator /(Color32 c, float v)
		{
			float value = Clamp01(v);
			return new Color32((byte)(c.r / value), (byte)(c.g / value), (byte)(c.b / value), (byte)(c.a / value));
		}

		public static Color32 operator /(float v, Color32 c)
		{
			float value = Clamp01(v);
			return new Color32((byte)(value / c.r), (byte)(value / c.g), (byte)(value / c.b), (byte)(value / c.a));
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

		public override bool Equals(object obj)
		{
			return (obj is Color32 color && (this == color));
		}

		public static bool operator ==(Color32 a, Color32 b)
		{
			return (a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a);
		}

		public static bool operator !=(Color32 a, Color32 b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			return $"[ {r} , {g} , {b} , {a} ]";
		}

		private static byte Clamp(byte value)
		{
			return (byte)System.Math.Min(255, System.Math.Max(0, (int)value));
		}

		private static float Clamp01(float value)
		{
			return System.Math.Min(1f, System.Math.Max(0f, value));
		}
	}
}
