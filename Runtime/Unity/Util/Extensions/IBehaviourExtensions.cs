
#if UNITY_2018_3_OR_NEWER

using System;
using System.Collections;
using UnityEngine;

namespace Ju
{
	public static class IBehaviourExtensions
	{
		public static void EventSubscribe<T>(this Behaviour behaviour, Action<T> action, bool alwaysActive = false)
		{
			Services.Get<IEventBusService>().Subscribe(behaviour, action, alwaysActive);
		}

		public static Coroutine CoroutineStart(this Behaviour behaviour, IEnumerator routine, bool alwaysActive = true)
		{
			return Services.Get<ICoroutineService>().StartCoroutine(behaviour, routine, alwaysActive);
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
