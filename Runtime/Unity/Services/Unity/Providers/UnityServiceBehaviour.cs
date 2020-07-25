
#if UNITY_2018_3_OR_NEWER

using UnityEngine;

namespace Ju.Services
{
	internal delegate void UnityServiceEvent();
	internal delegate void UnityServiceFocusEvent(bool hasFocus);

	internal class InternalUnityServiceBehaviour
	{
		public class UnityServiceBehaviour : MonoBehaviour
		{
			internal event UnityServiceEvent OnUpdateEvent = delegate { };
			internal event UnityServiceEvent OnFixedUpdateEvent = delegate { };
			internal event UnityServiceFocusEvent OnApplicationFocusEvent = delegate { };

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
		}
	}
}

#endif
