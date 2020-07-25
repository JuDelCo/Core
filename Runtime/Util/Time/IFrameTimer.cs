
namespace Ju.Time
{
	public interface IFrameTimer
	{
		void Reset();
		void Stop();
		int GetDuration();
		int GetElapsedFrames();
		int GetFramesLeft();
	}
}
