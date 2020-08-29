using System;
using System.Collections;
using System.Collections.Generic;

namespace Ju.Extensions
{
	public static class IEnumerableExtensions
	{
		public static bool All<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
		{
			return System.Linq.Enumerable.All(self, predicate);
		}

		public static bool Any<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
		{
			return System.Linq.Enumerable.Any(self, predicate);
		}

		public static IEnumerable<TResult> Cast<TResult>(this IEnumerable self)
		{
			return System.Linq.Enumerable.Cast<TResult>(self);
		}

		public static IEnumerable<TSource> Concatenate<TSource>(this IEnumerable<TSource> self, params IEnumerable<TSource>[] others)
		{
			using (var enumerator = self.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					yield return enumerator.Current;
				}
			}

			for (int i = 0, othersCount = others.Length; i < othersCount; ++i)
			{
				var ienumerable = others[i];

				using (var enumerator = ienumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						yield return enumerator.Current;
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
			return System.Linq.Enumerable.First(self, predicate);
		}

		public static TSource Find<TSource>(this IEnumerable<TSource> self, TSource defaultValue, Func<TSource, bool> predicate)
		{
			var result = defaultValue;

			using (var enumerator = self.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (predicate(enumerator.Current))
					{
						result = enumerator.Current;
						break;
					}
				}
			}

			return result;
		}

		public static TSource FindUnique<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
		{
			return System.Linq.Enumerable.Single(self, predicate);
		}

		public static TSource FindUnique<TSource>(this IEnumerable<TSource> self, TSource defaultValue, Func<TSource, bool> predicate)
		{
			var result = defaultValue;
			var found = false;

			using (var enumerator = self.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (predicate(enumerator.Current))
					{
						if (found)
						{
							throw new InvalidOperationException("Found two elements that match the predicate");
						}

						result = enumerator.Current;
						found = true;
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
					using (var internalEnumerator = selector(enumerator.Current).GetEnumerator())
					{
						while (internalEnumerator.MoveNext())
						{
							yield return internalEnumerator.Current;
						}
					}
				}
			}
		}

		public static void ForEach<TSource>(this IEnumerable<TSource> self, Action<TSource> action)
		{
			using (var enumerator = self.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					action(enumerator.Current);
				}
			}
		}

		public static void ForEach<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> action)
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

		public static void ForEachReverse<TSource>(this IEnumerable<TSource> self, Action<TSource> action)
		{
			var reversed = self.Reverse();

			using (var enumerator = reversed.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					action(enumerator.Current);
				}
			}
		}

		public static void ForEachReverse<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> action)
		{
			var reversed = self.Reverse();

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

		public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> self)
		{
			return self == null || !System.Linq.Enumerable.Any(self);
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
			return System.Linq.Enumerable.AsEnumerable(self);
		}

		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> self)
		{
			return System.Linq.Enumerable.ToList(self);
		}
	}
}
