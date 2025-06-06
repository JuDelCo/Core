// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Data.Conversion;
using UnityEngine;

public static class CastSourceUnityColor32Extensions
{
	public static string AsString(this CastSource<Color32> source, bool prependHashChar = true, bool toUpper = true)
	{
		return $"{(prependHashChar ? "#" : "")}{ConditionalCasing(ColorUtility.ToHtmlStringRGBA(source.value), toUpper)}";
	}

	private static string ConditionalCasing(string value, bool toUpper)
	{
		return (toUpper ? value.ToUpper() : value.ToLower());
	}
}

#endif
