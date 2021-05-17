// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Data.Conversion
{
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

		public static DateTime AsDateTimeFromUnixTimeStamp(this CastSource<string> source)
		{
			return Cast.This(source.AsDouble()).AsDateTimeFromUnixTimeStamp();
		}
	}
}
