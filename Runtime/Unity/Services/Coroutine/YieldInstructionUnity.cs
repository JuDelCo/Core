
#if UNITY_2018_3_OR_NEWER

using UnityEngine;

namespace Ju
{
	public class WaitForSeconds : YieldInstruction
	{
		public override bool KeepWaiting { get { return Time.time < timestamp; } }

		private float timestamp;

		public WaitForSeconds(float seconds)
		{
			timestamp = Time.time + seconds;
		}
	}

	public class WaitForSecondsRealtime : YieldInstruction
	{
		public override bool KeepWaiting { get { return Time.unscaledTime < timestamp; } }

		private float timestamp;

		public WaitForSecondsRealtime(float seconds)
		{
			timestamp = Time.unscaledTime + seconds;
		}
	}
}

#endif
