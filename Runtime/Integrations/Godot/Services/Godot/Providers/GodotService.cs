// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using System.Collections;
using Ju.Time;
using Godot;

namespace Ju.Services
{
	using Ju.Log;
	using Ju.Services.Internal;

	public class GodotService : IGodotService, IServiceLoad
	{
		private static bool initialized = false;
		private bool wantsToQuit = false;
		private bool quitting = false;
		private Node root;
		private static Type processProxyNodeType;
		private IGodotProcessProxyNode processProxyNode;

		public void Load()
		{
			if (OS.HasFeature("debug"))
			{
				Log.Info("[GodotService] Debug mode detected");
			}

			if (System.Diagnostics.Debugger.IsAttached)
			{
				Log.Info("[GodotService] NET Debugger detected");
			}

			root = ((SceneTree) Engine.GetMainLoop()).Root;
			root.GetTree().AutoAcceptQuit = false;
			root.GetTree().QuitOnGoBack = false;
			root.GetTree().NodeAdded += OnNodeAdded;

			SubscribeGodotInternalEvents();

			initialized = true;
		}

		private void OnNodeAdded(Node node)
		{
			if (node.GetParent() == root && !string.IsNullOrEmpty(node.SceneFilePath))
			{
				// Alternative: root.GetTree().CurrentScene
				ServiceCache.EventBus.Fire(new GodotSceneLoadedEvent(node.Name, node.SceneFilePath));
			}
		}

		public static void SetProcessProxyNodeType(Type processProxyNodeType)
		{
			if (initialized)
			{
				GD.PushError("[GodotService] SetProcessProxyNodeType must be called before loading the service");
				return;
			}

			if (typeof(IGodotProcessProxyNode).IsAssignableFrom(processProxyNodeType))
			{
				GodotService.processProxyNodeType = processProxyNodeType;
			}
		}

		private void SubscribeGodotInternalEvents()
		{
			if (processProxyNode == null)
			{
				if (processProxyNodeType != null)
				{
					var obj = Activator.CreateInstance(processProxyNodeType);

					if (obj is IGodotProcessProxyNode && obj is Node)
					{
						var node = (obj as Node);
						node.Name = "NodeProcessProxy";
						node.SetProcessInternal(true);
						node.SetPhysicsProcessInternal(true);
						node.ProcessMode = Node.ProcessModeEnum.Always;

						processProxyNode = (obj as IGodotProcessProxyNode);
						processProxyNode.OnNotificationEvent += ProcessProxyNodeOnNotificationEvent;
						processProxyNode.OnInputEvent += ProcessProxyNodeOnInputEvent;

						root.CallDeferred(Node.MethodName.AddChild, (Node) processProxyNode);

						Log.Info("[GodotService] GodotService loaded successfully");
					}
					else
					{
						Log.Error("[GodotService] Proxy node type must inherit from Node");
					}
				}
				else
				{
					Log.Error("[GodotService] No valid process proxy node type has been defined");
				}
			}
		}

		private void ProcessProxyNodeOnNotificationEvent(int what)
		{
			if (what == Node.NotificationInternalProcess)
			{
				var deltaTime = (float) processProxyNode.GetProcessDeltaTime();

				ServiceCache.EventBus.Fire(new TimePreUpdateEvent(deltaTime));
				ServiceCache.EventBus.Fire(new TimeUpdateEvent(deltaTime));

				processProxyNode.ProxyCallDeferred(() =>
				{
					ServiceCache.EventBus.Fire(new TimePostUpdateEvent(deltaTime));
				});
			}
			else if (what == Node.NotificationInternalPhysicsProcess)
			{
				var fixedDeltaTime = (float) processProxyNode.GetPhysicsProcessDeltaTime();

				ServiceCache.EventBus.Fire(new TimePreFixedUpdateEvent(fixedDeltaTime));
				ServiceCache.EventBus.Fire(new TimeFixedUpdateEvent(fixedDeltaTime));

				ServiceCache.EventBus.Fire(new TimePreCollisionsUpdateEvent(fixedDeltaTime));

				processProxyNode.ProxyCallDeferred(() =>
				{
					ServiceCache.EventBus.Fire(new TimePostCollisionsUpdateEvent((fixedDeltaTime)));
					ServiceCache.EventBus.Fire(new TimePostFixedUpdateEvent(fixedDeltaTime));
				});
			}
			else if (what == Node.NotificationApplicationFocusIn)
			{
				// Alternative: Node.NotificationWMWindowFocusIn
				ServiceCache.EventBus.Fire(new GodotAppFocusEvent(true));
			}
			else if (what == Node.NotificationApplicationFocusOut)
			{
				// Alternative: Node.NotificationWMWindowFocusOut
				ServiceCache.EventBus.Fire(new GodotAppFocusEvent(false));
			}
			else if (what == Node.NotificationWMCloseRequest)
			{
				OnGodotApplicationWantsToQuit();
			}
			else if (what == Node.NotificationWMGoBackRequest)
			{
				// (Android) Notification received from the OS when a go back request is sent (e.g. pressing the "Back" button on Android).
			}
			else if (what == Node.NotificationApplicationResumed)
			{
				// (Android / iOS) Notification received from the OS when the application is resumed.
			}
			else if (what == Node.NotificationApplicationPaused)
			{
				// (Android / iOS) Notification received from the OS when the application is paused.
				// Note: On iOS, you only have approximately 5 seconds to finish a task started by this signal. If you go over this allotment, iOS will kill the app instead of pausing it.
			}
			else if (what == Node.NotificationCrash)
			{
				Log.Error("[GodotService] Crash !!!");
			}

			ServiceCache.EventBus.Fire(new GodotAppNotificationEvent(what));
		}

		private void ProcessProxyNodeOnInputEvent(InputEvent inputEvent)
		{
			ServiceCache.EventBus.Fire(new GodotInputEvent(inputEvent));
		}

		public void AppQuit()
		{
			root.GetTree().Root.PropagateNotification((int) Node.NotificationWMCloseRequest);
		}

		public void CancelAppQuit()
		{
			wantsToQuit = false;
		}

		private void OnGodotApplicationWantsToQuit()
		{
			if (quitting)
			{
				return;
			}

			wantsToQuit = true;
			ServiceCache.EventBus.Fire<AppQuitRequestedEvent>();

			if (wantsToQuit)
			{
				StartApplicationQuitRoutine();
			}
		}

		private void StartApplicationQuitRoutine()
		{
			quitting = true;

			ServiceCache.EventBus.Fire<AppQuitEvent>();

			foreach (var node in root.GetChildren())
			{
				if (node == (processProxyNode as Node))
				{
					continue;
				}

				node.QueueFree();
			}

			root.GetTree().NodeAdded -= OnNodeAdded;

			this.CoroutineStart(DelayedDisposeServices());
		}

		private IEnumerator DelayedDisposeServices()
		{
			yield return null;

			if (processProxyNode != null && GodotObject.IsInstanceValid((processProxyNode as Node)))
			{
				(processProxyNode as Node).QueueFree();
			}

			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, false);
			GC.WaitForPendingFinalizers();

			ServiceContainer.Dispose();

			root.GetTree().AutoAcceptQuit = true;
			root.GetTree().QuitOnGoBack = true;
			root.GetTree().Quit();
		}
	}
}

#endif
