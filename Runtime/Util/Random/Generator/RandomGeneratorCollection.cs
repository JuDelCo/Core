// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;

namespace Ju.Random
{
	public partial class RandomGenerator
	{
		public void ListShuffle<T>(IList<T> list)
		{
			Random.ListShuffle(list, random);
		}

		public T ListElement<T>(IList<T> list)
		{
			return Random.ListElement(list, random);
		}

		public IEnumerable<T> ListElements<T>(IList<T> list, int iterations)
		{
			return Random.ListElements(list, iterations, random);
		}

		public IEnumerable<T> ListElementsUnique<T>(IList<T> list)
		{
			return Random.ListElementsUnique(list, random);
		}

		public IEnumerable<T> ListElementsUnique<T>(IList<T> list, int maxIterations)
		{
			return Random.ListElementsUnique(list, maxIterations, random);
		}

		public TValue DictionaryElement<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
		{
			return Random.DictionaryElement(dictionary, random);
		}

		public IEnumerable<TValue> DictionaryElements<TKey, TValue>(IDictionary<TKey, TValue> dictionary, int iterations)
		{
			return Random.DictionaryElements(dictionary, iterations, random);
		}

		public IEnumerable<TValue> DictionaryElementsUnique<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
		{
			return Random.DictionaryElementsUnique(dictionary, random);
		}

		public IEnumerable<TValue> DictionaryElementsUnique<TKey, TValue>(IDictionary<TKey, TValue> dictionary, int maxIterations)
		{
			return Random.DictionaryElementsUnique(dictionary, maxIterations, random);
		}
	}
}
