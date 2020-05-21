
#if UNITY_2018_3_OR_NEWER

using System;
using UnityEngine;

namespace Ju
{
	public struct BehaviourLinkHandler : ILinkHandler
	{
		private Behaviour behaviour;
		private bool alwaysActive;

		public BehaviourLinkHandler(Behaviour behaviour, bool alwaysActive = false)
		{
			this.behaviour = behaviour;
			this.alwaysActive = alwaysActive;
		}

		public bool IsActive => !IsDestroyed && (alwaysActive || behaviour.isActiveAndEnabled);
		public bool IsDestroyed => behaviour == null;
	}

	public static class UnityLinkHandlerExtensions
	{
		public static void Subscribe<T>(this IEventBusService service, Behaviour behaviour, Action<T> action, bool alwaysActive = false)
		{
			service.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action);
		}
	}
}

#endif
