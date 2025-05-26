// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using Ju.Data.Conversion;

public static class CastSourceShortExtensions
{
	public static string AsString(this CastSource<short> source)
	{
		return source.value.ToString();
	}
}
