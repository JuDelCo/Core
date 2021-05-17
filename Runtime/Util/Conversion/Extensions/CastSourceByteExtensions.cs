// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Data.Conversion
{
	public static class CastSourceByteExtensions
	{
		public static string AsString(this CastSource<byte> source)
		{
			return source.value.ToString();
		}
	}
}
