
#if UNITY_2018_3_OR_NEWER

using UnityEngine;

namespace Ju
{
	internal delegate void UnityServiceEvent();
	internal delegate void UnityServiceFocusEvent(bool hasFocus);

	public class UnityServiceBehaviour : MonoBehaviour
	{
		internal event UnityServiceEvent OnUpdateEvent = delegate { };
		internal event UnityServiceEvent OnFixedUpdateEvent = delegate { };
		internal event UnityServiceFocusEvent OnApplicationFocusEvent = delegate { };
		internal event UnityServiceEvent OnApplicationQuitEvent = delegate { };

		private void Update()
		{
			OnUpdateEvent();
		}

		private void FixedUpdate()
		{
			OnFixedUpdateEvent();
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			OnApplicationFocusEvent(hasFocus);
		}

		private void OnApplicationQuit()
		{
			OnApplicationQuitEvent();
		}
	}
}

#endif
