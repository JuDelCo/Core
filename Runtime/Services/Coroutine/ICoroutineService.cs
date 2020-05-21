using System.Collections;

namespace Ju
{
	public interface ICoroutineService : IService, ILoggableService
	{
		Coroutine StartCoroutine(ILinkHandler handle, IEnumerator routine);
	}
}
