
#if UNITY_2018_3_OR_NEWER

namespace Ju
{
	public delegate bool UnityServiceQuitRequestedEvent();
	public delegate void UnityServiceQuitEvent();

	public interface IUnityService : IService, ILoggableService
	{
		event UnityServiceQuitRequestedEvent OnApplicationWantsToQuit;
		event UnityServiceQuitEvent OnApplicationQuit;
	}
}

#endif
