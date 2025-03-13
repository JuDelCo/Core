// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using System.Collections;
using Ju.Services.Extensions;
using Ju.Time;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace Ju.Services
{
	using Ju.Log;
	using Ju.Services.Internal;

	public class UnityService : IUnityService, IServiceLoad
	{
		private bool appHasFocus = true;
		private bool wantsToQuit = false;
		private bool quitting = false;

		private struct UnityServiceLoopUpdateHook { };
		private struct UnityServiceLoopPostUpdateHook { };
		private struct UnityServiceLoopFixedUpdateHook { };
		private struct UnityServiceLoopPreCollisionsUpdateHook { };
		private struct UnityServiceLoopPostCollisionsUpdateHook { };
		private struct UnityServiceLoopPostFixedUpdateHook { };

		public void Load()
		{
			if (Application.genuineCheckAvailable)
			{
				if (!Application.genuine)
				{
					Log.Error("App integrity was compromised");
					Application.Quit();
				}
			}

			if (System.Diagnostics.Debugger.IsAttached)
			{
				Log.Warning("NET Debugger detected");
			}

			SubscribeApplicationStateEvents();
			SubscribeLoopEvents();
		}

		private void SubscribeApplicationStateEvents()
		{
			Application.lowMemory += HandleLowMemoryWarning;
			Application.wantsToQuit += OnUnityApplicationWantsToQuit;
			SceneManager.sceneLoaded += OnUnitySceneLoaded;

#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += HandleEditorPlayModeStateChanged;
#endif
		}

		private void UnsubscribeApplicationStateEvents()
		{
			Application.lowMemory -= HandleLowMemoryWarning;
			Application.wantsToQuit -= OnUnityApplicationWantsToQuit;
			SceneManager.sceneLoaded -= OnUnitySceneLoaded;

#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged -= HandleEditorPlayModeStateChanged;
#endif
		}

		private void SubscribeLoopEvents()
		{
			var loop = PlayerLoop.GetCurrentPlayerLoop();

			for (int mainSystemIndex = 0, mainSystemsCount = loop.subSystemList.Length; mainSystemIndex < mainSystemsCount; ++mainSystemIndex)
			{
				if (loop.subSystemList[mainSystemIndex].type == typeof(Update))
				{
					var subSystem = loop.subSystemList[mainSystemIndex];

					for (int index = 0, count = subSystem.subSystemList.Length; index < count; ++index)
					{
						if (subSystem.subSystemList[index].type == typeof(Update.ScriptRunBehaviourUpdate))
						{
							loop.subSystemList[mainSystemIndex] = InsertLoopSubSystem(OnUnityUpdate, typeof(UnityServiceLoopUpdateHook), subSystem, index);
							break;
						}
					}
				}
				else if (loop.subSystemList[mainSystemIndex].type == typeof(PreLateUpdate))
				{
					var subSystem = loop.subSystemList[mainSystemIndex];

					for (int index = 0, count = subSystem.subSystemList.Length; index < count; ++index)
					{
						if (subSystem.subSystemList[index].type == typeof(PreLateUpdate.ScriptRunBehaviourLateUpdate))
						{
							loop.subSystemList[mainSystemIndex] = InsertLoopSubSystem(OnUnityPostLateUpdate, typeof(UnityServiceLoopPostUpdateHook), subSystem, index);
							break;
						}
					}
				}
				else if (loop.subSystemList[mainSystemIndex].type == typeof(FixedUpdate))
				{
					var subSystem = loop.subSystemList[mainSystemIndex];

					for (int index = 0, count = subSystem.subSystemList.Length; index < count; ++index)
					{
						if (subSystem.subSystemList[index].type == typeof(FixedUpdate.ScriptRunBehaviourFixedUpdate))
						{
							var modifiedSubSystem = InsertLoopSubSystem(OnUnityFixedUpdate, typeof(UnityServiceLoopFixedUpdateHook), subSystem, index);
							loop.subSystemList[mainSystemIndex] = InsertLoopSubSystem(OnUnityPreCollisionsUpdate, typeof(UnityServiceLoopPreCollisionsUpdateHook), modifiedSubSystem, index + 2);
							break;
						}
					}

					subSystem = loop.subSystemList[mainSystemIndex];

					for (int index = 0, count = subSystem.subSystemList.Length; index < count; ++index)
					{
						if (subSystem.subSystemList[index].type == typeof(FixedUpdate.ScriptRunDelayedFixedFrameRate))
						{
							var modifiedSubSystem = InsertLoopSubSystem(OnUnityPostCollisionsUpdate, typeof(UnityServiceLoopPostCollisionsUpdateHook), subSystem, index + 1);
							loop.subSystemList[mainSystemIndex] = InsertLoopSubSystem(OnUnityPostFixedUpdate, typeof(UnityServiceLoopPostFixedUpdateHook), modifiedSubSystem, index + 2);
							break;
						}
					}
				}
			}

			PlayerLoop.SetPlayerLoop(loop);
		}

		private PlayerLoopSystem InsertLoopSubSystem(PlayerLoopSystem.UpdateFunction managedCallback, Type subSystemType, PlayerLoopSystem target, int index)
		{
			var loopUpdateSubSystem = new PlayerLoopSystem()
			{
				type = subSystemType,
				updateDelegate = managedCallback
			};

			var newSubSystems = new PlayerLoopSystem[target.subSystemList.Length + 1];
			var nativeLoopCondition = IntPtr.Zero;

			for (int i = 0, newSubSystemCount = newSubSystems.Length; i < newSubSystemCount; ++i)
			{
				if (i < index)
				{
					newSubSystems[i] = target.subSystemList[i];

					if ((i + 1) == index)
					{
						nativeLoopCondition = target.subSystemList[i].loopConditionFunction;
					}
				}
				else if (i == index)
				{
					loopUpdateSubSystem.loopConditionFunction = nativeLoopCondition;
					newSubSystems[i] = loopUpdateSubSystem;
				}
				else
				{
					newSubSystems[i] = target.subSystemList[i - 1];
				}
			}

			target.subSystemList = newSubSystems;

			return target;
		}

		private void OnUnityUpdate()
		{
			ServiceCache.EventBus.Fire(new TimePreUpdateEvent(UnityEngine.Time.deltaTime));
			ServiceCache.EventBus.Fire(new TimeUpdateEvent(UnityEngine.Time.deltaTime));
		}

		private void OnUnityPostLateUpdate()
		{
			ServiceCache.EventBus.Fire(new TimePostUpdateEvent(UnityEngine.Time.deltaTime));
		}

		private void OnUnityFixedUpdate()
		{
			ServiceCache.EventBus.Fire(new TimePreFixedUpdateEvent(UnityEngine.Time.fixedDeltaTime));
			ServiceCache.EventBus.Fire(new TimeFixedUpdateEvent(UnityEngine.Time.fixedDeltaTime));

			if (Application.isFocused && !appHasFocus)
			{
				appHasFocus = true;
				OnUnityApplicationFocus(true);
			}
			else if (!Application.isFocused && appHasFocus)
			{
				appHasFocus = false;
				OnUnityApplicationFocus(false);
			}
		}

		private void OnUnityPreCollisionsUpdate()
		{
			ServiceCache.EventBus.Fire(new TimePreCollisionsUpdateEvent(UnityEngine.Time.fixedDeltaTime));
		}

		private void OnUnityPostCollisionsUpdate()
		{
			ServiceCache.EventBus.Fire(new TimePostCollisionsUpdateEvent(UnityEngine.Time.fixedDeltaTime));
		}

		private void OnUnityPostFixedUpdate()
		{
			ServiceCache.EventBus.Fire(new TimePostFixedUpdateEvent(UnityEngine.Time.fixedDeltaTime));
		}

		private void OnUnitySceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			ServiceCache.EventBus.Fire(new UnitySceneLoadedEvent(scene.buildIndex, scene.name, loadSceneMode));
		}

		private void OnUnityApplicationFocus(bool hasFocus)
		{
			ServiceCache.EventBus.Fire(new UnityAppFocusEvent(hasFocus));
		}

		private void HandleLowMemoryWarning()
		{
			Log.Warning("Low memory detected");

			Resources.UnloadUnusedAssets();
		}

#if UNITY_EDITOR
		private void HandleEditorPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
		{
			if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode && !quitting)
			{
				OnUnityApplicationWantsToQuit();
				UnityEditor.EditorApplication.isPlaying = true;
			}
		}
#endif

		public void CancelAppQuit()
		{
			wantsToQuit = false;
		}

		private bool OnUnityApplicationWantsToQuit()
		{
			var result = false;

			if (!quitting)
			{
				wantsToQuit = true;

				ServiceCache.EventBus.Fire<AppQuitRequestedEvent>();

				if (wantsToQuit)
				{
					result = true;
					StartApplicationQuitRoutine();
				}
			}

			return result;
		}

		private void StartApplicationQuitRoutine()
		{
			quitting = true;

			ServiceCache.EventBus.Fire<AppQuitEvent>();

			foreach (var obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
			{
				UnityEngine.Object.Destroy(obj);
			}

			UnsubscribeApplicationStateEvents();

			this.CoroutineStart(DelayedDisposeServices());
		}

		private IEnumerator DelayedDisposeServices()
		{
			yield return null;

			PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());

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
