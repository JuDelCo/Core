using System;
using Ju.Handlers;

namespace Ju.Services
{
	public interface IEventBusService : IServiceLoad, ILoggableService
	{
		void Subscribe<T>(ILinkHandler handle, Action<T> action);
		void Fire<T>() where T : struct;
		void Fire<T>(T data);
	}
}
