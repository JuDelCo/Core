// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Data.Conversion
{
	public struct CastSource<T>
	{
		public readonly T value;

		public CastSource(T value)
		{
			this.value = value;
		}
	}
}
