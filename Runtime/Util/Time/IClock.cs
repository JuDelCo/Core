// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Time
{
	public interface IClock : IDisposable
	{
		Span Reset(float elapsedSeconds = 0f);
		Span GetElapsedTime();
	}
}
