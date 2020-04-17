
#if UNITY_2018_3_OR_NEWER

using System;
using UnityEngine;

namespace Ju
{
	public static class IMonoBehaviourExtensions
	{
		public static void EventSubscribe<T>(this MonoBehaviour self, Action<T> action)
		{
			Services.Get<IEventBusService>().Subscribe(self, action);
		}

		public static Vector3 GetPosition(this MonoBehaviour self)
		{
			return self.transform.position;
		}

		public static Vector3 GetLocalPosition(this MonoBehaviour self)
		{
			return self.transform.localPosition;
		}

		public static Quaternion GetRotation(this MonoBehaviour self)
		{
			return self.transform.rotation;
		}

		public static Quaternion GetLocalRotation(this MonoBehaviour self)
		{
			return self.transform.localRotation;
		}

		public static Vector3 GetScale(this MonoBehaviour self)
		{
			return self.transform.localScale;
		}
	}
}

#endif
