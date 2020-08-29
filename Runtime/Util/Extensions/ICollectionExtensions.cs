using System;
using System.Collections.Generic;

namespace Ju.Extensions
{
	public static class ICollectionExtensions
	{
		public static void RemoveIf<TSource>(this ICollection<TSource> self, Func<TSource, bool> condition)
		{
			var reversed = self.Reverse();

			using (var enumerator = reversed.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (condition(enumerator.Current))
					{
						self.Remove(enumerator.Current);
					}
				}
			}
		}
	}
}
