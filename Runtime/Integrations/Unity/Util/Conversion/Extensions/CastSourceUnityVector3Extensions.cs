// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Data.Conversion;
using UnityEngine;

public static class CastSourceUnityVector3Extensions
{
	public static string AsString(this CastSource<Vector3> source)
	{
		return $"{Cast.This(source.value.x).AsString()}, {Cast.This(source.value.y).AsString()}, {Cast.This(source.value.z).AsString()}";
	}
}

#endif
