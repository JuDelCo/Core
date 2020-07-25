using System;
using System.Collections.Generic;
using System.Linq;

namespace Ju.Extensions
{
	public static class ICollectionExtensions
	{
		public static void RemoveIf<TSource>(this ICollection<TSource> self, Func<TSource, bool> condition)
		{
			for (int i = (System.Linq.Enumerable.Count(self) - 1); i >= 0; --i)
			{
				if (condition(self.ElementAt(i)))
				{
					self.Remove(self.ElementAt(i));
				}
			}
		}
	}
}
