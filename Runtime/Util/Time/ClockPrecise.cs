using Ju.TimeUnit;

namespace Ju
{
	public class ClockPrecise
	{
		private Time startTime;

		public ClockPrecise()
		{
			Reset();
		}

		public ClockPrecise(float elapsedSeconds)
		{
			startTime = Time.Seconds(elapsedSeconds);
		}

		public Time Reset()
		{
			var timeElapsed = GetTimeElapsed();

			startTime = Time.Now();

			return timeElapsed;
		}

		public Time GetTimeElapsed()
		{
			return Time.Now() - startTime;
		}
	}
}
