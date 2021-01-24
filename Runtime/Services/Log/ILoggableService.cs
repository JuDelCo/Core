// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Services
{
	public delegate void LogMessageEvent(string message, params object[] args);

	public interface ILoggableService
	{
		event LogMessageEvent OnLogDebug;
		event LogMessageEvent OnLogInfo;
		event LogMessageEvent OnLogNotice;
		event LogMessageEvent OnLogWarning;
		event LogMessageEvent OnLogError;
	}
}
