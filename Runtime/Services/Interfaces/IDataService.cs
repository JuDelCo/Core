using System.Collections.Generic;

namespace Ju
{
	public interface IDataService : IService, ILoggableService
	{
		void SetShared<T>(T obj);
		T GetShared<T>() where T : class;
		void RemoveShared<T>();

		void Add<T>(T obj);
		List<T> GetList<T>();
		void Remove<T>(T obj);
	}
}
