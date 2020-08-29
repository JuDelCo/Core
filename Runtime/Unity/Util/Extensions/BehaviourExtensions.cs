
#if UNITY_2019_3_OR_NEWER

using System;
using System.Collections;
using Ju.Handlers;
using Ju.Promises;
using Ju.Services;
using Ju.Time;
using UnityEngine;

namespace Ju.Extensions
{
	public static class BehaviourExtensions
	{
		public static Vector3 GetPosition(this Behaviour behaviour)
		{
			return behaviour.transform.position;
		}

		public static Vector3 GetLocalPosition(this Behaviour behaviour)
		{
			return behaviour.transform.localPosition;
		}

		public static Quaternion GetRotation(this Behaviour behaviour)
		{
			return behaviour.transform.rotation;
		}

		public static Quaternion GetLocalRotation(this Behaviour behaviour)
		{
			return behaviour.transform.localRotation;
		}

		public static Vector3 GetScale(this Behaviour behaviour)
		{
			return behaviour.transform.localScale;
		}
	}
}

namespace Ju.Extensions
{
	public static class BehaviourUtilitiesExtensions
	{
		public static void EventSubscribe<T>(this Behaviour behaviour, Action<T> action, bool alwaysActive = false)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action action, bool alwaysActive = false)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), (T _) => action());
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action<T> action, Func<T, bool> filter, bool alwaysActive = false)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action, filter);
		}

		public static void EventSubscribe<T>(this Behaviour behaviour, Action action, Func<T, bool> filter, bool alwaysActive = false)
		{
			ServiceContainer.Get<IEventBusService>().Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action, filter);
		}

		public static Services.Coroutine CoroutineStart(this Behaviour behaviour, IEnumerator routine, bool alwaysActive = true)
		{
			return ServiceContainer.Get<ICoroutineService>().StartCoroutine(new BehaviourLinkHandler(behaviour, alwaysActive), routine);
		}

		public static IPromise WaitUntil(this Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return ServiceContainer.Get<ITaskService>().WaitUntil(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitWhile(this Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return ServiceContainer.Get<ITaskService>().WaitWhile(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitForSeconds(this Behaviour behaviour, float delay, bool alwaysActive = false)
		{
			return ServiceContainer.Get<ITaskService>().WaitForSeconds(new BehaviourLinkHandler(behaviour, alwaysActive), delay);
		}

		public static Clock<T> NewClock<T>(this Behaviour behaviour, bool alwaysActive = false) where T : ILoopTimeEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Clock<T>(() => linkHandler.IsActive);
		}

		public static Clock<T> NewClock<T>(this Behaviour behaviour, float elapsedSeconds, bool alwaysActive = false) where T : ILoopTimeEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Clock<T>(elapsedSeconds, () => linkHandler.IsActive);
		}

		public static Timer<T> NewTimer<T>(this Behaviour behaviour, float seconds, Action onCompleted, bool alwaysActive = false) where T : ILoopTimeEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Timer<T>(seconds, onCompleted, () => linkHandler.IsActive);
		}

		public static FrameTimer<T> NewFrameTimer<T>(this Behaviour behaviour, int frames, Action onCompleted, bool alwaysActive = false) where T : ILoopEvent
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new FrameTimer<T>(frames, onCompleted, () => linkHandler.IsActive);
		}
	}
}

#endif
