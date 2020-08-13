using System;
using Ju.Handlers;

namespace Ju.Services
{
	public delegate void EventBusServiceFiredEvent(Type type, object obj, int actionCount);

	public interface IEventBusService : IServiceLoad, ILoggableService
	{
		event EventBusServiceFiredEvent OnEventFired;

		void Subscribe<T>(ILinkHandler handle, Action<T> action);
		void Fire<T>() where T : struct;
		void Fire<T>(T data);
	}
}
