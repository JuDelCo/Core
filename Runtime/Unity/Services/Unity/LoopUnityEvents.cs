
#if UNITY_2018_3_OR_NEWER

using UnityEngine.SceneManagement;

namespace Ju.Services
{
	public struct UnitySceneLoadedEvent
	{
		public int sceneIndex;
		public string sceneName;
		public LoadSceneMode loadMode;

		public UnitySceneLoadedEvent(int sceneIndex, string sceneName, LoadSceneMode loadMode)
		{
			this.sceneIndex = sceneIndex;
			this.sceneName = sceneName;
			this.loadMode = loadMode;
		}
	}

	public struct UnityApplicationFocusEvent
	{
		public bool hasFocus;

		public UnityApplicationFocusEvent(bool hasFocus)
		{
			this.hasFocus = hasFocus;
		}
	}
}

#endif
