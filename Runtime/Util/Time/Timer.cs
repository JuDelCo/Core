using System;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public class Timer : ITimer, IDisposable
	{
		private DisposableLinkHandler linkHandler;
		private Span elapsed;
		private readonly Span duration;
		private readonly Func<bool> updateCondition;
		private readonly Action onCompleted;

		private void SubscribeEvent(TimeUpdateMode updateMode)
		{
			linkHandler = new DisposableLinkHandler(true);

			var eventBusService = ServiceContainer.Get<IEventBusService>();

			if (updateMode == TimeUpdateMode.Update)
			{
				eventBusService.Subscribe<LoopUpdateEvent>(linkHandler, e => Update(e.deltaTime));
			}
			else
			{
				eventBusService.Subscribe<LoopFixedUpdateEvent>(linkHandler, e => Update(e.fixedDeltaTime));
			}
		}

		private Timer(float seconds, TimeUpdateMode updateMode = TimeUpdateMode.Update)
		{
			SubscribeEvent(updateMode);
			this.duration = Span.Seconds(seconds);
		}

		public Timer(float seconds, Action onCompleted, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(seconds, updateMode)
		{
			this.onCompleted = onCompleted;
		}

		public Timer(float seconds, Action onCompleted, Func<bool> updateCondition, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(seconds, onCompleted, updateMode)
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
			elapsed = Span.zero;
		}

		public void Stop()
		{
			elapsed = duration;
		}

		public Span GetDuration()
		{
			return duration;
		}

		public Span GetElapsedTime()
		{
			return elapsed;
		}

		public Span GetTimeLeft()
		{
			return duration - elapsed;
		}

		private void Update(float deltaTime)
		{
			if (updateCondition != null)
			{
				if (!updateCondition())
				{
					return;
				}
			}

			var completed = elapsed >= duration;

			elapsed += Span.Seconds(deltaTime);

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
