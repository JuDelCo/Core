// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

namespace Ju.Services
{
	public interface ILogUnityService
	{
		void SetLogLevel(LogLevel logLevel);
		void ToggleStackTraces(bool enabled);
	}
}

#endif
