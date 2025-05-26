// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System.Collections.Generic;
using Ju.Time;

namespace Ju.Services
{
	public class GodotTimeService : TimeService, IServiceLoad
	{
		private List<float> deltaTimeHistory = new List<float>(SMOOTH_AVERAGE_COUNT);
		private List<float> unscaledDeltaTimeHistory = new List<float>(SMOOTH_AVERAGE_COUNT);

		private const int SMOOTH_AVERAGE_COUNT = 10;
		private int originalPhysicsTicksPerSecond;

		public void Load()
		{
			TimeScale = (float) Godot.Engine.TimeScale;

			// Scaled time in seconds since the start of the first Update call. Returns fixedTime when in FixedUpdate.
			Time = 0f;
			// Scaled time in seconds since the start of the first FixedUpdate call.
			FixedTime = 0f;

			originalPhysicsTicksPerSecond = Godot.Engine.PhysicsTicksPerSecond;

			var root = ((Godot.SceneTree) Godot.Engine.GetMainLoop()).Root;

			// Scaled last frame time in seconds. Returns fixedDeltaTime when in FixedUpdate.
			DeltaTime = (float) root.GetProcessDeltaTime();
			DeltaTimeSmooth = (float) root.GetProcessDeltaTime();
			// Scaled last fixed frame time in seconds.
			FixedDeltaTime = (float) root.GetPhysicsProcessDeltaTime();

			// Unscaled time in seconds since the start of the first Update call. Returns fixedUnscaledTime when in FixedUpdate.
			UnscaledTime = (Godot.Time.GetTicksMsec() / 1000f);
			// Unscaled time in seconds since the start of the first FixedUpdate call.
			UnscaledFixedTime = (Godot.Time.GetTicksMsec() / 1000f);

			// Unscaled last frame time in seconds. Returns fixedUnscaledDeltaTime when in FixedUpdate.
			UnscaledDeltaTime = DeltaTime * TimeScale;
			UnscaledDeltaTimeSmooth = UnscaledDeltaTime;
			// Unscaled last fixed frame time in seconds.
			UnscaledFixedDeltaTime = (1f / originalPhysicsTicksPerSecond);

			FrameCount = (uint) Godot.Engine.GetFramesDrawn();

			deltaTimeHistory.Add(DeltaTime);
			unscaledDeltaTimeHistory.Add(UnscaledDeltaTime);

			this.EventSubscribe<TimePreUpdateEvent>(PreUpdate);
			this.EventSubscribe<TimePreFixedUpdateEvent>(PreFixedUpdate);
		}

		public override void SetTimeScale(float timeScale)
		{
			TimeScale = timeScale;

			Godot.Engine.TimeScale = timeScale;
			Godot.Engine.PhysicsTicksPerSecond = (int) System.Math.Round(originalPhysicsTicksPerSecond * timeScale);
		}

		private void PreUpdate()
		{
			var root = ((Godot.SceneTree) Godot.Engine.GetMainLoop()).Root;

			DeltaTime = (float) root.GetProcessDeltaTime();
			UnscaledDeltaTime = DeltaTime * TimeScale;

			Time += DeltaTime;
			UnscaledTime = (Godot.Time.GetTicksMsec() / 1000f);

			CalculateSmoothDeltaTime();

			FrameCount += 1;
		}

		private void CalculateSmoothDeltaTime()
		{
			if (deltaTimeHistory.Count >= SMOOTH_AVERAGE_COUNT)
			{
				deltaTimeHistory.RemoveAt(0);
			}

			if (unscaledDeltaTimeHistory.Count >= SMOOTH_AVERAGE_COUNT)
			{
				unscaledDeltaTimeHistory.RemoveAt(0);
			}

			deltaTimeHistory.Add(DeltaTime);
			unscaledDeltaTimeHistory.Add(UnscaledDeltaTime);

			DeltaTimeSmooth = 0f;
			UnscaledDeltaTimeSmooth = 0f;

			for (int i = 0; i < deltaTimeHistory.Count; ++i)
			{
				DeltaTimeSmooth += deltaTimeHistory[i];
			}

			for (int i = 0; i < unscaledDeltaTimeHistory.Count; ++i)
			{
				UnscaledDeltaTimeSmooth += unscaledDeltaTimeHistory[i];
			}

			DeltaTimeSmooth /= deltaTimeHistory.Count;
			UnscaledDeltaTimeSmooth /= unscaledDeltaTimeHistory.Count;
		}

		private void PreFixedUpdate()
		{
			var root = ((Godot.SceneTree) Godot.Engine.GetMainLoop()).Root;

			FixedDeltaTime = (float) root.GetPhysicsProcessDeltaTime();
			UnscaledFixedDeltaTime = (1f / originalPhysicsTicksPerSecond);

			FixedTime += FixedDeltaTime;
			UnscaledFixedTime = (Godot.Time.GetTicksMsec() / 1000f);

			DeltaTime = FixedDeltaTime;
			DeltaTimeSmooth = FixedDeltaTime;
			UnscaledDeltaTime = UnscaledFixedDeltaTime;
			UnscaledDeltaTimeSmooth = UnscaledFixedDeltaTime;

			Time = FixedTime;
			UnscaledTime = UnscaledFixedTime;
		}
	}
}

#endif
