using System;

namespace Ju
{
	public interface ITaskService : IService, ILoggableService
	{
		void RunOnMainThread(Action action, float delay = 0f);

		IPromise WaitUntil(ILinkHandler handle, Func<bool> condition);
		IPromise WaitWhile(ILinkHandler handle, Func<bool> condition);
		IPromise WaitForSeconds(ILinkHandler handle, float delay);
	}
}
