
#if UNITY_2018_3_OR_NEWER

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Ju.Services.InternalUnityServiceBehaviour;

namespace Ju.Services
{
	public class UnityService : IUnityService
	{
		public event UnityServiceQuitRequestedEvent OnApplicationWantsToQuit;
		public event UnityServiceQuitEvent OnApplicationQuit;

		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private IEventBusService eventService;
		private UnityServiceBehaviour monoBehaviour;
		private bool behaviourInitialized = false;
		private bool quitting = false;

		public void Setup()
		{
			SceneManager.sceneLoaded += OnUnitySceneLoaded;
			Application.wantsToQuit += OnUnityApplicationWantsToQuit;

#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += state =>
			{
				if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode && !quitting)
				{
					UnityEditor.EditorApplication.isPlaying = !OnUnityApplicationWantsToQuit();
				}
			};
#endif

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
			eventService = ServiceContainer.Get<IEventBusService>();
		}

		private void OnUnityUpdate()
		{
			eventService.Fire(new LoopUpdateEvent(UnityEngine.Time.deltaTime));
		}

		private void OnUnityFixedUpdate()
		{
			eventService.Fire(new LoopFixedUpdateEvent(UnityEngine.Time.fixedDeltaTime));
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
			var result = true;

			if (!quitting)
			{
				if (OnApplicationWantsToQuit != null)
				{
					result = OnApplicationWantsToQuit();
				}

				if (result)
				{
					result = false;
					StartApplicationQuitRoutine();
				}
			}

			return result;
		}

		private void StartApplicationQuitRoutine()
		{
			quitting = true;

			if (OnApplicationQuit != null)
			{
				OnApplicationQuit();
			}

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

			GameObject.Destroy(monoBehaviour);
			Resources.UnloadUnusedAssets();

			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, false);
			GC.WaitForPendingFinalizers();

			ServiceContainer.Dispose();

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}

#endif
