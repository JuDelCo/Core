
#if UNITY_2018_3_OR_NEWER

using System.Collections;
using UnityEngine;

namespace Ju
{
	public static class ICoroutineServiceUnityExtensions
	{
		public static Coroutine StartCoroutine(this ICoroutineService service, Behaviour behaviour, IEnumerator routine, bool alwaysActive = true)
		{
			return service.StartCoroutine(new BehaviourLinkHandler(behaviour, alwaysActive), routine);
		}
	}
}

#endif
