// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Data.Conversion
{
	public static class Cast
	{
		public static CastSource<T> This<T>(T value)
		{
			return new CastSource<T>(value);
		}
	}
}
