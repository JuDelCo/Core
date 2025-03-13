// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Globalization;

namespace Ju.Data.Conversion
{
	using Ju.Color;

	public static class CastSourceStringExtensions
	{
		public static bool AsBool(this CastSource<string> source, bool defaultValue = default(bool))
		{
			return (bool.TryParse(source.value, out bool result)) ? result : defaultValue;
		}

		public static byte AsByte(this CastSource<string> source, byte defaultValue = default(byte))
		{
			return (byte.TryParse(source.value, out byte result)) ? result : defaultValue;
		}

		public static sbyte AsSByte(this CastSource<string> source, sbyte defaultValue = default(sbyte))
		{
			return (sbyte.TryParse(source.value, out sbyte result)) ? result : defaultValue;
		}

		public static char AsChar(this CastSource<string> source, char defaultValue = default(char))
		{
			return (char.TryParse(source.value, out char result)) ? result : defaultValue;
		}

		public static float AsSingle(this CastSource<string> source, float defaultValue = default(float))
		{
			return (float.TryParse(source.value, out float result)) ? result : defaultValue;
		}

		public static float AsFloat(this CastSource<string> source, float defaultValue = default(float))
		{
			return source.AsSingle(defaultValue);
		}

		public static double AsDouble(this CastSource<string> source, double defaultValue = default(double))
		{
			return (double.TryParse(source.value, out double result)) ? result : defaultValue;
		}

		public static double AsFloat64(this CastSource<string> source, double defaultValue = default(double))
		{
			return source.AsDouble(defaultValue);
		}

		public static decimal AsDecimal(this CastSource<string> source, decimal defaultValue = default(decimal))
		{
			return (decimal.TryParse(source.value, out decimal result)) ? result : defaultValue;
		}

		public static decimal AsFloat128(this CastSource<string> source, decimal defaultValue = default(decimal))
		{
			return source.AsDecimal(defaultValue);
		}

		public static short AsShort(this CastSource<string> source, short defaultValue = default(short))
		{
			return (short.TryParse(source.value, out short result)) ? result : defaultValue;
		}

		public static short AsInt16(this CastSource<string> source, short defaultValue = default(short))
		{
			return source.AsShort(defaultValue);
		}

		public static int AsInt(this CastSource<string> source, int defaultValue = default(int))
		{
			return (int.TryParse(source.value, out int result)) ? result : defaultValue;
		}

		public static int AsInt32(this CastSource<string> source, int defaultValue = default(int))
		{
			return source.AsInt(defaultValue);
		}

		public static long AsLong(this CastSource<string> source, long defaultValue = default(long))
		{
			return (long.TryParse(source.value, out long result)) ? result : defaultValue;
		}

		public static long AsInt64(this CastSource<string> source, long defaultValue = default(long))
		{
			return source.AsLong(defaultValue);
		}

		public static ushort AsUShort(this CastSource<string> source, ushort defaultValue = default(ushort))
		{
			return (ushort.TryParse(source.value, out ushort result)) ? result : defaultValue;
		}

		public static ushort AsUInt16(this CastSource<string> source, ushort defaultValue = default(ushort))
		{
			return source.AsUShort(defaultValue);
		}

		public static uint AsUInt(this CastSource<string> source, uint defaultValue = default(uint))
		{
			return (uint.TryParse(source.value, out uint result)) ? result : defaultValue;
		}

		public static uint AsUInt32(this CastSource<string> source, uint defaultValue = default(uint))
		{
			return source.AsUInt(defaultValue);
		}

		public static ulong AsULong(this CastSource<string> source, ulong defaultValue = default(ulong))
		{
			return (ulong.TryParse(source.value, out ulong result)) ? result : defaultValue;
		}

		public static ulong AsUInt64(this CastSource<string> source, ulong defaultValue = default(ulong))
		{
			return source.AsULong(defaultValue);
		}

		public static T AsEnum<T>(this CastSource<string> source, T defaultValue = default(T)) where T : struct, Enum
		{
			return (Enum.TryParse(source.value, true, out T result)) ? result : defaultValue;
		}

		public static DateTime AsDateTimeFromUnixTimeStamp(this CastSource<string> source, DateTime defaultValue = default(DateTime))
		{
			var defaultValueDouble = Cast.This(defaultValue).AsUnixTimeStamp();

			return Cast.This(source.AsDouble(defaultValueDouble)).AsDateTimeFromUnixTimeStamp();
		}

		public static Guid AsGuid(this CastSource<string> source, Guid defaultValue = default(Guid))
		{
			var result = defaultValue;

			try
			{
				result = new Guid(source.value);
			}
			catch
			{
			}

			return result;
		}

		public static Color AsColor(this CastSource<string> source, Color defaultValue = default(Color))
		{
			var value = source.value;

			if (source.value.Length <= 8 && !source.value.StartsWith("#"))
			{
				value = "#" + source.value;
			}

			return (TryParseColor(value, out Color result)) ? result : defaultValue;
		}

		public static Color32 AsColor32(this CastSource<string> source, Color32 defaultValue = default(Color32))
		{
			return source.AsColor(defaultValue);
		}

		internal static bool TryParseColor(string value, out Color result)
		{
			result = default(Color);

			if (string.IsNullOrEmpty(value))
			{
				return false;
			}

			value = value.Trim();

			if (value.StartsWith("#"))
			{
				value = value.TrimStart('#');
			}

			var parseResult = true;
			var numberStyle = NumberStyles.HexNumber;
			var formatProvider = CultureInfo.InvariantCulture;

			if (value.Length == 6 || value.Length == 8)
			{
				parseResult &= byte.TryParse(value.Substring(0, 2), numberStyle, formatProvider, out byte r);
				parseResult &= byte.TryParse(value.Substring(2, 2), numberStyle, formatProvider, out byte g);
				parseResult &= byte.TryParse(value.Substring(4, 2), numberStyle, formatProvider, out byte b);
				byte a = 255;

				if (value.Length == 8)
				{
					parseResult &= byte.TryParse(value.Substring(6, 2), numberStyle, formatProvider, out a);
				}

				if (parseResult)
				{
					result = new Color32(r, g, b, a);

					return true;
				}
			}
			else if (value.Length == 3)
			{
				parseResult &= byte.TryParse(new string(value.Substring(0, 1)[0], 2), numberStyle, formatProvider, out byte r);
				parseResult &= byte.TryParse(new string(value.Substring(1, 1)[0], 2), numberStyle, formatProvider, out byte g);
				parseResult &= byte.TryParse(new string(value.Substring(2, 1)[0], 2), numberStyle, formatProvider, out byte b);

				if (parseResult)
				{
					result = new Color32(r, g, b);

					return true;
				}
			}

			return false;
		}
	}
}
