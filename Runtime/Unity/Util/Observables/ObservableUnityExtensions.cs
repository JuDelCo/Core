
#if UNITY_2019_3_OR_NEWER

using System;
using Ju.Handlers;
using Ju.Observables;
using UnityEngine;

public static class ObservableUnityExtensions
{
	public static void Subscribe<T>(this Observable<T> observable, Behaviour behaviour, Action<T> action, bool alwaysActive = false)
	{
		observable.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action);
	}
}

#endif
