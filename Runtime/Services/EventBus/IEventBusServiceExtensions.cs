using System;

namespace Ju
{
	public static class IEventBusServiceExtensions
	{
		public static void Subscribe<T>(this IEventBusService eventService, IService service, Action<T> action)
		{
			eventService.Subscribe(new KeepLinkHandler(), action);
		}

		public static void Subscribe<T>(this IEventBusService eventService, State state, Action<T> action)
		{
			eventService.Subscribe(new StateLinkHandler(state), action);
		}
	}
}
