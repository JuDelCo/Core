// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System.Collections.Generic;

namespace Ju.Extensions
{
	public static class RandomCollectionExtensions
	{
		public static void Shuffle<T>(this IList<T> list, System.Random random = null)
		{
			Ju.Random.Random.ListShuffle(list, random);
		}

		public static T RandomElement<T>(this IList<T> list, System.Random random = null)
		{
			return Ju.Random.Random.ListElement(list, random);
		}

		public static IEnumerable<T> RandomElements<T>(this IList<T> list, int iterations, System.Random random = null)
		{
			return Ju.Random.Random.ListElements(list, iterations, random);
		}

		public static IEnumerable<T> RandomElementsUnique<T>(this IList<T> list, System.Random random = null)
		{
			return Ju.Random.Random.ListElementsUnique(list, random);
		}

		public static IEnumerable<T> RandomElementsUnique<T>(this IList<T> list, int maxIterations, System.Random random = null)
		{
			return Ju.Random.Random.ListElementsUnique(list, maxIterations, random);
		}

		public static T RandomRemoveElement<T>(this IList<T> list, System.Random random = null)
		{
			return Ju.Random.Random.ListRemoveElement(list, random);
		}

		public static TValue RandomElement<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, System.Random random = null)
		{
			return Ju.Random.Random.DictionaryElement(dictionary, random);
		}

		public static IEnumerable<TValue> RandomElements<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, int iterations, System.Random random = null)
		{
			return Ju.Random.Random.DictionaryElements(dictionary, iterations, random);
		}

		public static IEnumerable<TValue> RandomElementsUnique<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, System.Random random = null)
		{
			return Ju.Random.Random.DictionaryElementsUnique(dictionary, random);
		}

		public static IEnumerable<TValue> RandomElementsUnique<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, int maxIterations, System.Random random = null)
		{
			return Ju.Random.Random.DictionaryElementsUnique(dictionary, maxIterations, random);
		}

		public static TValue RandomRemoveElement<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, System.Random random = null)
		{
			return Ju.Random.Random.DictionaryRemoveElement(dictionary, random);
		}
	}
}
