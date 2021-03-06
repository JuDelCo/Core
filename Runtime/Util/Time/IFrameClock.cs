// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Time
{
	public interface IFrameClock : IDisposable
	{
		int Reset();
		int GetElapsedFrames();
	}
}
