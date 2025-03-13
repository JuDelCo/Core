// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Services
{
	public struct CacheAddEvent
	{
		public Type Type { get; private set; }
		public object Object { get; private set; }

		public CacheAddEvent(Type type, object obj)
		{
			Type = type;
			Object = obj;
		}
	}
}
