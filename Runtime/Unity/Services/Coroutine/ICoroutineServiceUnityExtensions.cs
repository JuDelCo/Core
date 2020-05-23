
#if UNITY_2018_3_OR_NEWER

using System.Collections;
using UnityEngine;

namespace Ju
{
	public static class ICoroutineServiceUnityExtensions
	{
		public static Coroutine StartCoroutine(this ICoroutineService coroutineService, Behaviour behaviour, IEnumerator routine, bool alwaysActive = true)
		{
			return coroutineService.StartCoroutine(new BehaviourLinkHandler(behaviour, alwaysActive), routine);
		}
	}
}

#endif
