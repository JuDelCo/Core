// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER
#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System;
using Ju.Extensions;
using Ju.Services;
using Ju.Services.Internal;
using Ju.Log;

[Serializable]
[InitializeOnLoad]
[ExecuteInEditMode]
[DefaultExecutionOrder(int.MinValue)]
public class UnityHotReload : MonoBehaviour, ISerializationCallbackReceiver
{
	[SerializeReference]
	private ServiceContainer serviceContainer;
	private bool servicesNeedReloading;
	private bool logContextEnabled;
	private bool logFolderTrim;
	private bool wasPausedOnCompile;
	private bool pauseOnCompile = true;

	private static UnityHotReload instance;
	private static bool wasInstantiated;

	public static bool IsSafeToUseUnityAPI { get; private set; }

	static UnityHotReload()
	{
		EditorApplication.update += OnEditorUpdate;
	}

	public static void ForceRecompile()
	{
		UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
	}

	public static void SetPauseOnCompile(bool enabled)
	{
		instance.pauseOnCompile = enabled;
	}

	[DidReloadScripts(int.MinValue)]
	internal static void OnReloadScripts()
	{
		if (!wasInstantiated)
		{
			var go = new GameObject("UnityHotReload");
			go.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
			go.AddComponent<UnityHotReload>();

			wasInstantiated = true;
		}
	}

	private static void OnEditorUpdate()
	{
		if (EditorApplication.isCompiling)
		{
			if (EditorApplication.isPlaying)
			{
				IsSafeToUseUnityAPI = false;
				ServiceCache.EventBus.Fire<UnityHotReloadStartEvent>();

				instance.logContextEnabled = Log.contextEnabled;
				instance.logFolderTrim = Log.folderTrim;

				if (instance.pauseOnCompile)
				{
					EditorApplication.isPaused = true;
					instance.wasPausedOnCompile = true;
				}
			}

			EditorApplication.update -= OnEditorUpdate;
		}
	}

	internal void OnEnable()
	{
		instance = this;
		IsSafeToUseUnityAPI = true;

		if (servicesNeedReloading)
		{
			foreach (var servicesPerType in ServiceContainer.instance.container.services)
			{
				foreach (var servicePerId in servicesPerType.Value)
				{
					if (servicePerId.Value is IServiceLoad serviceLoad)
					{
						try
						{
							serviceLoad.Load();
						}
						catch (Exception e)
						{
							Debug.LogException(e);
						}
					}
				}
			}

			if (EditorApplication.isPlaying)
			{
				this.WaitForNextUpdate(true).Then(() =>
				{
					IsSafeToUseUnityAPI = true;
					ServiceCache.EventBus.Fire<UnityHotReloadEndEvent>();
				});
			}

			servicesNeedReloading = false;
		}

		if (EditorApplication.isPlaying && wasPausedOnCompile)
		{
			EditorApplication.isPaused = false;
			wasPausedOnCompile = false;
		}
	}

	public void OnBeforeSerialize()
	{
		serviceContainer = ServiceContainer.instance;
	}

	public void OnAfterDeserialize()
	{
		// Restore static value
		wasInstantiated = true;

		if (serviceContainer != null)
		{
			Log.contextEnabled = logContextEnabled;
			Log.folderTrim = logFolderTrim;

			ServiceContainer.instance = serviceContainer;
			serviceContainer = null;
			servicesNeedReloading = true;
		}
	}
}

#endif
#endif
