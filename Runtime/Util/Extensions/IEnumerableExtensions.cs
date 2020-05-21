using System;
using System.Collections.Generic;
using System.Linq;

namespace Ju
{
	public static class IEnumerableExtensions
	{
		public static T[] ToArray<T>(this IEnumerable<T> self)
		{
			return self.ToArray();
		}

		public static List<T> ToList<T>(this IEnumerable<T> self)
		{
			return self.ToList();
		}

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

		public static TSource Reduce<TSource>(this IEnumerable<TSource> self, Func<TSource, TSource, TSource> function)
		{
			return self.Aggregate(function);
		}

		public static TResult Reduce<TSource, TResult>(this IEnumerable<TSource> self, TResult startValue, Func<TResult, TSource, TResult> function)
		{
			return self.Aggregate<TSource, TResult>(startValue, function);
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
		{
			return self == null || !self.Any();
		}

		public static void RemoveIf<TSource>(this ICollection<TSource> self, Func<TSource, bool> condition)
		{
			for (int i = (self.Count() - 1); i >= 0; --i)
			{
				if (condition(self.ElementAt(i)))
				{
					self.Remove(self.ElementAt(i));
				}
			}
		}
	}
}
