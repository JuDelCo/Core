// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Color
{
	public partial struct Color
	{
		public static Color beige = new Color(0.82f, 0.69f, 0.51f, 1);
		public static Color black = new Color(0, 0, 0, 1);
		public static Color blue = new Color(0, 0.47f, 0.95f, 1);
		public static Color brown = new Color(0.50f, 0.42f, 0.31f, 1);
		public static Color clear = new Color(0, 0, 0, 0);
		public static Color cyan = new Color(0, 1, 1, 1);
		public static Color darkblue = new Color(0, 0.32f, 0.67f, 1);
		public static Color darkbrown = new Color(0.30f, 0.25f, 0.18f, 1);
		public static Color darkgray = new Color(0.31f, 0.31f, 0.31f, 1);
		public static Color darkgreen = new Color(0, 0.46f, 0.17f, 1);
		public static Color darkpurple = new Color(0.44f, 0.12f, 0.49f, 1);
		public static Color gold = new Color(1, 0.80f, 0, 1);
		public static Color gray = new Color(0.51f, 0.51f, 0.51f, 1);
		public static Color green = new Color(0, 0.89f, 0.19f, 1);
		public static Color lightgray = new Color(0.78f, 0.78f, 0.78f, 1);
		public static Color lime = new Color(0, 0.62f, 0.18f, 1);
		public static Color magenta = new Color(1, 0, 1, 1);
		public static Color maroon = new Color(0.75f, 0.13f, 0.22f, 1);
		public static Color orange = new Color(1, 0.63f, 0, 1);
		public static Color pink = new Color(1, 0.42f, 0.76f, 1);
		public static Color purple = new Color(0.78f, 0.48f, 1, 1);
		public static Color red = new Color(0.90f, 0.16f, 0.22f, 1);
		public static Color skyblue = new Color(0.40f, 0.75f, 1, 1);
		public static Color violet = new Color(0.53f, 0.24f, 0.75f, 1);
		public static Color white = new Color(1, 1, 1, 1);
		public static Color yellow = new Color(0.99f, 0.98f, 0, 1);

		public Color(float brightness)
		{
			rValue = brightness;
			gValue = brightness;
			bValue = brightness;
			aValue = 1f;
		}

		// Do not use max saturation or value
		public static Color FromHSV(float hue, float saturation, float value, float alpha)
		{
			if (saturation == 0f)
			{
				return new Color(value, value, value, alpha);
			}

			float max = value < 0.5f ? value * (1f + saturation) : (value + saturation) - (value * saturation);
			float min = (value * 2f) - max;

			return new Color(
				RGBChannelFromHue(min, max, hue + (1 / 3f)),
				RGBChannelFromHue(min, max, hue),
				RGBChannelFromHue(min, max, hue - (1 / 3f)),
				alpha
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

		private const string hexCharacters = "0123456789ABCDEF";

		private static byte HexToByte(char c)
		{
			return (byte)hexCharacters.IndexOf(char.ToUpper(c));
		}

		public static Color HexToColor(string hex)
		{
			float r = (HexToByte(hex[0]) * 16 + HexToByte(hex[1])) / 255f;
			float g = (HexToByte(hex[2]) * 16 + HexToByte(hex[3])) / 255f;
			float b = (HexToByte(hex[4]) * 16 + HexToByte(hex[5])) / 255f;

			return new Color(r, g, b, 1f);
		}

		public static Color HexToColorAlpha(string hex)
		{
			float r = (HexToByte(hex[0]) * 16 + HexToByte(hex[1])) / 255f;
			float g = (HexToByte(hex[2]) * 16 + HexToByte(hex[3])) / 255f;
			float b = (HexToByte(hex[4]) * 16 + HexToByte(hex[5])) / 255f;
			float a = (HexToByte(hex[6]) * 16 + HexToByte(hex[7])) / 255f;

			return new Color(r, g, b, a);
		}

		public static Color HexToColor(int hex)
		{
			var r = (hex & 0x000000FF);
			var g = (hex & 0x0000FF00) >> 8;
			var b = (hex & 0x00FF0000) >> 16;

			return new Color(r / 255f, g / 255f, b / 255f, 1f);
		}

		public static Color HexToColorAlpha(int hex)
		{
			var r = (hex & 0x000000FF);
			var g = (hex & 0x0000FF00) >> 8;
			var b = (hex & 0x00FF0000) >> 16;
			var a = (hex & 0xFF000000) >> 24;

			return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
		}

		public static Color Lerp(Color a, Color b, float alpha, bool extrapolate = false)
		{
			alpha = extrapolate ? alpha : ColorExtensions.Clamp01(alpha);

			return a * (1f - alpha) + b * alpha;
		}
	}

	public static class ColorExtensions
	{
		public static float Brightness(this Color color)
		{
			float red = color.r;
			float green = color.g;
			float blue = color.b;
			float max = Max(red, green, blue);

			return max;
		}

		public static float BrightnessPerceived(this Color color)
		{
			// Photometric/digital ITU BT.709
			// return (color.r * 0.2126f + color.g * 0.7152f + color.b * 0.0722f);

			// HSP Color model
			// return Math.Sqrt(Math.Pow(color.r, 2) * 0.299f + Math.Pow(color.g, 2) * 0.587f + Math.Pow(color.b, 2) * 0.114f);

			// Digital ITU BT.601
			return (color.r * 0.299f + color.g * 0.587f + color.b * 0.114f);
		}

		public static Color Grayscale(this Color color)
		{
			return new Color(
				(color.r * 0.299f + color.g * 0.587f + color.b * 0.114f),
				(color.r * 0.299f + color.g * 0.587f + color.b * 0.114f),
				(color.r * 0.299f + color.g * 0.587f + color.b * 0.114f),
				color.a
			);
		}

		public static float Hue(this Color color)
		{
			float hueValue = 0f;
			float red = color.r;
			float green = color.g;
			float blue = color.b;
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

		public static Color Invert(this Color color)
		{
			return new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
		}

		public static float Saturation(this Color color)
		{
			float saturation = 0f;
			float red = color.r;
			float green = color.g;
			float blue = color.b;
			float min = Min(red, green, blue);
			float max = Max(red, green, blue);

			if (max > 0)
			{
				saturation = (max - min) / max;
			}

			return saturation;
		}

		public static Color Clamp01(this Color color)
		{
			return new Color(Clamp01(color.r), Clamp01(color.g), Clamp01(color.b), Clamp01(color.a));
		}

		internal static float Clamp01(float value)
		{
			return Math.Min(1f, Math.Max(0f, value));
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
