
namespace Ju.Time
{
	public interface IClock
	{
		Span Reset();
		Span GetElapsedTime();
	}
}
