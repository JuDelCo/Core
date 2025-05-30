// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Data.Conversion;
using UnityEngine;

public static class CastSourceUnityColorExtensions
{
	public static string AsString(this CastSource<UnityEngine.Color> source, bool prependHashChar = true, bool toUpper = true)
	{
		return $"{(prependHashChar ? "#" : "")}{ConditionalCasing(ColorUtility.ToHtmlStringRGBA(source.value), toUpper)}";
	}

	private static string ConditionalCasing(string value, bool toUpper)
	{
		return (toUpper ? value.ToUpper() : value.ToLower());
	}
}

#endif
