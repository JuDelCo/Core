using System;
using System.Text;

namespace Ju
{
	public class LogService : ILogService
	{
		public event LogServiceEvent OnDebugMessage = delegate { };
		public event LogServiceEvent OnInfoMessage = delegate { };
		public event LogServiceEvent OnNoticeMessage = delegate { };
		public event LogServiceEvent OnWarningMessage = delegate { };
		public event LogServiceEvent OnErrorMessage = delegate { };
		public event LogGeneralServiceEvent OnAnyMessage = delegate { };

		private LogLevel minLogLevel = LogLevel.Debug;
		private StringBuilder sb;

		public void Setup()
		{
			sb = new StringBuilder();
		}

		public void Start()
		{
		}

		public void SetLogLevel(LogLevel logLevel)
		{
			minLogLevel = logLevel;
		}

		public void SubscribeLoggable(ILoggableService loggable, LogLevel logLevel = LogLevel.Debug)
		{
			if (logLevel <= LogLevel.Debug)
			{
				loggable.OnLogDebug += Debug;
			}

			if (logLevel <= LogLevel.Info)
			{
				loggable.OnLogInfo += Info;
			}

			if (logLevel <= LogLevel.Notice)
			{
				loggable.OnLogNotice += Notice;
			}

			if (logLevel <= LogLevel.Warning)
			{
				loggable.OnLogWarning += Warning;
			}

			if (logLevel <= LogLevel.Error)
			{
				loggable.OnLogError += Error;
			}
		}

		public void Debug(string message, params object[] args)
		{
			if (minLogLevel <= LogLevel.Debug)
			{
				var currentTime = GetCurrentTime();
				OnDebugMessage(message, currentTime, args);
				OnAnyMessage(message, currentTime, LogLevel.Debug, args);
			}
		}

		public void Info(string message, params object[] args)
		{
			if (minLogLevel <= LogLevel.Info)
			{
				var currentTime = GetCurrentTime();
				OnInfoMessage(message, currentTime, args);
				OnAnyMessage(message, currentTime, LogLevel.Info, args);
			}
		}

		public void Notice(string message, params object[] args)
		{
			if (minLogLevel <= LogLevel.Notice)
			{
				var currentTime = GetCurrentTime();
				OnNoticeMessage(message, currentTime, args);
				OnAnyMessage(message, currentTime, LogLevel.Notice, args);
			}
		}

		public void Warning(string message, params object[] args)
		{
			if (minLogLevel <= LogLevel.Warning)
			{
				var currentTime = GetCurrentTime();
				OnWarningMessage(message, currentTime, args);
				OnAnyMessage(message, currentTime, LogLevel.Warning, args);
			}
		}

		public void Error(string message, params object[] args)
		{
			if (minLogLevel <= LogLevel.Error)
			{
				var currentTime = GetCurrentTime();
				OnErrorMessage(message, currentTime, args);
				OnAnyMessage(message, currentTime, LogLevel.Error, args);
			}
		}

		protected string GetCurrentTime()
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
