
#if UNITY_2019_3_OR_NEWER

using System.Collections.Generic;
using Ju.Services.Extensions;
using Ju.Time;

namespace Ju.Services
{
	public class UnityTimeService : TimeService, IServiceLoad
	{
		private List<float> deltaTimeHistory;
		private List<float> unscaledDeltaTimeHistory;

		private const int SMOOTH_AVERAGE_COUNT = 10;
		private float originalFixedDeltaTime;

		public void Setup()
		{
			deltaTimeHistory = new List<float>(SMOOTH_AVERAGE_COUNT);
			unscaledDeltaTimeHistory = new List<float>(SMOOTH_AVERAGE_COUNT);
		}

		public void Start()
		{
			TimeScale = UnityEngine.Time.timeScale;

			// Scaled time in seconds since the start of the first Update call. Returns fixedTime when in FixedUpdate.
			Time = UnityEngine.Time.time;
			// Scaled time in seconds since the start of the first FixedUpdate call.
			FixedTime = UnityEngine.Time.fixedTime;

			// Scaled last frame time in seconds. Returns fixedDeltaTime when in FixedUpdate.
			DeltaTime = UnityEngine.Time.deltaTime;
			DeltaTimeSmooth = UnityEngine.Time.deltaTime;
			// Scaled last fixed frame time in seconds.
			FixedDeltaTime = UnityEngine.Time.fixedDeltaTime;

			// Unscaled time in seconds since the start of the first Update call. Returns fixedUnscaledTime when in FixedUpdate.
			UnscaledTime = UnityEngine.Time.unscaledTime;
			// Unscaled time in seconds since the start of the first FixedUpdate call.
			UnscaledFixedTime = UnityEngine.Time.fixedUnscaledTime;

			// Unscaled last frame time in seconds. Returns fixedUnscaledDeltaTime when in FixedUpdate.
			UnscaledDeltaTime = UnityEngine.Time.unscaledDeltaTime;
			UnscaledDeltaTimeSmooth = UnityEngine.Time.unscaledDeltaTime;
			// Unscaled last fixed frame time in seconds.
			UnscaledFixedDeltaTime = UnityEngine.Time.fixedUnscaledDeltaTime;

			FrameCount = (uint)UnityEngine.Time.frameCount;

			deltaTimeHistory.Add(DeltaTime);
			unscaledDeltaTimeHistory.Add(UnscaledDeltaTime);

			originalFixedDeltaTime = UnityEngine.Time.fixedDeltaTime;

			this.EventSubscribe<LoopPreUpdateEvent>(PreUpdate);
			this.EventSubscribe<LoopPreFixedUpdateEvent>(PreFixedUpdate);
		}

		public override void SetTimeScale(float timeScale)
		{
			TimeScale = timeScale;

			UnityEngine.Time.timeScale = timeScale;
			UnityEngine.Time.fixedDeltaTime = originalFixedDeltaTime * timeScale;
		}

		private void PreUpdate()
		{
			DeltaTime = UnityEngine.Time.deltaTime;
			UnscaledDeltaTime = UnityEngine.Time.unscaledDeltaTime;

			Time = UnityEngine.Time.time;
			UnscaledTime = UnityEngine.Time.unscaledTime;

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
			FixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
			UnscaledFixedDeltaTime = UnityEngine.Time.fixedUnscaledDeltaTime;

			FixedTime = UnityEngine.Time.fixedTime;
			UnscaledFixedTime = UnityEngine.Time.fixedUnscaledTime;

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
