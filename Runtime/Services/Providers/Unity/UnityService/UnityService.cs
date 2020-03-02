
#if UNITY_2018_3_OR_NEWER

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ju
{
	public class UnityService : IUnityService
	{
		public event UnityServiceEvent OnUpdate = delegate { };
		public event UnityServiceEvent OnFixedUpdate = delegate { };
		public event UnityServiceEvent OnSceneLoaded = delegate { };
		public event UnityServiceFocusEvent OnApplicationFocus = delegate { };
		public event UnityServiceQuitRequestedEvent OnApplicationWantsToQuit;

		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private UnityServiceBehaviour monoBehaviour;
		private static bool behaviourInitialized = false;

		public void Setup()
		{
			SceneManager.sceneLoaded += OnUnitySceneLoaded;
			Application.wantsToQuit += OnUnityApplicationWantsToQuit;

			SceneManager.sceneLoaded += (scene, loadSceneMode) =>
			{
				if (behaviourInitialized)
				{
					return;
				}

				var gameObject = new GameObject("UnityService");
				gameObject.hideFlags = HideFlags.HideInHierarchy;
				UnityEngine.Object.DontDestroyOnLoad(gameObject);

				monoBehaviour = gameObject.AddComponent<UnityServiceBehaviour>();
				monoBehaviour.OnUpdateEvent += OnUnityUpdate;
				monoBehaviour.OnFixedUpdateEvent += OnUnityFixedUpdate;
				monoBehaviour.OnApplicationFocusEvent += OnUnityApplicationFocus;
				monoBehaviour.OnApplicationQuitEvent += OnUnityApplicationQuit;

				behaviourInitialized = true;
			};

			if (Application.genuineCheckAvailable)
			{
				if (!Application.genuine)
				{
					OnLogError("App integrity was compromised");
					Application.Quit();
				}
			}

			if (System.Diagnostics.Debugger.IsAttached)
			{
				OnLogWarning("NET Debugger detected");
			}

			Application.lowMemory += () =>
			{
				OnLogWarning("Low memory detected");
				Resources.UnloadUnusedAssets();
			};
		}

		public void Start()
		{
		}

		public Coroutine StartCoroutine(IEnumerator routine, float delay = 0f)
		{
			return monoBehaviour.StartCoroutine(StartCoroutineImpl(routine, delay));
		}

		private IEnumerator StartCoroutineImpl(IEnumerator routine, float delay)
		{
			if(delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}

			yield return monoBehaviour.StartCoroutine(routine);
		}

		public void StopCoroutine(Coroutine routine)
		{
			monoBehaviour.StopCoroutine(routine);
		}

		private void OnUnityUpdate()
		{
			OnUpdate();
		}

		private void OnUnityFixedUpdate()
		{
			OnFixedUpdate();
		}

		private void OnUnitySceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			OnSceneLoaded();
		}

		private void OnUnityApplicationFocus(bool hasFocus)
		{
			OnApplicationFocus(hasFocus);
		}

		private bool OnUnityApplicationWantsToQuit()
		{
			if (OnApplicationWantsToQuit != null)
			{
				return OnApplicationWantsToQuit();
			}

			return true;
		}

		private void OnUnityApplicationQuit()
		{
			ForceUnloadAllGameObjects();
			Services.Dispose();
		}

		private void ForceUnloadAllGameObjects()
		{
			foreach (var obj in Object.FindObjectsOfType<GameObject>())
			{
				GameObject.DestroyImmediate(obj);
			}
		}
	}
}

#endif
