// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;
using Ju.Extensions;

namespace Ju.Random
{
	public static partial class Random
	{
		public static void ListShuffle<T>(IList<T> list, System.Random random = null)
		{
			for (var i = list.Count; i > 0; i--)
			{
				var key = Int(i, random);
				(list[i - 1], list[key]) = (list[key], list[i - 1]);
			}
		}

		public static T ListElement<T>(IList<T> list, System.Random random = null)
		{
			return list[Int(list.Count, random)];
		}

		public static IEnumerable<T> ListElements<T>(IList<T> list, int iterations, System.Random random = null)
		{
			var count = list.Count;

			for (int i = 0; i < iterations; i++)
			{
				yield return list[Int(count, random)];
			}
		}

		public static IEnumerable<T> ListElementsUnique<T>(IList<T> list, System.Random random = null)
		{
			var tempList = new List<T>(list);

			ListShuffle(tempList, random);

			for (int i = (tempList.Count - 1); i >= 0; i--)
			{
				var value = tempList[i];
				tempList.RemoveAt(i);

				yield return value;
			}
		}

		public static IEnumerable<T> ListElementsUnique<T>(IList<T> list, int maxIterations, System.Random random = null)
		{
			var tempList = new List<T>(list);

			ListShuffle(tempList, random);

			var count = tempList.Count;
			maxIterations = System.Math.Min(count, System.Math.Max(0, maxIterations));
			var minIndex = count - maxIterations;

			for (int i = (count - 1); i >= minIndex; i--)
			{
				var value = tempList[i];
				tempList.RemoveAt(i);

				yield return value;
			}
		}

		public static TValue DictionaryElement<TKey, TValue>(IDictionary<TKey, TValue> dictionary, System.Random random = null)
		{
			var values = dictionary.Values.ToList();
			var count = dictionary.Count;

			return values[Int(count, random)];
		}

		public static IEnumerable<TValue> DictionaryElements<TKey, TValue>(IDictionary<TKey, TValue> dictionary, int iterations, System.Random random = null)
		{
			var values = dictionary.Values.ToList();
			var count = dictionary.Count;

			for (int i = 0; i < iterations; i++)
			{
				yield return values[Int(count, random)];
			}
		}

		public static IEnumerable<TValue> DictionaryElementsUnique<TKey, TValue>(IDictionary<TKey, TValue> dictionary, System.Random random = null)
		{
			var tempDictionary = new Dictionary<TKey, TValue>(dictionary);

			for (int i = tempDictionary.Count; i > 0; i--)
			{
				var key = tempDictionary.Keys.ElementAt(Int(i, random));
				var value = tempDictionary[key];
				tempDictionary.Remove(key);

				yield return value;
			}
		}

		public static IEnumerable<TValue> DictionaryElementsUnique<TKey, TValue>(IDictionary<TKey, TValue> dictionary, int maxIterations, System.Random random = null)
		{
			var tempDictionary = new Dictionary<TKey, TValue>(dictionary);

			var count = tempDictionary.Count;
			maxIterations = System.Math.Min(count, System.Math.Max(0, maxIterations));
			var minIndex = count - maxIterations;

			for (int i = count; i > minIndex; i--)
			{
				var key = tempDictionary.Keys.ElementAt(Int(i, random));
				var value = tempDictionary[key];
				tempDictionary.Remove(key);

				yield return value;
			}
		}
	}
}
