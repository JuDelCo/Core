using System;

namespace Ju
{
	public interface IEventBusService : IService, ILoggableService
	{
		void Subscribe<T>(ILinkHandler handle, Action<T> action);
		void Fire<T>(T data);
	}
}
