// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Input;
using UnityEngine;

public static class IMouseControllerUnityExtensions
{
	public static Vector2Int GetPosition(this IMouseController mouseController)
	{
		mouseController.GetPosition(out int mouseX, out int mouseY);

		return new Vector2Int(mouseX, mouseY);
	}

	public static Vector2 GetPositionDelta(this IMouseController mouseController)
	{
		mouseController.GetPositionDelta(out float mouseX, out float mouseY);

		return new Vector2(mouseX, mouseY);
	}
}

#endif
