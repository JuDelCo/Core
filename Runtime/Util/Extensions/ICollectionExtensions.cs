// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ju.Extensions
{
	public static class ICollectionExtensions
	{
		public static bool All<TSource>(this ICollection<TSource> self, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (self is IList<TSource> list)
			{
				var count = list.Count;

				for (int i = 0; i < count; ++i)
				{
					if (!predicate(list[i]))
					{
						return false;
					}
				}

				return true;
			}

			foreach (TSource element in self)
			{
				if (!predicate(element))
				{
					return false;
				}
			}

			return true;
		}

		public static bool Any<TSource>(this ICollection<TSource> self, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (self is IList<TSource> list)
			{
				var count = list.Count;

				for (int i = 0; i < count; ++i)
				{
					if (predicate(list[i]))
					{
						return true;
					}
				}

				return false;
			}

			foreach (TSource element in self)
			{
				if (predicate(element))
				{
					return true;
				}
			}

			return false;
		}

		public static IList<TResult> Cast<TResult>(this ICollection self)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			var result = new List<TResult>(self.Count);

			foreach (var item in self)
			{
				result.Add((TResult) item); // Can throw InvalidCastException
			}

			return result;
		}

		public static IList<TSource> Concatenate<TSource>(this ICollection<TSource> self, params ICollection<TSource>[] others)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (others == null)
			{
				throw new ArgumentNullException(nameof(others));
			}

			var capacity = self.Count;

			for (int i = 0; i < others.Length; ++i)
			{
				if (others[i] != null)
				{
					capacity += others[i].Count;
				}
			}

			var result = new List<TSource>(capacity);

			foreach (var item in self)
			{
				result.Add(item);
			}

			for (int i = 0; i < others.Length; ++i)
			{
				var otherCollection = others[i];

				if (otherCollection != null)
				{
					foreach (var item in otherCollection)
					{
						result.Add(item);
					}
				}
			}

			return result;
		}

		public static bool Contains<TSource>(this ICollection<TSource> self, TSource value)
		{
			return self.Contains(value);
		}

		public static int Count<TSource>(this ICollection<TSource> self)
		{
			return self.Count;
		}

		public static TSource ElementAt<TSource>(this ICollection<TSource> self, int index)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (self is IList<TSource> list)
			{
				if (index >= list.Count)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				return list[index];
			}

			int currentIndex = 0;

			foreach (TSource item in self)
			{
				if (currentIndex == index)
				{
					return item;
				}

				currentIndex++;
			}

			throw new ArgumentOutOfRangeException(nameof(index));
		}

		public static IList<TSource> Filter<TSource>(this ICollection<TSource> self, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			var result = new List<TSource>();

			foreach (var item in self)
			{
				if (predicate(item))
				{
					result.Add(item);
				}
			}

			return result;
		}

		public static TSource Find<TSource>(this ICollection<TSource> self, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (self is IList<TSource> list)
			{
				var count = list.Count;

				for (int i = 0; i < count; ++i)
				{
					var item = list[i];

					if (predicate(item))
					{
						return item;
					}
				}
			}
			else
			{
				foreach (TSource item in self)
				{
					if (predicate(item))
					{
						return item;
					}
				}
			}

			throw new InvalidOperationException("Sequence contains no matching element");
		}

		public static TSource Find<TSource>(this ICollection<TSource> self, TSource defaultValue, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			if (self is IList<TSource> list)
			{
				var count = list.Count;

				for (int i = 0; i < count; ++i)
				{
					var item = list[i];

					if (predicate(item))
					{
						return item;
					}
				}
			}
			else
			{
				foreach (var item in self)
				{
					if (predicate(item))
					{
						return item;
					}
				}
			}

			return defaultValue;
		}

		public static TSource FindUnique<TSource>(this ICollection<TSource> self, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			var result = default(TSource);
			var found = false;

			if (self is IList<TSource> list)
			{
				var count = list.Count;

				for (int i = 0; i < count; ++i)
				{
					var item = list[i];

					if (predicate(item))
					{
						if (found)
						{
							throw new InvalidOperationException("Sequence contains more than one matching element");
						}

						result = item;
						found = true;
					}
				}
			}
			else
			{
				foreach (TSource item in self)
				{
					if (predicate(item))
					{
						if (found)
						{
							throw new InvalidOperationException("Sequence contains more than one matching element");
						}

						result = item;
						found = true;
					}
				}
			}

			if (!found)
			{
				throw new InvalidOperationException("Sequence contains no matching element");
			}

			return result;
		}

		public static TSource FindUnique<TSource>(this ICollection<TSource> self, TSource defaultValue, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			var result = defaultValue;
			var found = false;

			if (self is IList<TSource> list)
			{
				var count = list.Count;

				for (int i = 0; i < count; ++i)
				{
					var item = list[i];

					if (predicate(item))
					{
						if (found)
						{
							throw new InvalidOperationException("Sequence contains more than one matching element");
						}

						result = item;
						found = true;
					}
				}
			}
			else
			{
				foreach (TSource item in self)
				{
					if (predicate(item))
					{
						if (found)
						{
							throw new InvalidOperationException("Sequence contains more than one matching element");
						}

						result = item;
						found = true;
					}
				}
			}

			return result;
		}

		public static TSource First<TSource>(this ICollection<TSource> self)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (self.Count == 0)
			{
				throw new InvalidOperationException("Sequence contains no elements");
			}

			if (self is IList<TSource> list)
			{
				return list[0];
			}

			using (var enumerator = self.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}

				throw new InvalidOperationException("Sequence contains no elements (unexpected state after count check).");
			}
		}

		public static IList<TResult> Flatten<TSource, TResult>(this ICollection<TSource> self, Func<TSource, ICollection<TResult>> selector)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (selector == null)
			{
				throw new ArgumentNullException(nameof(selector));
			}

			var result = new List<TResult>();

			foreach (var outerItem in self)
			{
				var innerCollection = selector(outerItem);

				if (innerCollection != null)
				{
					foreach (var innerItem in innerCollection)
					{
						result.Add(innerItem);
					}
				}
			}

			return result;
		}

		public static void ForEach<TSource>(this ICollection<TSource> self, Action<TSource> action)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			if (self is IList<TSource> list)
			{
				for (int i = 0, count = list.Count; i < count; ++i)
				{
					action(list[i]);
				}
			}
			else
			{
				foreach (var item in self)
				{
					action(item);
				}
			}
		}

		public static void ForEach<TSource>(this ICollection<TSource> self, Func<TSource, bool> action)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			if (self is IList<TSource> list)
			{
				for (int i = 0, count = list.Count; i < count; ++i)
				{
					if (!action(list[i]))
					{
						break;
					}
				}
			}
			else
			{
				foreach (var item in self)
				{
					if (!action(item))
					{
						break;
					}
				}
			}
		}

		public static void ForEachReverse<TSource>(this ICollection<TSource> self, Action<TSource> action)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			if (self is IList<TSource> list)
			{
				for (int i = list.Count - 1; i >= 0; --i)
				{
					action(list[i]);
				}
			}
			else
			{
				var tempList = System.Linq.Enumerable.ToList(self);

				for (int i = tempList.Count - 1; i >= 0; --i)
				{
					action(tempList[i]);
				}
			}
		}

		public static void ForEachReverse<TSource>(this ICollection<TSource> self, Func<TSource, bool> action)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			if (self is IList<TSource> list)
			{
				for (int i = list.Count - 1; i >= 0; --i)
				{
					if (!action(list[i]))
					{
						break;
					}
				}
			}
			else
			{
				var tempList = System.Linq.Enumerable.ToList(self);

				for (int i = tempList.Count - 1; i >= 0; --i)
				{
					if (!action(tempList[i]))
					{
						break;
					}
				}
			}
		}

		public static bool IsNullOrEmpty<TSource>(this ICollection<TSource> self)
		{
			return self == null || self.Count == 0;
		}

		public static TSource Last<TSource>(this ICollection<TSource> self)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (self.Count == 0)
			{
				throw new InvalidOperationException("Sequence contains no elements");
			}

			if (self is IList<TSource> list)
			{
				return list[list.Count - 1];
			}

			var lastElement = default(TSource);

			// For unordered collections (like HashSet), "last" is less defined but this will return one of the elements
			foreach (var item in self)
			{
				lastElement = item;
			}

			return lastElement;
		}

		public static IList<TResult> Map<TSource, TResult>(this ICollection<TSource> self, Func<TSource, TResult> selector)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (selector == null)
			{
				throw new ArgumentNullException(nameof(selector));
			}

			var result = new List<TResult>(self.Count);

			foreach (var item in self)
			{
				result.Add(selector(item));
			}

			return result;
		}

		public static IList<TSource> OrderBy<TSource, TKey>(this ICollection<TSource> self, Func<TSource, TKey> keySelector)
		{
			return System.Linq.Enumerable.OrderBy(self, keySelector).ToList();
		}

		public static TSource Reduce<TSource>(this ICollection<TSource> self, Func<TSource, TSource, TSource> function)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (function == null)
			{
				throw new ArgumentNullException(nameof(function));
			}

			if (self.Count == 0)
			{
				throw new InvalidOperationException("Source sequence doesn't contain any elements.");
			}

			using (var enumerator = self.GetEnumerator())
			{
				enumerator.MoveNext();
				var accumulator = enumerator.Current;

				while (enumerator.MoveNext())
				{
					accumulator = function(accumulator, enumerator.Current);
				}

				return accumulator;
			}
		}

		public static TResult Reduce<TSource, TResult>(this ICollection<TSource> self, TResult startValue, Func<TResult, TSource, TResult> function)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (function == null)
			{
				throw new ArgumentNullException(nameof(function));
			}

			var accumulator = startValue;

			foreach (var item in self)
			{
				accumulator = function(accumulator, item);
			}

			return accumulator;
		}

		public static void RemoveIf<TSource>(this ICollection<TSource> self, Func<TSource, bool> condition)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (condition == null)
			{
				throw new ArgumentNullException(nameof(condition));
			}

			if (self.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			if (self is IList<TSource> list)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					var currentItem = list[i];

					if (condition(currentItem))
					{
						list.RemoveAt(i);
					}
				}

				return;
			}

			List<TSource> itemsToRemove = null;

			foreach (var item in self)
			{
				if (condition(item))
				{
					if (itemsToRemove == null)
					{
						itemsToRemove = new List<TSource>();
					}

					itemsToRemove.Add(item);
				}
			}

			if (itemsToRemove != null && itemsToRemove.Count > 0)
			{
				foreach (var itemToRemove in itemsToRemove)
				{
					self.Remove(itemToRemove);
				}
			}
		}

		public static IList<TSource> Reverse<TSource>(this ICollection<TSource> self)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			var list = System.Linq.Enumerable.ToList(self);
			int i = 0;
			int j = (list.Count - 1);

			while (i < j)
			{
				var temp = list[i];

				list[i] = list[j];
				list[j] = temp;

				i++;
				j--;
			}

			return list;
		}

		public static bool Some<TSource>(this ICollection<TSource> self, Func<TSource, bool> predicate)
		{
			return self.Any(predicate);
		}

		public static TSource[] ToArray<TSource>(this ICollection<TSource> self)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			var array = new TSource[self.Count];
			self.CopyTo(array, 0);

			return array;
		}

		public static IEnumerable<TSource> ToEnumerable<TSource>(this ICollection<TSource> self)
		{
			return self;
		}

		public static List<TSource> ToList<TSource>(this ICollection<TSource> self)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (self is List<TSource> list)
			{
				return list;
			}

			return new List<TSource>(self);
		}
	}
}
