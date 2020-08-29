
#if UNITY_2019_3_OR_NEWER

namespace Ju.Services
{
	public class TaskWaitForSeconds : YieldInstruction
	{
		public override bool KeepWaiting { get { return UnityEngine.Time.time < timestamp; } }

		private readonly float timestamp;

		public TaskWaitForSeconds(float seconds)
		{
			timestamp = UnityEngine.Time.time + seconds;
		}
	}

	public class TaskWaitForSecondsRealtime : YieldInstruction
	{
		public override bool KeepWaiting { get { return UnityEngine.Time.unscaledTime < timestamp; } }

		private readonly float timestamp;

		public TaskWaitForSecondsRealtime(float seconds)
		{
			timestamp = UnityEngine.Time.unscaledTime + seconds;
		}
	}
}

#endif
