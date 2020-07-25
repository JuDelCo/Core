using System;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public class FrameClock : IFrameClock, IDisposable
	{
		private DisposableLinkHandler linkHandler;
		private int elapsed;
		private Func<bool> updateCondition;

		private void SubscribeEvent(TimeUpdateMode updateMode)
		{
			linkHandler = new DisposableLinkHandler(false);

			var eventBusService = ServiceContainer.Get<IEventBusService>();

			if (updateMode == TimeUpdateMode.Update)
			{
				eventBusService.Subscribe<LoopUpdateEvent>(linkHandler, e => Tick());
			}
			else
			{
				eventBusService.Subscribe<LoopFixedUpdateEvent>(linkHandler, e => Tick());
			}
		}

		public FrameClock(TimeUpdateMode updateMode = TimeUpdateMode.Update)
		{
			SubscribeEvent(updateMode);
		}

		public FrameClock(int elapsedFrames, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(updateMode)
		{
			elapsed = elapsedFrames;
		}

		public FrameClock(Func<bool> updateCondition, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(updateMode)
		{
			this.updateCondition = updateCondition;
		}

		public FrameClock(int elapsedFrames, Func<bool> updateCondition, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(elapsedFrames, updateMode)
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
