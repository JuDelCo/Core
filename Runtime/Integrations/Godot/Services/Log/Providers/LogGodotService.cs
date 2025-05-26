// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using System.Text;

namespace Ju.Services
{
	public class LogGodotService : ILogGodotService, IServiceLoad
	{
		private LogLevel minLogLevel = LogLevel.Debug;
		private StringBuilder sb = new StringBuilder();

		public void Load()
		{
			this.EventSubscribe<LogEvent>(OnLogEvent, -1000);

			SetLogLevel(minLogLevel);
		}

		public void SetLogLevel(LogLevel logLevel)
		{
			minLogLevel = logLevel;
		}

		private void OnLogEvent(LogEvent e)
		{
			if (minLogLevel > LogLevel.Error)
			{
				return;
			}

			if (minLogLevel > e.LogLevel)
			{
				return;
			}

			var timeStamp = Godot.Engine.IsEditorHint() ? null : GetCurrentTime() + " ";
			var message = e.Message;

			if (timeStamp != null || e.Context != null)
			{
				message = $"{timeStamp}{e.Message}{e.Context}";
			}

			switch (e.LogLevel)
			{
				case LogLevel.Debug:
				case LogLevel.Info:
				case LogLevel.Notice:
					Godot.GD.Print(message.Replace("{", "{{").Replace("}", "}}"));
					break;
				case LogLevel.Warning:
					Godot.GD.PushWarning(message.Replace("{", "{{").Replace("}", "}}"));
					break;
				case LogLevel.Error:
					Godot.GD.PrintErr(message.Replace("{", "{{").Replace("}", "}}"));
					break;
			}

			if (e.Exception != null)
			{
				Godot.GD.PushError(e.Exception);
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
