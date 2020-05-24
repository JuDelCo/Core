using System;

namespace Ju
{
	public static class ITaskServiceExtensions
	{
		public static IPromise WaitUntil(this ITaskService taskService, State state, Func<bool> condition)
		{
			return taskService.WaitUntil(new StateLinkHandler(state), condition);
		}

		public static IPromise WaitWhile(this ITaskService taskService, State state, Func<bool> condition)
		{
			return taskService.WaitWhile(new StateLinkHandler(state), condition);
		}

		public static IPromise WaitForSeconds(this ITaskService taskService, State state, float delay)
		{
			return taskService.WaitForSeconds(new StateLinkHandler(state), delay);
		}
	}
}
