// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Services
{
	public struct LogEvent
	{
		public LogLevel LogLevel { get; private set; }
		public string Message { get; private set; }
		public string Context { get; private set; }
		public Exception Exception { get; private set; }

		public LogEvent(LogLevel level, string message, Exception exception = null)
		{
			LogLevel = level;
			Message = message;
			Context = null;
			Exception = exception;
		}

		public LogEvent(LogLevel level, string message, string context, Exception exception = null)
		{
			LogLevel = level;
			Message = message;
			Context = context;
			Exception = exception;
		}
	}
}
