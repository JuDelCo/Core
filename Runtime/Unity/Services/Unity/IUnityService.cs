
#if UNITY_2018_3_OR_NEWER

namespace Ju
{
	public delegate void UnityServiceEvent();
	public delegate void UnityServiceFocusEvent(bool hasFocus);
	public delegate bool UnityServiceQuitRequestedEvent();

	public interface IUnityService : IService, ILoggableService
	{
		event UnityServiceEvent OnUpdate;
		event UnityServiceEvent OnFixedUpdate;
		event UnityServiceEvent OnSceneLoaded;
		event UnityServiceFocusEvent OnApplicationFocus;
		event UnityServiceQuitRequestedEvent OnApplicationWantsToQuit;
	}
}

#endif
