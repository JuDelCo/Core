using System;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public class FrameTimer<T> : IFrameTimer, IDisposable where T : ILoopEvent
	{
		private readonly DisposableLinkHandler linkHandler;
		private int elapsed;
		private readonly int duration;
		private readonly Func<bool> updateCondition;
		private readonly Action onCompleted;

		private FrameTimer(int frames)
		{
			linkHandler = new DisposableLinkHandler(false);
			ServiceContainer.Get<IEventBusService>().Subscribe<T>(linkHandler, _ => Tick());

			this.duration = frames;
		}

		public FrameTimer(int frames, Action onCompleted) : this(frames)
		{
			this.onCompleted = onCompleted;
		}

		public FrameTimer(int frames, Action onCompleted, Func<bool> updateCondition) : this(frames, onCompleted)
		{
			this.updateCondition = updateCondition;
		}

		public void Dispose()
		{
			linkHandler.Dispose();
			GC.SuppressFinalize(this);
		}

		public void Reset()
		{
			elapsed = 0;
		}

		public void Stop()
		{
			elapsed = duration;
		}

		public int GetDuration()
		{
			return duration;
		}

		public int GetElapsedFrames()
		{
			return elapsed;
		}

		public int GetFramesLeft()
		{
			return duration - elapsed;
		}

		private void Tick()
		{
			if (updateCondition != null)
			{
				if (!updateCondition())
				{
					return;
				}
			}

			var completed = elapsed >= duration;

			elapsed += 1;

			if (!completed && elapsed >= duration)
			{
				if (onCompleted != null)
				{
					onCompleted();
				}
			}
		}
	}
}
