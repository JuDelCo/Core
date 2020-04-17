
#if UNITY_2018_3_OR_NEWER

using System;

namespace Ju
{
	public static class MonoBehaviourActorExtensions
	{
		public static void Subscribe<T>(this Observable<T> self, MonoBehaviourActor actor, Action<T> action)
		{
			actor.AddObservableUnsubscriber(self.Subscribe(action));
		}
	}
}

#endif
