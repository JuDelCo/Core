using System;

namespace Ju
{
	public delegate void TaskUpdateEvent(float deltaTime);

	public interface ITaskService : IService, ILoggableService
	{
		event TaskUpdateEvent OnTick;

		void Tick(float deltaTime);

		void RunOnMainThread(Action action, float delay = 0f);

		IPromise WaitUntil(ILinkHandler handle, Func<bool> condition);
		IPromise WaitWhile(ILinkHandler handle, Func<bool> condition);
		IPromise WaitForSeconds(ILinkHandler handle, float delay);
	}
}
