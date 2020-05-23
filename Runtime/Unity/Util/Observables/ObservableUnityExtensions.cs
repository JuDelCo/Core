
#if UNITY_2018_3_OR_NEWER

using System;
using UnityEngine;

namespace Ju
{
	public static class ObservableUnityExtensions
	{
		public static void Subscribe<T>(this Observable<T> observable, Behaviour behaviour, Action<T> action, bool alwaysActive = false)
		{
			observable.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action);
		}
	}
}

#endif
