// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Input;
using UnityEngine;

public static class IInputActionUnityExtensions
{
	public static Vector2 GetAxisRawValue(this IInputAction action)
	{
		action.GetAxisRawValue(out float axisX, out float axisY);

		return new Vector2(axisX, axisY);
	}
}

#endif
