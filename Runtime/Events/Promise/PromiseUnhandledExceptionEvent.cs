// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Promises
{
	public struct PromiseUnhandledExceptionEvent
	{
		public Exception Exception { get; private set; }

		public PromiseUnhandledExceptionEvent(Exception exception)
		{
			Exception = exception;
		}
	}
}
