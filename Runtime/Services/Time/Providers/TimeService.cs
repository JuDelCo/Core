
namespace Ju.Services
{
	public abstract class TimeService : ITimeService
	{
		public float TimeScale { get; protected set; }

		public float Time { get; protected set; }
		public float FixedTime { get; protected set; }

		public float DeltaTime { get; protected set; }
		public float DeltaTimeSmooth { get; protected set; }
		public float FixedDeltaTime { get; protected set; }

		public float UnscaledTime { get; protected set; }
		public float UnscaledFixedTime { get; protected set; }

		public float UnscaledDeltaTime { get; protected set; }
		public float UnscaledDeltaTimeSmooth { get; protected set; }
		public float UnscaledFixedDeltaTime { get; protected set; }

		public uint FrameCount { get; protected set; }

		public abstract void SetTimeScale(float timeScale);
	}
}
