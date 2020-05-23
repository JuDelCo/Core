using System;
using System.Collections.Generic;

namespace Ju
{
	public delegate void DataServiceListEvent(Type type, object obj);

	public interface IDataService : IService, ILoggableService
	{
		event DataServiceListEvent OnListAdd;
		event DataServiceListEvent OnListRemove;

		void Set<T>(T obj, string id);
		T Get<T>(string id) where T : class;
		void Unset<T>(string id);

		List<Type> GetTypes();

		void ListAdd<T>(T obj, string id);
		List<T> ListGet<T>(string id);
		void ListRemove<T>(T obj, string id);

		List<Type> ListGetTypes();
	}
}
