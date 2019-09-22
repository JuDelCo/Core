using System;
using System.Collections.Generic;
using System.Linq;

namespace Ju
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
		{
			return self.Select(selector);
		}

		public static List<TResult> MapList<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
		{
			return self.Select(selector).ToList();
		}

		public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
		{
			return self.Where(predicate);
		}

		public static List<TSource> FilterList<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
		{
			return self.Where(predicate).ToList();
		}

		public static TSource Reduce<TSource>(this IEnumerable<TSource> self, Func<TSource, TSource, TSource> func)
		{
			return self.Aggregate(func);
		}

		public static TResult Reduce<TSource, TResult>(this IEnumerable<TSource> self, TResult seed, Func<TResult, TSource, TResult> func)
		{
			return self.Aggregate<TSource, TResult>(seed, func);
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
		{
			return self == null || !self.Any();
		}
	}
}
