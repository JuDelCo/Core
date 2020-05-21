using System.Collections;

namespace Ju
{
	public interface ICoroutineService : IService, ILoggableService
	{
		void Tick();
		Coroutine StartCoroutine(ILinkHandler handle, IEnumerator routine);
	}
}
