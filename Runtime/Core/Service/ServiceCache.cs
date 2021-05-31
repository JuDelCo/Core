
namespace Ju.Services.Internal
{
	internal static class ServiceCache
	{
		private static IEventBusService eventBusService = null;
		private static ITaskService taskService = null;
		private static ICoroutineService coroutineService = null;
		private static ITimeService timeService = null;

		internal static IEventBusService EventBus
		{
			get
			{
				if (eventBusService is null)
				{
					eventBusService = ServiceContainer.Get<IEventBusService>();
				}

				return eventBusService;
			}
		}

		internal static ITaskService Task
		{
			get
			{
				if (taskService is null)
				{
					taskService = ServiceContainer.Get<ITaskService>();
				}

				return taskService;
			}
		}

		internal static ICoroutineService Coroutine
		{
			get
			{
				if (coroutineService is null)
				{
					coroutineService = ServiceContainer.Get<ICoroutineService>();
				}

				return coroutineService;
			}
		}

		internal static ITimeService Time
		{
			get
			{
				if (timeService is null)
				{
					timeService = ServiceContainer.Get<ITimeService>();
				}

				return timeService;
			}
		}

		internal static void Dispose()
		{
			eventBusService = null;
			taskService = null;
			coroutineService = null;
			timeService = null;
		}
	}
}
