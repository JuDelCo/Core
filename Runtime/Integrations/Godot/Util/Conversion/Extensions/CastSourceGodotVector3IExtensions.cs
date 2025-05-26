// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;
using Ju.Data.Conversion;

public static class CastSourceGodotVector3IExtensions
{
	public static string AsString(this CastSource<Vector3I> source)
	{
		return $"{Cast.This(source.value.X).AsString()}, {Cast.This(source.value.Y).AsString()}, {Cast.This(source.value.Z).AsString()}";
	}
}

#endif
