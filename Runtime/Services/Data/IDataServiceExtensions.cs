using System.Collections.Generic;
using Identifier = System.String;

namespace Ju
{
	public static class IDataServiceExtensions
	{
		private static readonly Identifier DEFAULT_ID = "base";

		public static void Set<T>(this IDataService dataService, T obj)
		{
			dataService.Set(obj, DEFAULT_ID);
		}

		public static T Get<T>(this IDataService dataService) where T : class
		{
			return dataService.Get<T>(DEFAULT_ID);
		}
		public static void Unset<T>(this IDataService dataService)
		{
			dataService.Unset<T>(DEFAULT_ID);
		}

		public static void ListAdd<T>(this IDataService dataService, T obj)
		{
			dataService.ListAdd(obj, DEFAULT_ID);
		}

		public static List<T> ListGet<T>(this IDataService dataService)
		{
			return dataService.ListGet<T>(DEFAULT_ID);
		}

		public static void ListRemove<T>(this IDataService dataService, T obj)
		{
			dataService.ListRemove(obj, DEFAULT_ID);
		}
	}
}