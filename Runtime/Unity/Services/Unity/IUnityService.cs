
#if UNITY_2018_3_OR_NEWER

namespace Ju
{
	public delegate bool UnityServiceQuitRequestedEvent();

	public interface IUnityService : IService, ILoggableService
	{
		event UnityServiceQuitRequestedEvent OnApplicationWantsToQuit;
	}
}

#endif
