// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Ju.Input;
using Godot;

public static class IGamepadControllerGodotExtensions
{
	public static Vector2 GetAxisRaw(this IGamepadController gamepadController, GamepadAxis axis)
	{
		gamepadController.GetAxisRaw(axis, out float axisX, out float axisY);

		return new Vector2(axisX, axisY);
	}
}

#endif
