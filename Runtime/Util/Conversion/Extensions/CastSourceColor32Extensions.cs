// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using Ju.Color;
using Ju.Data.Conversion;

public static class CastSourceColor32Extensions
{
	public static string AsString(this CastSource<Color32> source, bool prependHashChar = true, bool toUpper = true)
	{
		return $"{(prependHashChar ? "#" : "")}{ConditionalCasing(ToHtmlStringRGBA(source.value), toUpper)}";
	}

	private static string ConditionalCasing(string value, bool toUpper)
	{
		return (toUpper ? value.ToUpper() : value.ToLower());
	}

	private static string ToHtmlStringRGBA(Color32 value)
	{
		return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", value.r, value.g, value.b, value.a);
	}
}
