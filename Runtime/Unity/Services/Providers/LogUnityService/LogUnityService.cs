
#if UNITY_2018_3_OR_NEWER

namespace Ju
{
	public class LogUnityService : ILogUnityService
	{
		public void Setup()
		{
			var logService = Services.Get<ILogService>();
			logService.SetLogLevel(LogLevel.Debug);

			logService.OnDebugMessage += OnDebugMessage;
			logService.OnInfoMessage += OnInfoMessage;
			logService.OnNoticeMessage += OnNoticeMessage;
			logService.OnWarningMessage += OnWarningMessage;
			logService.OnErrorMessage += OnErrorMessage;
		}

		public void Start()
		{
		}

		private void OnDebugMessage(string message, string timeStamp, params object[] args)
		{
			UnityEngine.Debug.Log(timeStamp + " " + (args.Length > 0 ? string.Format(message, args) : message));
		}

		private void OnInfoMessage(string message, string timeStamp, params object[] args)
		{
			UnityEngine.Debug.Log(timeStamp + " " + (args.Length > 0 ? string.Format(message, args) : message));
		}

		private void OnNoticeMessage(string message, string timeStamp, params object[] args)
		{
			UnityEngine.Debug.Log(timeStamp + " " + (args.Length > 0 ? string.Format(message, args) : message));
		}

		private void OnWarningMessage(string message, string timeStamp, params object[] args)
		{
			UnityEngine.Debug.LogWarning(timeStamp + " " + (args.Length > 0 ? string.Format(message, args) : message));
		}

		private void OnErrorMessage(string message, string timeStamp, params object[] args)
		{
			UnityEngine.Debug.LogError(timeStamp + " " + (args.Length > 0 ? string.Format(message, args) : message));
		}
	}
}

#endif
