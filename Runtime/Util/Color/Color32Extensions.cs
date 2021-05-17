// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Color
{
	public partial struct Color32
	{
		public static Color32 beige = new Color32(211, 176, 131, 255);
		public static Color32 black = new Color32(0, 0, 0, 255);
		public static Color32 blue = new Color32(0, 121, 241, 255);
		public static Color32 brown = new Color32(127, 106, 79, 255);
		public static Color32 clear = new Color32(0, 0, 0, 0);
		public static Color32 cyan = new Color32(0, 255, 255, 255);
		public static Color32 darkblue = new Color32(0, 82, 172, 255);
		public static Color32 darkbrown = new Color32(76, 63, 47, 255);
		public static Color32 darkgray = new Color32(80, 80, 80, 255);
		public static Color32 darkgreen = new Color32(0, 117, 44, 255);
		public static Color32 darkpurple = new Color32(112, 31, 126, 255);
		public static Color32 gold = new Color32(255, 203, 0, 255);
		public static Color32 gray = new Color32(130, 130, 130, 255);
		public static Color32 green = new Color32(0, 228, 48, 255);
		public static Color32 lightgray = new Color32(200, 200, 200, 255);
		public static Color32 lime = new Color32(0, 158, 47, 255);
		public static Color32 magenta = new Color32(255, 0, 255, 255);
		public static Color32 maroon = new Color32(190, 33, 55, 255);
		public static Color32 orange = new Color32(255, 161, 0, 255);
		public static Color32 pink = new Color32(255, 109, 194, 255);
		public static Color32 purple = new Color32(200, 122, 255, 255);
		public static Color32 red = new Color32(230, 41, 55, 255);
		public static Color32 skyblue = new Color32(102, 191, 255, 255);
		public static Color32 violet = new Color32(135, 60, 190, 255);
		public static Color32 white = new Color32(255, 255, 255, 255);
		public static Color32 yellow = new Color32(253, 249, 0, 255);

		public Color32(byte brightness)
		{
			rValue = Clamp(brightness);
			gValue = Clamp(brightness);
			bValue = Clamp(brightness);
			aValue = 255;
		}

		public Color32(int r, int g, int b, int a)
		{
			rValue = Clamp((byte)r);
			gValue = Clamp((byte)g);
			bValue = Clamp((byte)b);
			aValue = Clamp((byte)a);
		}

		public Color32(float r, float g, float b, float a)
		{
			rValue = Clamp((byte)(r * 255f));
			gValue = Clamp((byte)(g * 255f));
			bValue = Clamp((byte)(b * 255f));
			aValue = Clamp((byte)(a * 255f));
		}

		// Do not use max saturation or value
		public static Color32 FromHSV(float hue, float saturation, float value, float alpha)
		{
			if (saturation == 0f)
			{
				return new Color(value, value, value, alpha);
			}

			float max = value < 0.5f ? value * (1f + saturation) : (value + saturation) - (value * saturation);
			float min = (value * 2f) - max;

			return new Color32(
				RGBChannelFromHue(min, max, hue + (1 / 3f)) * 255f,
				RGBChannelFromHue(min, max, hue) * 255f,
				RGBChannelFromHue(min, max, hue - (1 / 3f)) * 255f,
				alpha * 255f
			);
		}

		private static float RGBChannelFromHue(float min, float max, float hue)
		{
			if (hue < 0)
			{
				hue += 1f;
			}
			else if (hue > 1)
			{
				hue -= 1f;
			}

			if (hue * 6 < 1)
			{
				return min + (max - min) * 6 * hue;
			}
			else if (hue * 2 < 1)
			{
				return max;
			}
			else if (hue * 3 < 2)
			{
				return min + (max - min) * 6 * (2f / 3f - hue);
			}

			return min;
		}

		public static implicit operator Color32(Color color)
		{
			return new Color32((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
		}

		public static implicit operator Color(Color32 color)
		{
			return new Color(color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
		}

		private const string hexCharacters = "0123456789ABCDEF";

		private static byte HexToByte(char c)
		{
			return (byte)hexCharacters.IndexOf(char.ToUpper(c));
		}

		public static Color32 HexToColor(string hex)
		{
			int r = HexToByte(hex[0]) * 16 + HexToByte(hex[1]);
			int g = HexToByte(hex[2]) * 16 + HexToByte(hex[3]);
			int b = HexToByte(hex[4]) * 16 + HexToByte(hex[5]);

			return new Color32(r, g, b, 255);
		}

		public static Color32 HexToColorAlpha(string hex)
		{
			int r = HexToByte(hex[0]) * 16 + HexToByte(hex[1]);
			int g = HexToByte(hex[2]) * 16 + HexToByte(hex[3]);
			int b = HexToByte(hex[4]) * 16 + HexToByte(hex[5]);
			int a = HexToByte(hex[6]) * 16 + HexToByte(hex[7]);

			return new Color32(r, g, b, a);
		}

		public static Color32 HexToColor(int hex)
		{
			var r = (hex & 0x000000FF);
			var g = (hex & 0x0000FF00) >> 8;
			var b = (hex & 0x00FF0000) >> 16;

			return new Color32(r, g, b, 255);
		}

		public static Color32 HexToColorAlpha(int hex)
		{
			var r = (hex & 0x000000FF);
			var g = (hex & 0x0000FF00) >> 8;
			var b = (hex & 0x00FF0000) >> 16;
			var a = (hex & 0xFF000000) >> 24;

			return new Color32(r, g, b, a);
		}

		public static Color32 Lerp(Color32 a, Color32 b, float alpha, bool extrapolate = false)
		{
			alpha = extrapolate ? alpha : Clamp01(alpha);

			return new Color32(
				(a.r * (1f - alpha) + b.r * alpha),
				(a.g * (1f - alpha) + b.g * alpha),
				(a.b * (1f - alpha) + b.b * alpha),
				(a.a * (1f - alpha) + b.a * alpha)
			);
		}
	}

	public static class Color32Extensions
	{
		public static float Brightness(this Color32 color)
		{
			float red = color.r;
			float green = color.g;
			float blue = color.b;
			float max = Max(red, green, blue);

			return max / 255f;
		}

		public static float BrightnessPerceived(this Color32 color)
		{
			// Photometric/digital ITU BT.709
			// return (color.r * 0.2126f + color.g * 0.7152f + color.b * 0.0722f);

			// HSP Color model
			// return Math.Sqrt(Math.Pow(color.r, 2) * 0.299f + Math.Pow(color.g, 2) * 0.587f + Math.Pow(color.b, 2) * 0.114f);

			// Digital ITU BT.601
			return (color.r * 0.299f + color.g * 0.587f + color.b * 0.114f);
		}

		public static Color32 Grayscale(this Color32 color)
		{
			return new Color32(
				(color.r * 0.299f + color.g * 0.587f + color.b * 0.114f),
				(color.r * 0.299f + color.g * 0.587f + color.b * 0.114f),
				(color.r * 0.299f + color.g * 0.587f + color.b * 0.114f),
				color.a
			);
		}

		public static float Hue(this Color32 color)
		{
			float hueValue = 0f;
			float red = color.r / 255f;
			float green = color.g / 255f;
			float blue = color.b / 255f;
			float min = Min(red, green, blue);
			float max = Max(red, green, blue);

			if (red == max && blue == min)
			{
				hueValue = green / 6;
			}
			else if (green == max && blue == min)
			{
				hueValue = (red - 2) / -6;
			}
			else if (red == min && green == max)
			{
				hueValue = (blue + 2) / 6;
			}
			else if (red == min && blue == max)
			{
				hueValue = (green - 4) / -6;
			}
			else if (green == min && blue == max)
			{
				hueValue = (red + 4) / 6;
			}
			else if (red == max && green == min)
			{
				hueValue = (blue - 6) / -6;
			}

			return hueValue;
		}

		public static Color32 Invert(this Color32 color)
		{
			return new Color32(255 - color.r, 255 - color.g, 255 - color.b, color.a);
		}

		public static float Saturation(this Color32 color)
		{
			float saturation = 0f;
			float red = color.r / 255f;
			float green = color.g / 255f;
			float blue = color.b / 255f;
			float min = Min(red, green, blue);
			float max = Max(red, green, blue);

			if (max > 0)
			{
				saturation = (max - min) / max;
			}

			return saturation;
		}

		private static float Min(float a, float b, float c)
		{
			return Math.Min(a, Math.Min(b, c));
		}

		private static float Max(float a, float b, float c)
		{
			return Math.Max(a, Math.Max(b, c));
		}
	}
}
