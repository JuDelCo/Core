// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Services
{
	public delegate void LogServiceEvent(string message, string timeStamp, params object[] args);
	public delegate void LogGeneralServiceEvent(string message, string timeStamp, LogLevel logLevel, params object[] args);

	public interface ILogService : IServiceLoad
	{
		event LogServiceEvent OnDebugMessage;
		event LogServiceEvent OnInfoMessage;
		event LogServiceEvent OnNoticeMessage;
		event LogServiceEvent OnWarningMessage;
		event LogServiceEvent OnErrorMessage;
		event LogGeneralServiceEvent OnAnyMessage;

		void SetLogLevel(LogLevel logLevel);
		void SubscribeLoggable(ILoggableService loggable, LogLevel logLevel = LogLevel.Debug);

		void Debug(string message, params object[] args);
		void Info(string message, params object[] args);
		void Notice(string message, params object[] args);
		void Warning(string message, params object[] args);
		void Error(string message, params object[] args);
	}
}
