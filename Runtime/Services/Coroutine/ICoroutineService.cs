using System.Collections;
using Ju.Handlers;

namespace Ju.Services
{
	public interface ICoroutineService : IServiceLoad, ILoggableService
	{
		Coroutine StartCoroutine(ILinkHandler handle, IEnumerator routine);
	}
}
