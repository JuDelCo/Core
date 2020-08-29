using System;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public class FrameClock<T> : IFrameClock, IDisposable where T : ILoopEvent
	{
		private readonly DisposableLinkHandler linkHandler;
		private int elapsed;
		private readonly Func<bool> updateCondition;

		public FrameClock()
		{
			linkHandler = new DisposableLinkHandler(false);
			ServiceContainer.Get<IEventBusService>().Subscribe<T>(linkHandler, _ => Tick());
		}

		public FrameClock(int elapsedFrames) : this()
		{
			elapsed = elapsedFrames;
		}

		public FrameClock(Func<bool> updateCondition) : this()
		{
			this.updateCondition = updateCondition;
		}

		public FrameClock(int elapsedFrames, Func<bool> updateCondition) : this(elapsedFrames)
		{
			this.updateCondition = updateCondition;
		}

		public void Dispose()
		{
			linkHandler.Dispose();
			GC.SuppressFinalize(this);
		}

		public int Reset()
		{
			var elapsedTime = elapsed;

			elapsed = 0;

			return elapsedTime;
		}

		public int GetElapsedFrames()
		{
			return elapsed;
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

			elapsed += 1;
		}
	}
}
