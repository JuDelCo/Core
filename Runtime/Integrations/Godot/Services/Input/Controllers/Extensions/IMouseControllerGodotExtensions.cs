// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Ju.Input;
using Godot;

public static class IMouseControllerGodotExtensions
{
	public static Vector2I GetPosition(this IMouseController mouseController)
	{
		mouseController.GetPosition(out int mouseX, out int mouseY);

		return new Vector2I(mouseX, mouseY);
	}

	public static Vector2 GetPositionDelta(this IMouseController mouseController)
	{
		mouseController.GetPositionDelta(out float mouseX, out float mouseY);

		return new Vector2(mouseX, mouseY);
	}
}

#endif
