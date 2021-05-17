// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Hjson
{
	public static class JsonObjectExtensions
	{
		public static string GetValue(this JsonObject obj, string key, string defaultValue = default(string))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static T GetValue<T>(this JsonObject obj, string key, T defaultValue = default(T)) where T : struct, Enum
		{
			return obj[key].GetValue(defaultValue);
		}

		public static bool GetValue(this JsonObject obj, string key, bool defaultValue = default(bool))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static byte GetValue(this JsonObject obj, string key, byte defaultValue = default(byte))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static sbyte GetValue(this JsonObject obj, string key, sbyte defaultValue = default(sbyte))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static char GetValue(this JsonObject obj, string key, char defaultValue = default(char))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static float GetValue(this JsonObject obj, string key, float defaultValue = default(float))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static double GetValue(this JsonObject obj, string key, double defaultValue = default(double))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static decimal GetValue(this JsonObject obj, string key, decimal defaultValue = default(decimal))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static short GetValue(this JsonObject obj, string key, short defaultValue = default(short))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static int GetValue(this JsonObject obj, string key, int defaultValue = default(int))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static long GetValue(this JsonObject obj, string key, long defaultValue = default(long))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static ushort GetValue(this JsonObject obj, string key, ushort defaultValue = default(ushort))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static uint GetValue(this JsonObject obj, string key, uint defaultValue = default(uint))
		{
			return obj[key].GetValue(defaultValue);
		}

		public static ulong GetValue(this JsonObject obj, string key, ulong defaultValue = default(ulong))
		{
			return obj[key].GetValue(defaultValue);
		}
	}
}
