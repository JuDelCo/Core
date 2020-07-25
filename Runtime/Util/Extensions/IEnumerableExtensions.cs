using System;
using System.Collections.Generic;
using System.Linq;

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

		public static IEnumerable<TResult> Cast<TSource, TResult>(this IEnumerable<TSource> self)
		{
			return self.Cast<TResult>();
		}

		public static IEnumerable<TSource> Concatenate<TSource>(this IEnumerable<TSource> self, params IEnumerable<TSource>[] others)
		{
			for (int i = 0, count = System.Linq.Enumerable.Count(self); i < count; ++i)
			{
				yield return self.ElementAt(i);
			}

			for (int i = 0, othersCount = others.Length; i < othersCount; ++i)
			{
				var ienumerable = others[i];

				for (int j = 0, count = System.Linq.Enumerable.Count(ienumerable); j < count; ++j)
				{
					yield return ienumerable.ElementAt(j);
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

		public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
		{
			return self.Where(predicate);
		}

		public static List<TSource> FilterList<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
		{
			return System.Linq.Enumerable.ToList(self.Where(predicate));
		}

		public static void ForEach<TSource>(this IEnumerable<TSource> self, Action<TSource> action)
		{
			for (int i = 0, count = System.Linq.Enumerable.Count(self); i < count; ++i)
			{
				action(self.ElementAt(i));
			}
		}

		public static void ForEachReverse<TSource>(this IEnumerable<TSource> self, Action<TSource> action)
		{
			for (int i = (System.Linq.Enumerable.Count(self) - 1); i >= 0; --i)
			{
				action(self.ElementAt(i));
			}
		}

		public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> self)
		{
			return self == null || !self.Any();
		}

		public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
		{
			return self.Select(selector);
		}

		public static List<TResult> MapList<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
		{
			return System.Linq.Enumerable.ToList(self.Select(selector));
		}

		public static TSource Reduce<TSource>(this IEnumerable<TSource> self, Func<TSource, TSource, TSource> function)
		{
			return self.Aggregate(function);
		}

		public static TResult Reduce<TSource, TResult>(this IEnumerable<TSource> self, TResult startValue, Func<TResult, TSource, TResult> function)
		{
			return self.Aggregate<TSource, TResult>(startValue, function);
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
