
#if UNITY_2018_3_OR_NEWER

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Ju.InternalUnityServiceBehaviour;

namespace Ju
{
	public class UnityService : IUnityService
	{
		public event UnityServiceQuitRequestedEvent OnApplicationWantsToQuit;

		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private IEventBusService eventService;
		private UnityServiceBehaviour monoBehaviour;
		private bool behaviourInitialized = false;

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
			eventService = Services.Get<IEventBusService>();
		}

		private void OnUnityUpdate()
		{
			eventService.Fire(new LoopUpdateEvent(Time.deltaTime));
		}

		private void OnUnityFixedUpdate()
		{
			eventService.Fire(new LoopFixedUpdateEvent(Time.fixedDeltaTime));
		}

		private void OnUnitySceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			eventService.Fire(new UnitySceneLoadedEvent(scene.buildIndex, scene.name, loadSceneMode));
		}

		private void OnUnityApplicationFocus(bool hasFocus)
		{
			eventService.Fire(new UnityApplicationFocusEvent(hasFocus));
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
			monoBehaviour.StartCoroutine(DelayedDisposeServices());
		}

		private void ForceUnloadAllGameObjects()
		{
			var cachedGameObject = monoBehaviour.gameObject;

			foreach (var obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
			{
				if (obj != cachedGameObject)
				{
					GameObject.Destroy(obj);
				}
			}
		}

		private IEnumerator DelayedDisposeServices()
		{
			yield return null;
			Services.Dispose();
			GameObject.Destroy(monoBehaviour);
		}
	}
}

#endif
