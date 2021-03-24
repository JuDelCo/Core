// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using System.Text;
using UnityEngine;
using Ju.Services.Extensions;

namespace Ju.Services
{
	public class LogUnityService : ILogUnityService, IServiceLoad
	{
		private LogLevel minLogLevel = LogLevel.Debug;
		private bool generateStackTraces = false;
		private StringBuilder sb;

		public void Load()
		{
			sb = new StringBuilder();

			this.EventSubscribe<LogEvent>(OnLogEvent);

			SetLogLevel(minLogLevel);
		}

		public void SetLogLevel(LogLevel logLevel)
		{
			minLogLevel = logLevel;

			switch (logLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				case LogLevel.Notice:
					Debug.unityLogger.filterLogType = LogType.Log;
					break;
				case LogLevel.Warning:
					Debug.unityLogger.filterLogType = LogType.Warning;
					break;
				case LogLevel.Error:
					Debug.unityLogger.filterLogType = LogType.Error;
					break;
			}
		}

		public void ToggleStackTraces(bool enabled)
		{
			generateStackTraces = enabled;
		}

		private void OnLogEvent(LogEvent e)
		{
			if (minLogLevel > LogLevel.Error)
			{
				return;
			}

			var timeStamp = Application.isEditor ? null : GetCurrentTime() + " ";
			var message = e.Message;

			if (!(timeStamp is null) || !(e.Context is null))
			{
				message = $"{timeStamp}{e.Message}{e.Context}";
			}

			switch (e.LogLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				case LogLevel.Notice:
					if (generateStackTraces)
					{
						UnityEngine.Debug.Log(message);
					}
					else
					{
						UnityEngine.Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, message);
					}
					break;
				case LogLevel.Warning:
					if (generateStackTraces)
					{
						UnityEngine.Debug.LogWarning(message);
					}
					else
					{
						UnityEngine.Debug.LogFormat(LogType.Warning, LogOption.NoStacktrace, null, message);
					}
					break;
				case LogLevel.Error:
					if (generateStackTraces)
					{
						UnityEngine.Debug.LogError(message);
					}
					else
					{
						UnityEngine.Debug.LogFormat(LogType.Error, LogOption.NoStacktrace, null, message);
					}
					break;
			}

			if (!(e.Exception is null))
			{
				UnityEngine.Debug.LogException(e.Exception);
			}
		}

		private string GetCurrentTime()
		{
			var now = DateTime.Now;

			sb.Length = 0;
			sb.Append(now.Hour.ToString().PadLeft(2, '0'));
			sb.Append(":");
			sb.Append(now.Minute.ToString().PadLeft(2, '0'));
			sb.Append(":");
			sb.Append(now.Second.ToString().PadLeft(2, '0'));

			return sb.ToString();
		}
	}
}

#endif
