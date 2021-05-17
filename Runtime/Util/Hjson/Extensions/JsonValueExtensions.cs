// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Data.Conversion;

namespace Ju.Hjson
{
	public static class JsonValueExtensions
	{
		public static string GetValue(this JsonValue value, string defaultValue = default(string))
		{
			return value.Qstr();
		}

		public static T GetValue<T>(this JsonValue value, T defaultValue = default(T)) where T : struct, Enum
		{
			return Cast.This(value.Qstr()).AsEnum(defaultValue);
		}

		public static bool GetValue(this JsonValue value, bool defaultValue = default(bool))
		{
			return Cast.This(value.Qstr()).AsBool(defaultValue);
		}

		public static byte GetValue(this JsonValue value, byte defaultValue = default(byte))
		{
			return Cast.This(value.Qstr()).AsByte(defaultValue);
		}

		public static sbyte GetValue(this JsonValue value, sbyte defaultValue = default(sbyte))
		{
			return Cast.This(value.Qstr()).AsSByte(defaultValue);
		}

		public static char GetValue(this JsonValue value, char defaultValue = default(char))
		{
			return Cast.This(value.Qstr()).AsChar(defaultValue);
		}

		public static float GetValue(this JsonValue value, float defaultValue = default(float))
		{
			return Cast.This(value.Qstr()).AsSingle(defaultValue);
		}

		public static double GetValue(this JsonValue value, double defaultValue = default(double))
		{
			return Cast.This(value.Qstr()).AsDouble(defaultValue);
		}

		public static decimal GetValue(this JsonValue value, decimal defaultValue = default(decimal))
		{
			return Cast.This(value.Qstr()).AsDecimal(defaultValue);
		}

		public static short GetValue(this JsonValue value, short defaultValue = default(short))
		{
			return Cast.This(value.Qstr()).AsShort(defaultValue);
		}

		public static int GetValue(this JsonValue value, int defaultValue = default(int))
		{
			return Cast.This(value.Qstr()).AsInt(defaultValue);
		}

		public static long GetValue(this JsonValue value, long defaultValue = default(long))
		{
			return Cast.This(value.Qstr()).AsLong(defaultValue);
		}

		public static ushort GetValue(this JsonValue value, ushort defaultValue = default(ushort))
		{
			return Cast.This(value.Qstr()).AsUShort(defaultValue);
		}

		public static uint GetValue(this JsonValue value, uint defaultValue = default(uint))
		{
			return Cast.This(value.Qstr()).AsUInt(defaultValue);
		}

		public static ulong GetValue(this JsonValue value, ulong defaultValue = default(ulong))
		{
			return Cast.This(value.Qstr()).AsULong(defaultValue);
		}
	}
}
