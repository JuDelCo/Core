// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using UnityEngine;

namespace Ju.Data.Conversion
{
	public static class CastSourceUnityVector2IntExtensions
	{
		public static string AsString(this CastSource<Vector2Int> source)
		{
			return $"{Cast.This(source.value.x).AsString()}, {Cast.This(source.value.y).AsString()}";
		}
	}
}

#endif
