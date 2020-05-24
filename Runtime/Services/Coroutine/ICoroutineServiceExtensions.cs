using System.Collections;

namespace Ju
{
	public static class ICoroutineServiceExtensions
	{
		public static Coroutine StartCoroutine(this ICoroutineService coroutineService, State state, IEnumerator routine)
		{
			return coroutineService.StartCoroutine(new StateLinkHandler(state), routine);
		}
	}
}
