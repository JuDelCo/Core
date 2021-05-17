// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Data.Conversion
{
	public static class JDataStringExtensions
	{
		public static T AsEnum<T>(this JData<string> data, T defaultValue = default(T)) where T : struct, Enum
		{
			return Cast.This(data.Value).AsEnum(defaultValue);
		}

		public static bool AsBool(this JData<string> data, bool defaultValue = default(bool))
		{
			return Cast.This(data.Value).AsBool(defaultValue);
		}

		public static byte AsByte(this JData<string> data, byte defaultValue = default(byte))
		{
			return Cast.This(data.Value).AsByte(defaultValue);
		}

		public static sbyte AsSByte(this JData<string> data, sbyte defaultValue = default(sbyte))
		{
			return Cast.This(data.Value).AsSByte(defaultValue);
		}

		public static char AsChar(this JData<string> data, char defaultValue = default(char))
		{
			return Cast.This(data.Value).AsChar(defaultValue);
		}

		public static float AsSingle(this JData<string> data, float defaultValue = default(float))
		{
			return Cast.This(data.Value).AsSingle(defaultValue);
		}

		public static float AsFloat(this JData<string> data, float defaultValue = default(float))
		{
			return Cast.This(data.Value).AsFloat(defaultValue);
		}

		public static double AsDouble(this JData<string> data, double defaultValue = default(double))
		{
			return Cast.This(data.Value).AsDouble(defaultValue);
		}

		public static double AsFloat64(this JData<string> data, double defaultValue = default(double))
		{
			return Cast.This(data.Value).AsFloat64(defaultValue);
		}

		public static decimal AsDecimal(this JData<string> data, decimal defaultValue = default(decimal))
		{
			return Cast.This(data.Value).AsDecimal(defaultValue);
		}

		public static decimal AsFloat128(this JData<string> data, decimal defaultValue = default(decimal))
		{
			return Cast.This(data.Value).AsFloat128(defaultValue);
		}

		public static short AsShort(this JData<string> data, short defaultValue = default(short))
		{
			return Cast.This(data.Value).AsShort(defaultValue);
		}

		public static short AsInt16(this JData<string> data, short defaultValue = default(short))
		{
			return Cast.This(data.Value).AsInt16(defaultValue);
		}

		public static int AsInt(this JData<string> data, int defaultValue = default(int))
		{
			return Cast.This(data.Value).AsInt(defaultValue);
		}

		public static int AsInt32(this JData<string> data, int defaultValue = default(int))
		{
			return Cast.This(data.Value).AsInt32(defaultValue);
		}

		public static long AsLong(this JData<string> data, long defaultValue = default(long))
		{
			return Cast.This(data.Value).AsLong(defaultValue);
		}

		public static long AsInt64(this JData<string> data, long defaultValue = default(long))
		{
			return Cast.This(data.Value).AsInt64(defaultValue);
		}

		public static ushort AsUShort(this JData<string> data, ushort defaultValue = default(ushort))
		{
			return Cast.This(data.Value).AsUShort(defaultValue);
		}

		public static ushort AsUInt16(this JData<string> data, ushort defaultValue = default(ushort))
		{
			return Cast.This(data.Value).AsUInt16(defaultValue);
		}

		public static uint AsUInt(this JData<string> data, uint defaultValue = default(uint))
		{
			return Cast.This(data.Value).AsUInt(defaultValue);
		}

		public static uint AsUInt32(this JData<string> data, uint defaultValue = default(uint))
		{
			return Cast.This(data.Value).AsUInt32(defaultValue);
		}

		public static ulong AsULong(this JData<string> data, ulong defaultValue = default(ulong))
		{
			return Cast.This(data.Value).AsULong(defaultValue);
		}

		public static ulong AsUInt64(this JData<string> data, ulong defaultValue = default(ulong))
		{
			return Cast.This(data.Value).AsUInt64(defaultValue);
		}

		public static DateTime AsDateTimeFromUnixTimeStamp(this JData<string> data)
		{
			return Cast.This(data.Value).AsDateTimeFromUnixTimeStamp();
		}

		public static void Set<T>(this JData<string> data, T value) where T : Enum
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, bool value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, byte value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, sbyte value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, char value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, float value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, double value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, decimal value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, short value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, int value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, long value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, ushort value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, uint value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, ulong value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, DateTime value, string format = "yyyyMMdd_HHmmss")
		{
			data.Value = Cast.This(value).AsString(format);
		}
	}
}
