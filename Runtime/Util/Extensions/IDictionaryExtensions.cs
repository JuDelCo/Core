// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.Extensions
{
	public static class IDictionaryExtensions
	{
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue defaultValue = default(TValue))
		{
			TValue value;
			return self.TryGetValue(key, out value) ? value : defaultValue;
		}

		public static TValue GetOrInsertDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue defaultValue = default(TValue))
		{
			TValue value;

			if (!self.TryGetValue(key, out value))
			{
				value = defaultValue;
				self[key] = value;
			}

			return value;
		}

		public static TValue GetOrInsertNew<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key) where TValue : new()
		{
			TValue value;

			if (!self.TryGetValue(key, out value))
			{
				value = new TValue();
				self[key] = value;
			}

			return value;
		}
	}
}
