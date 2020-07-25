using System;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public class FrameTimer : IFrameTimer, IDisposable
	{
		private DisposableLinkHandler linkHandler;
		private int elapsed;
		private int duration;
		private Func<bool> updateCondition;
		private Action onCompleted;

		private void SubscribeEvent(TimeUpdateMode updateMode)
		{
			linkHandler = new DisposableLinkHandler(true);

			var eventBusService = ServiceContainer.Get<IEventBusService>();

			if (updateMode == TimeUpdateMode.Update)
			{
				eventBusService.Subscribe<LoopUpdateEvent>(linkHandler, _ => Tick());
			}
			else
			{
				eventBusService.Subscribe<LoopFixedUpdateEvent>(linkHandler, _ => Tick());
			}
		}

		private FrameTimer(int frames, TimeUpdateMode updateMode = TimeUpdateMode.Update)
		{
			SubscribeEvent(updateMode);
			this.duration = frames;
		}

		public FrameTimer(int frames, Action onCompleted, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(frames, updateMode)
		{
			this.onCompleted = onCompleted;
		}

		public FrameTimer(int frames, Action onCompleted, Func<bool> updateCondition, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(frames, onCompleted, updateMode)
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
