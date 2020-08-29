using System;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public class Clock : IClock, IDisposable
	{
		private DisposableLinkHandler linkHandler;
		private Span elapsed;
		private readonly Func<bool> updateCondition;

		private void SubscribeEvent(TimeUpdateMode updateMode)
		{
			linkHandler = new DisposableLinkHandler(false);

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

		public Clock(TimeUpdateMode updateMode = TimeUpdateMode.Update)
		{
			SubscribeEvent(updateMode);
		}

		public Clock(float elapsedSeconds, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(updateMode)
		{
			elapsed = Span.Seconds(elapsedSeconds);
		}

		public Clock(Func<bool> updateCondition, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(updateMode)
		{
			this.updateCondition = updateCondition;
		}

		public Clock(float elapsedSeconds, Func<bool> updateCondition, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(elapsedSeconds, updateMode)
		{
			this.updateCondition = updateCondition;
		}

		public void Dispose()
		{
			linkHandler.Dispose();
			GC.SuppressFinalize(this);
		}

		public Span Reset()
		{
			var elapsedTime = elapsed;

			elapsed = Span.zero;

			return elapsedTime;
		}

		public Span GetElapsedTime()
		{
			return elapsed;
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

			elapsed += Span.Seconds(deltaTime);
		}
	}
}
