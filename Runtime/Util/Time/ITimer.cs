
namespace Ju.Time
{
	public interface ITimer
	{
		void Reset();
		void Stop();
		Span GetDuration();
		Span GetElapsedTime();
		Span GetTimeLeft();
	}
}
