// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ju.Extensions
{
	public static class IEnumerableExtensions
	{
		public static bool All<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
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

		public static bool Any<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
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

		public static IEnumerable<TResult> Cast<TResult>(this IEnumerable self)
		{
			return System.Linq.Enumerable.Cast<TResult>(self);
		}

		public static IEnumerable<TSource> Concatenate<TSource>(this IEnumerable<TSource> self, params IEnumerable<TSource>[] others)
		{
			if (self == null)
			{
				throw new ArgumentNullException(nameof(self));
			}

			if (others == null)
			{
				throw new ArgumentNullException(nameof(others));
			}

			using (var enumerator = self.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					yield return enumerator.Current;
				}
			}

			for (int i = 0, othersCount = others.Length; i < othersCount; ++i)
			{
				var otherEnumerable = others[i];

				if (otherEnumerable != null)
				{
					using (var enumerator = otherEnumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							yield return enumerator.Current;
						}
					}
				}
			}
		}

		public static bool Contains<TSource>(this IEnumerable<TSource> self, TSource value)
		{
			return System.Linq.Enumerable.Contains(self, value);
		}

		public static int Count<TSource>(this IEnumerable<TSource> self)
		{
			return System.Linq.Enumerable.Count(self);
		}

		public static TSource ElementAt<TSource>(this IEnumerable<TSource> self, int index)
		{
			return System.Linq.Enumerable.ElementAt(self, index);
		}

		public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
		{
			return System.Linq.Enumerable.Where(self, predicate);
		}

		public static TSource Find<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
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

		public static TSource Find<TSource>(this IEnumerable<TSource> self, TSource defaultValue, Func<TSource, bool> predicate)
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

		public static TSource FindUnique<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
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

		public static TSource FindUnique<TSource>(this IEnumerable<TSource> self, TSource defaultValue, Func<TSource, bool> predicate)
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
				using (var enumerator = self.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var current = enumerator.Current;

						if (predicate(current))
						{
							if (found)
							{
								throw new InvalidOperationException("Sequence contains more than one matching element");
							}

							result = current;
							found = true;
						}
					}
				}
			}

			return result;
		}

		public static TSource First<TSource>(this IEnumerable<TSource> self)
		{
			return System.Linq.Enumerable.First(self);
		}

		public static IEnumerable<TResult> Flatten<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, IEnumerable<TResult>> selector)
		{
			using (var enumerator = self.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var innerEnumerable = selector(enumerator.Current);

					if (innerEnumerable != null)
					{
						using (var internalEnumerator = innerEnumerable.GetEnumerator())
						{
							while (internalEnumerator.MoveNext())
							{
								yield return internalEnumerator.Current;
							}
						}
					}
				}
			}
		}

		public static void ForEach<TSource>(this IEnumerable<TSource> self, Action<TSource> action)
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
				var count = list.Count;

				for (int i = 0; i < count; ++i)
				{
					action(list[i]);
				}
			}
			else
			{
				using (var enumerator = self.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						action(enumerator.Current);
					}
				}
			}
		}

		public static void ForEach<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> action)
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
				var count = list.Count;

				for (int i = 0; i < count; ++i)
				{
					if (!action(list[i]))
					{
						break;
					}
				}
			}
			else
			{
				using (var enumerator = self.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!action(enumerator.Current))
						{
							break;
						}
					}
				}
			}
		}

		public static void ForEachReverse<TSource>(this IEnumerable<TSource> self, Action<TSource> action)
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
				var reversed = System.Linq.Enumerable.Reverse(self);

				using (var enumerator = reversed.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						action(enumerator.Current);
					}
				}
			}
		}

		public static void ForEachReverse<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> action)
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
				var reversed = System.Linq.Enumerable.Reverse(self);

				using (var enumerator = reversed.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!action(enumerator.Current))
						{
							break;
						}
					}
				}
			}
		}

		public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> self)
		{
			if (self == null)
			{
				return true;
			}

			if (self is ICollection<TSource> genericCollection)
			{
				return (genericCollection.Count == 0);
			}

			if (self is System.Collections.ICollection nonGenericCollection)
			{
				return (nonGenericCollection.Count == 0);
			}

			using (var enumerator = self.GetEnumerator())
			{
				return !enumerator.MoveNext();
			}
		}

		public static TSource Last<TSource>(this IEnumerable<TSource> self)
		{
			return System.Linq.Enumerable.Last(self);
		}

		public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
		{
			using (var enumerator = self.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					yield return selector(enumerator.Current);
				}
			}
		}

		public static IEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> self, Func<TSource, TKey> keySelector)
		{
			return System.Linq.Enumerable.OrderBy(self, keySelector);
		}

		public static TSource Reduce<TSource>(this IEnumerable<TSource> self, Func<TSource, TSource, TSource> function)
		{
			return System.Linq.Enumerable.Aggregate(self, function);
		}

		public static TResult Reduce<TSource, TResult>(this IEnumerable<TSource> self, TResult startValue, Func<TResult, TSource, TResult> function)
		{
			return System.Linq.Enumerable.Aggregate(self, startValue, function);
		}

		public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> self)
		{
			return System.Linq.Enumerable.Reverse(self);
		}

		public static bool Some<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
		{
			return self.Any(predicate);
		}

		public static TSource[] ToArray<TSource>(this IEnumerable<TSource> self)
		{
			return System.Linq.Enumerable.ToArray(self);
		}

		public static IEnumerable<TSource> ToEnumerable<TSource>(this IEnumerable<TSource> self)
		{
			return self;
		}

		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> self)
		{
			return System.Linq.Enumerable.ToList(self);
		}
	}
}
