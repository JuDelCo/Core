// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;
using Ju.Data.Conversion;

public static class CastSourceGodotVector2Extensions
{
	public static string AsString(this CastSource<Vector2> source)
	{
		return $"{Cast.This(source.value.X).AsString()}, {Cast.This(source.value.Y).AsString()}";
	}
}

#endif
