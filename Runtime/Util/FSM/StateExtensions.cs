using System;
using System.Collections;

namespace Ju
{
	public static class StateExtensions
	{
		public static void EventSubscribe<T>(this State state, Action<T> action)
		{
			Services.Get<IEventBusService>().Subscribe(state, action);
		}

		public static Coroutine CoroutineStart(this State state, IEnumerator routine)
		{
			return Services.Get<ICoroutineService>().StartCoroutine(state, routine);
		}

		public static IPromise WaitUntil(this State state, Func<bool> condition)
		{
			return Services.Get<ITaskService>().WaitUntil(state, condition);
		}

		public static IPromise WaitWhile(this State state, Func<bool> condition)
		{
			return Services.Get<ITaskService>().WaitWhile(state, condition);
		}

		public static IPromise WaitForSeconds(this State state, float delay)
		{
			return Services.Get<ITaskService>().WaitForSeconds(state, delay);
		}
	}
}
