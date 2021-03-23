// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Services
{
	public struct DataRemoveEvent
	{
		public Type Type { get; private set; }
		public object Object { get; private set; }

		public DataRemoveEvent(Type type, object obj)
		{
			Type = type;
			Object = obj;
		}
	}
}
