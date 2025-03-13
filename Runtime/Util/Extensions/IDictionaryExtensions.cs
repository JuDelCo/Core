// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.Extensions
{
	public static class IDictionaryExtensions
	{
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue defaultValue = default(TValue))
		{
			return self.TryGetValue(key, out TValue value) ? value : defaultValue;
		}

		public static TValue GetOrInsertDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue defaultValue = default(TValue))
		{
			if (!self.TryGetValue(key, out TValue value))
			{
				value = defaultValue;
				self[key] = value;
			}

			return value;
		}

		public static TValue GetOrInsertNew<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key) where TValue : new()
		{
			if (!self.TryGetValue(key, out TValue value))
			{
				value = new TValue();
				self[key] = value;
			}

			return value;
		}

		public static void ForEachKey<TKey, TValue>(this IDictionary<TKey, TValue> self, Action<TKey> action)
		{
			self.ForEach(kvp =>
			{
				action(kvp.Key);
			});
		}

		public static void ForEachKeyReverse<TKey, TValue>(this IDictionary<TKey, TValue> self, Action<TKey> action)
		{
			self.ForEachReverse(kvp =>
			{
				action(kvp.Key);
			});
		}

		public static void ForEachValue<TKey, TValue>(this IDictionary<TKey, TValue> self, Action<TValue> action)
		{
			self.ForEach(kvp =>
			{
				action(kvp.Value);
			});
		}

		public static void ForEachValueReverse<TKey, TValue>(this IDictionary<TKey, TValue> self, Action<TValue> action)
		{
			self.ForEachReverse(kvp =>
			{
				action(kvp.Value);
			});
		}
	}
}
