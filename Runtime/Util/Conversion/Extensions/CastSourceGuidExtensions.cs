// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Data.Conversion
{
	public static class CastSourceGuidExtensions
	{
		public static string AsString(this CastSource<Guid> source)
		{
			return source.value.ToString();
		}
	}
}
