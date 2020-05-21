
#if UNITY_2018_3_OR_NEWER

using UnityEngine;

namespace Ju
{
	public class UnityServiceBehaviour : MonoBehaviour
	{
		public event UnityServiceEvent OnUpdateEvent = delegate { };
		public event UnityServiceEvent OnFixedUpdateEvent = delegate { };
		public event UnityServiceFocusEvent OnApplicationFocusEvent = delegate { };
		public event UnityServiceEvent OnApplicationQuitEvent = delegate { };

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
