// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ju.Extensions
{
	public static class IListExtensions
	{
		public static bool All<TSource>(this IList<TSource> self, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				if (!predicate(self[i]))
				{
					return false;
				}
			}

			return true;
		}

		public static bool Any<TSource>(this IList<TSource> self, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				if (predicate(self[i]))
				{
					return true;
				}
			}

			return false;
		}

		public static IList<TResult> Cast<TResult>(this IList self)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			var result = new List<TResult>(self.Count);

			for (int i = 0; i < self.Count; ++i)
			{
				result.Add((TResult) self[i]);
			}

			return result;
		}

		public static IList<TSource> Concatenate<TSource>(this IList<TSource> self, params IList<TSource>[] others)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (others == null)
			{
				throw new ArgumentNullException(nameof(others));
			}

			int estimatedCapacity = self.Count;

			for (int i = 0; i < others.Length; ++i)
			{
				if (others[i] != null)
				{
					estimatedCapacity += others[i].Count;
				}
			}

			var result = new List<TSource>(estimatedCapacity);

			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				result.Add(self[i]);
			}

			var othersCount = others.Length;

			for (int i = 0; i < othersCount; ++i)
			{
				var otherList = others[i];

				if (otherList != null)
				{
					var innerCount = otherList.Count;

					for (int j = 0; j < innerCount; ++j)
					{
						result.Add(otherList[j]);
					}
				}
			}

			return result;
		}

		public static bool Contains<TSource>(this IList<TSource> self, TSource value)
		{
			return self.Contains(value);
		}

		public static int Count<TSource>(this IList<TSource> self)
		{
			return self.Count;
		}

		public static TSource ElementAt<TSource>(this IList<TSource> self, int index)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (index < 0 || index >= self.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			return self[index];
		}

		public static IList<TSource> Filter<TSource>(this IList<TSource> self, Func<TSource, bool> predicate)
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
			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				if (predicate(self[i]))
				{
					result.Add(self[i]);
				}
			}

			return result;
		}

		public static TSource Find<TSource>(this IList<TSource> self, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				var item = self[i];

				if (predicate(item))
				{
					return item;
				}
			}

			throw new InvalidOperationException("Sequence contains no matching element");
		}

		public static TSource Find<TSource>(this IList<TSource> self, TSource defaultValue, Func<TSource, bool> predicate)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				var item = self[i];

				if (predicate(item))
				{
					return item;
				}
			}

			return defaultValue;
		}

		public static TSource FindUnique<TSource>(this IList<TSource> self, Func<TSource, bool> predicate)
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
			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				var item = self[i];

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

			if (!found)
			{
				throw new InvalidOperationException("Sequence contains no matching element");
			}

			return result;
		}

		public static TSource FindUnique<TSource>(this IList<TSource> self, TSource defaultValue, Func<TSource, bool> predicate)
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
			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				var item = self[i];

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

			return result;
		}

		public static TSource First<TSource>(this IList<TSource> self)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (self.Count == 0)
			{
				throw new InvalidOperationException("Sequence contains no elements");
			}

			return self[0];
		}

		public static IList<TResult> Flatten<TSource, TResult>(this IList<TSource> self, Func<TSource, IList<TResult>> selector)
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
			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				var innerList = selector(self[i]);

				if (innerList != null)
				{
					var innerCount = innerList.Count;

					for (int j = 0; j < innerCount; ++j)
					{
						result.Add(innerList[j]);
					}
				}
			}

			return result;
		}

		public static void ForEach<TSource>(this IList<TSource> self, Action<TSource> action)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				action(self[i]);
			}
		}

		public static void ForEach<TSource>(this IList<TSource> self, Func<TSource, bool> action)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				if (!action(self[i]))
				{
					break;
				}
			}
		}

		public static void ForEachReverse<TSource>(this IList<TSource> self, Action<TSource> action)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			for (int i = self.Count - 1; i >= 0; --i)
			{
				action(self[i]);
			}
		}

		public static void ForEachReverse<TSource>(this IList<TSource> self, Func<TSource, bool> action)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			for (int i = self.Count - 1; i >= 0; --i)
			{
				if (!action(self[i])) break;
			}
		}

		public static bool IsNullOrEmpty<TSource>(this IList<TSource> self)
		{
			return self == null || self.Count == 0;
		}

		public static TSource Last<TSource>(this IList<TSource> self)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (self.Count == 0)
			{
				throw new InvalidOperationException("Sequence contains no elements");
			}

			return self[self.Count - 1];
		}

		public static IList<TResult> Map<TSource, TResult>(this IList<TSource> self, Func<TSource, TResult> selector)
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
			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				result.Add(selector(self[i]));
			}

			return result;
		}

		public static IList<TSource> OrderBy<TSource, TKey>(this IList<TSource> self, Func<TSource, TKey> keySelector)
		{
			return System.Linq.Enumerable.OrderBy(self, keySelector).ToList();
		}

		public static TSource Reduce<TSource>(this IList<TSource> self, Func<TSource, TSource, TSource> function)
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

			var accumulator = self[0];
			var count = self.Count;

			for (int i = 1; i < count; ++i)
			{
				accumulator = function(accumulator, self[i]);
			}

			return accumulator;
		}

		public static TResult Reduce<TSource, TResult>(this IList<TSource> self, TResult startValue, Func<TResult, TSource, TResult> function)
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
			var count = self.Count;

			for (int i = 0; i < count; ++i)
			{
				accumulator = function(accumulator, self[i]);
			}

			return accumulator;
		}

		public static void RemoveIf<TSource>(this IList<TSource> self, Func<TSource, bool> condition)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (condition == null)
			{
				throw new ArgumentNullException(nameof(condition));
			}

			for (int i = self.Count - 1; i >= 0; --i)
			{
				if (condition(self[i]))
				{
					self.RemoveAt(i);
				}
			}
		}

		public static IList<TSource> Reverse<TSource>(this IList<TSource> self)
		{
			return System.Linq.Enumerable.Reverse(self).ToList();
		}

		public static bool Some<TSource>(this IList<TSource> self, Func<TSource, bool> predicate)
		{
			return self.Any(predicate);
		}

		public static TSource[] ToArray<TSource>(this IList<TSource> self)
		{
			var array = new TSource[self.Count];

			for (int i = 0; i < self.Count; ++i)
			{
				array[i] = self[i];
			}

			return array;
		}

		public static IEnumerable<TSource> ToEnumerable<TSource>(this IList<TSource> self)
		{
			return self;
		}
	}
}
