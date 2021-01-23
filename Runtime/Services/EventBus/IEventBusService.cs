using System;
using Ju.Handlers;
using ChannelId = System.Byte;
using EventType = System.Type;

namespace Ju.Services
{
	public delegate void EventBusServiceFiredEvent(ChannelId channel, EventType type, object obj, int actionCount);

	public interface IEventBusService : IServiceLoad, ILoggableService
	{
		event EventBusServiceFiredEvent OnEventFired;

		void Subscribe<T>(ChannelId channel, ILinkHandler handle, Action<T> action);
		void Fire<T>(ChannelId channel, T data);
	}
}
