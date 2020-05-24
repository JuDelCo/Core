
#if UNITY_2018_3_OR_NEWER

using UnityEngine;

namespace Ju
{
	public class LogUnityService : ILogUnityService
	{
		public void Setup()
		{
			// IMPORTANT: Other services should be accessed always on Start(), not here in Setup().
			// This is a special case because ILogService is always available and we want catch all the log messages from the start.
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
			UnityEngine.Debug.Log((Application.isEditor ? "" : timeStamp + " ") + (args.Length > 0 ? string.Format(message, args) : message));
		}

		private void OnInfoMessage(string message, string timeStamp, params object[] args)
		{
			UnityEngine.Debug.Log((Application.isEditor ? "" : timeStamp + " ") + (args.Length > 0 ? string.Format(message, args) : message));
		}

		private void OnNoticeMessage(string message, string timeStamp, params object[] args)
		{
			UnityEngine.Debug.Log((Application.isEditor ? "" : timeStamp + " ") + (args.Length > 0 ? string.Format(message, args) : message));
		}

		private void OnWarningMessage(string message, string timeStamp, params object[] args)
		{
			UnityEngine.Debug.LogWarning((Application.isEditor ? "" : timeStamp + " ") + (args.Length > 0 ? string.Format(message, args) : message));
		}

		private void OnErrorMessage(string message, string timeStamp, params object[] args)
		{
			UnityEngine.Debug.LogError((Application.isEditor ? "" : timeStamp + " ") + (args.Length > 0 ? string.Format(message, args) : message));
		}
	}
}

#endif
