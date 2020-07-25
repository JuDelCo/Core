using System;
using System.Collections;
using Ju;
using Ju.FSM;
using Ju.Promises;
using Ju.Services;
using Ju.Time;

public static class StateUtilitiesExtensions
{
	public static void EventSubscribe<T>(this State state, Action<T> action)
	{
		ServiceContainer.Get<IEventBusService>().Subscribe(new StateLinkHandler(state), action);
	}

	public static Coroutine CoroutineStart(this State state, IEnumerator routine)
	{
		return ServiceContainer.Get<ICoroutineService>().StartCoroutine(new StateLinkHandler(state), routine);
	}

	public static IPromise WaitUntil(this State state, Func<bool> condition)
	{
		return ServiceContainer.Get<ITaskService>().WaitUntil(new StateLinkHandler(state), condition);
	}

	public static IPromise WaitWhile(this State state, Func<bool> condition)
	{
		return ServiceContainer.Get<ITaskService>().WaitWhile(new StateLinkHandler(state), condition);
	}

	public static IPromise WaitForSeconds(this State state, float delay)
	{
		return ServiceContainer.Get<ITaskService>().WaitForSeconds(new StateLinkHandler(state), delay);
	}

	public static Clock NewClock(this State state, TimeUpdateMode updateMode = TimeUpdateMode.Update)
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock(() => linkHandler.IsActive, updateMode);
	}

	public static Clock NewClock(this State state, float elapsedSeconds, TimeUpdateMode updateMode = TimeUpdateMode.Update)
	{
		var linkHandler = new StateLinkHandler(state);
		return new Clock(elapsedSeconds, () => linkHandler.IsActive, updateMode);
	}

	public static Timer NewTimer(this State state, float seconds, Action onCompleted, TimeUpdateMode updateMode = TimeUpdateMode.Update)
	{
		var linkHandler = new StateLinkHandler(state);
		return new Timer(seconds, onCompleted, () => linkHandler.IsActive, updateMode);
	}

	public static FrameTimer NewFrameTimer(this State state, int frames, Action onCompleted, TimeUpdateMode updateMode = TimeUpdateMode.Update)
	{
		var linkHandler = new StateLinkHandler(state);
		return new FrameTimer(frames, onCompleted, () => linkHandler.IsActive, updateMode);
	}
}
