using System;
using Ju.TimeUnit;

namespace Ju
{
	public class Clock : IDisposable
	{
		private DisposableLinkHandler linkHandler;
		private Time elapsed;
		private Func<bool> updateCondition;

		private void SubscribeEvent(TimeUpdateMode updateMode)
		{
			linkHandler = new DisposableLinkHandler(false);

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

		public Clock(TimeUpdateMode updateMode = TimeUpdateMode.Update)
		{
			SubscribeEvent(updateMode);
		}

		public Clock(float elapsedSeconds, TimeUpdateMode updateMode = TimeUpdateMode.Update) : this(updateMode)
		{
			elapsed = Time.Seconds(elapsedSeconds);
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

		public Time Reset()
		{
			var timeElapsed = elapsed;

			elapsed = Time.zero;

			return timeElapsed;
		}

		public Time GetElapsedTime()
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

			elapsed += Time.Seconds(deltaTime);
		}
	}
}
