// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Ju.Input;
using Godot;

public static class IInputActionGodotExtensions
{
	public static Vector2 GetAxisRawValue(this IInputAction action)
	{
		action.GetAxisRawValue(out float axisX, out float axisY);

		return new Vector2(axisX, axisY);
	}
}

#endif
