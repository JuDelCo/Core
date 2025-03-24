// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Ju.Input;
using Godot;

public static class IAIInputActionGodotExtensions
{
	public static void Set(this IAIInputAction action, Vector2 value)
	{
		action.Set(value.X, value.Y);
	}
}

#endif
