
#if UNITY_2018_3_OR_NEWER

namespace Ju.Services
{
	public delegate bool UnityServiceQuitRequestedEvent();
	public delegate void UnityServiceQuitEvent();

	public interface IUnityService : IServiceLoad, ILoggableService
	{
		event UnityServiceQuitRequestedEvent OnApplicationWantsToQuit;
		event UnityServiceQuitEvent OnApplicationQuit;
	}
}

#endif
