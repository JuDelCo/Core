
#if UNITY_2018_3_OR_NEWER

using System;
using UnityEngine;

namespace Ju
{
	public static class IEventBusServiceUnityExtensions
	{
		public static void Subscribe<T>(this IEventBusService eventService, Behaviour behaviour, Action<T> action, bool alwaysActive = false)
		{
			eventService.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action);
		}
	}
}

#endif
