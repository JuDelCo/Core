// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using UnityEngine;

namespace Ju.Services
{
	public class LogUnityService : ILogUnityService, IServiceLoad
	{
		public void Load()
		{
			var logService = ServiceContainer.Get<ILogService>();
			logService.SetLogLevel(LogLevel.Debug);

			logService.OnDebugMessage += OnDebugMessage;
			logService.OnInfoMessage += OnInfoMessage;
			logService.OnNoticeMessage += OnNoticeMessage;
			logService.OnWarningMessage += OnWarningMessage;
			logService.OnErrorMessage += OnErrorMessage;
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

			if (args.Length > 0 && args[args.Length - 1] is Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
	}
}

#endif
