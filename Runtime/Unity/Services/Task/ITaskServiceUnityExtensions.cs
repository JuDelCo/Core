
#if UNITY_2018_3_OR_NEWER

using System;
using UnityEngine;

namespace Ju
{
	public static class ITaskServiceUnityExtensions
	{
		public static IPromise WaitUntil(this ITaskService taskService, Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return taskService.WaitUntil(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitWhile(this ITaskService taskService, Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return taskService.WaitWhile(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitForSeconds(this ITaskService taskService, Behaviour behaviour, float delay, bool alwaysActive = false)
		{
			return taskService.WaitForSeconds(new BehaviourLinkHandler(behaviour, alwaysActive), delay);
		}
	}
}

#endif
