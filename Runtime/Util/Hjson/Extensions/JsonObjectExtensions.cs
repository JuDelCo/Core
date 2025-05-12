// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Specialized;
using Ju.Handlers;
using Ju.FSM;
using Ju.Services;

namespace Ju.Hjson
{
	using Ju.Color;

	public static class JsonObjectExtensions
	{
		public static void Subscribe(this JsonObject obj, IService service, Action<NotifyCollectionChangedEventArgs> action)
		{
			obj.Subscribe(new ObjectLinkHandler<IService>(service), action);
		}

		public static void Subscribe(this JsonObject obj, State state, Action<NotifyCollectionChangedEventArgs> action)
		{
			obj.Subscribe(new StateLinkHandler(state), action);
		}

		public static bool HasValue(this JsonObject obj, string key)
		{
			return obj.ContainsKey(key);
		}

		public static string GetValue(this JsonObject obj, string key, string defaultValue = default(string))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static T GetValue<T>(this JsonObject obj, string key, T defaultValue = default(T)) where T : struct, Enum
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static bool GetValue(this JsonObject obj, string key, bool defaultValue = default(bool))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static byte GetValue(this JsonObject obj, string key, byte defaultValue = default(byte))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static sbyte GetValue(this JsonObject obj, string key, sbyte defaultValue = default(sbyte))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static char GetValue(this JsonObject obj, string key, char defaultValue = default(char))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static float GetValue(this JsonObject obj, string key, float defaultValue = default(float))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static double GetValue(this JsonObject obj, string key, double defaultValue = default(double))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static decimal GetValue(this JsonObject obj, string key, decimal defaultValue = default(decimal))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static short GetValue(this JsonObject obj, string key, short defaultValue = default(short))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static int GetValue(this JsonObject obj, string key, int defaultValue = default(int))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static long GetValue(this JsonObject obj, string key, long defaultValue = default(long))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static ushort GetValue(this JsonObject obj, string key, ushort defaultValue = default(ushort))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static uint GetValue(this JsonObject obj, string key, uint defaultValue = default(uint))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static ulong GetValue(this JsonObject obj, string key, ulong defaultValue = default(ulong))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static DateTime GetValue(this JsonObject obj, string key, DateTime defaultValue = default(DateTime))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static Guid GetValue(this JsonObject obj, string key, Guid defaultValue = default(Guid))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static Color GetValue(this JsonObject obj, string key, Color defaultValue = default(Color))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static Color32 GetValue(this JsonObject obj, string key, Color32 defaultValue = default(Color32))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}
	}
}
