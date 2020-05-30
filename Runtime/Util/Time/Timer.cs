using System;
using Ju.TimeUnit;

namespace Ju
{
	public class Timer : IDisposable
	{
		private DisposableLinkHandler linkHandler;
		private Time elapsed;
		private Time duration;
		private Func<bool> updateCondition;
		private Action onCompleted;

		private void SubscribeEvent(TimeUpdateMode updateMode)
		{
			linkHandler = new DisposableLinkHandler(true);

			var eventBusService = Services.Get<IEventBusService>();

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
			this.duration = Time.Seconds(seconds);
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
			elapsed = Time.zero;
		}

		public void Stop()
		{
			elapsed = duration;
		}

		public Time GetDuration()
		{
			return duration;
		}

		public Time GetElapsedTime()
		{
			return elapsed;
		}

		public Time GetTimeLeft()
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

			elapsed += Time.Seconds(deltaTime);

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
