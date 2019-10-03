using System.Collections.Generic;

namespace Ju
{
	public interface IDataService : IService, ILoggableService
	{
		void Set<T>(T obj);
		void Set<T>(T obj, string id);
		T Get<T>() where T : class;
		T Get<T>(string id) where T : class;
		void Unset<T>();
		void Unset<T>(string id);

		void ListAdd<T>(T obj);
		void ListAdd<T>(T obj, string id);
		List<T> ListGet<T>();
		List<T> ListGet<T>(string id);
		void ListRemove<T>(T obj);
		void ListRemove<T>(T obj, string id);
	}
}
