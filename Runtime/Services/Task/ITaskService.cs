using System;
using Ju.Handlers;
using Ju.Promises;

namespace Ju.Services
{
	public interface ITaskService : IServiceLoad, ILoggableService
	{
		void RunOnMainThread(Action action, float delay = 0f);

		IPromise WaitUntil(ILinkHandler handle, Func<bool> condition);
		IPromise WaitWhile(ILinkHandler handle, Func<bool> condition);
		IPromise WaitForSeconds(ILinkHandler handle, float delay);
	}
}
