using System;
using Handle = System.Object;

namespace Ju
{
public interface IEventBusService : IService, ILoggableService
{
	void Suscribe<T>(Handle handle, Action<T> action);
	void Enable(Handle handle);
	void Disable(Handle handle);
	void UnSuscribe(Handle handle);
	void Fire<T>(T data);
}
}
