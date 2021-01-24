// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Input;
using UnityEngine;

public static class IAIInputActionUnityExtensions
{
	public static void Set(this IAIInputAction action, Vector2 value)
	{
		action.Set(value.x, value.y);
	}
}

#endif
