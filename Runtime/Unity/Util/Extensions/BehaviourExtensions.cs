
#if UNITY_2018_3_OR_NEWER

using System;
using System.Collections;
using UnityEngine;

namespace Ju
{
	public static class BehaviourExtensions
	{
		public static void EventSubscribe<T>(this Behaviour behaviour, Action<T> action, bool alwaysActive = false)
		{
			Services.Get<IEventBusService>().Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action);
		}

		public static Coroutine CoroutineStart(this Behaviour behaviour, IEnumerator routine, bool alwaysActive = true)
		{
			return Services.Get<ICoroutineService>().StartCoroutine(new BehaviourLinkHandler(behaviour, alwaysActive), routine);
		}

		public static IPromise WaitUntil(this Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return Services.Get<ITaskService>().WaitUntil(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitWhile(this Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return Services.Get<ITaskService>().WaitWhile(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitForSeconds(this Behaviour behaviour, float delay, bool alwaysActive = false)
		{
			return Services.Get<ITaskService>().WaitForSeconds(new BehaviourLinkHandler(behaviour, alwaysActive), delay);
		}

		public static Clock NewClock(this Behaviour behaviour, TimeUpdateMode updateMode = TimeUpdateMode.Update, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Clock(() => linkHandler.IsActive, updateMode);
		}

		public static Clock NewClock(this Behaviour behaviour, float elapsedSeconds, TimeUpdateMode updateMode = TimeUpdateMode.Update, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Clock(elapsedSeconds, () => linkHandler.IsActive, updateMode);
		}

		public static Timer NewTimer(this Behaviour behaviour, float seconds, Action onCompleted, TimeUpdateMode updateMode = TimeUpdateMode.Update, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new Timer(seconds, onCompleted, () => linkHandler.IsActive, updateMode);
		}

		public static FrameTimer NewFrameTimer(this Behaviour behaviour, int frames, Action onCompleted, TimeUpdateMode updateMode = TimeUpdateMode.Update, bool alwaysActive = false)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			return new FrameTimer(frames, onCompleted, () => linkHandler.IsActive, updateMode);
		}

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

#endif
