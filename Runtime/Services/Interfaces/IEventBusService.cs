using System;
using Handle = System.Object;

namespace Ju
{
	public interface IEventBusService : IService, ILoggableService
	{
		void SetInvalidHandleTest(Func<Handle, bool> invalidHandleTest);
		void SetEnabledHandleTest(Func<Handle, bool> enabledHandleTest);
		void Subscribe<T>(Handle handle, Action<T> action);
		void Enable(Handle handle);
		void Disable(Handle handle);
		void UnSubscribe(Handle handle);
		void Fire<T>(T data);
	}
}
