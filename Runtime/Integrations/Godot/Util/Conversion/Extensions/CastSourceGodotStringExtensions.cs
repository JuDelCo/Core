// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Godot;
using Ju.Data.Conversion;

public static class CastSourceGodotStringExtensions
{
	public static Godot.Color AsGodotColor(this CastSource<string> source, Godot.Color defaultValue = default(Godot.Color))
	{
		var value = source.value;

		// Not needed
		// if (source.value.Length <= 8 && !source.value.StartsWith("#"))
		// {
		// 	value = "#" + source.value;
		// }

		var result = defaultValue;

		try
		{
			result = Godot.Color.FromHtml(value);
		}
		catch { }

		return result;
	}

	private static readonly char[] vectorStringDelimiters = new char[] { '(', ',', ')' };

	public static Vector2 AsVector2(this CastSource<string> source, Vector2 defaultValue = default(Vector2))
	{
		var result = defaultValue;
		var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

		if (splittedValue.Length == 2 && float.TryParse(splittedValue[0], out float x) && float.TryParse(splittedValue[1], out float y))
		{
			result = new Vector2(x, y);
		}

		return result;
	}

	public static Vector2I AsVector2Int(this CastSource<string> source, Vector2I defaultValue = default(Vector2I))
	{
		var result = defaultValue;
		var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

		if (splittedValue.Length == 2 && int.TryParse(splittedValue[0], out int x) && int.TryParse(splittedValue[1], out int y))
		{
			result = new Vector2I(x, y);
		}

		return result;
	}

	public static Vector3 AsVector3(this CastSource<string> source, Vector3 defaultValue = default(Vector3))
	{
		var result = defaultValue;
		var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

		if (splittedValue.Length == 3 && float.TryParse(splittedValue[0], out float x) && float.TryParse(splittedValue[1], out float y) && float.TryParse(splittedValue[2], out float z))
		{
			result = new Vector3(x, y, z);
		}

		return result;
	}

	public static Vector3I AsVector3Int(this CastSource<string> source, Vector3I defaultValue = default(Vector3I))
	{
		var result = defaultValue;
		var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

		if (splittedValue.Length == 3 && int.TryParse(splittedValue[0], out int x) && int.TryParse(splittedValue[1], out int y) && int.TryParse(splittedValue[2], out int z))
		{
			result = new Vector3I(x, y, z);
		}

		return result;
	}

	public static Vector4 AsVector4(this CastSource<string> source, Vector4 defaultValue = default(Vector4))
	{
		var result = defaultValue;
		var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

		if (splittedValue.Length == 4 && float.TryParse(splittedValue[0], out float x) && float.TryParse(splittedValue[1], out float y) && float.TryParse(splittedValue[2], out float z) && float.TryParse(splittedValue[3], out float w))
		{
			result = new Vector4(x, y, z, w);
		}

		return result;
	}

	public static Vector4I AsVector4Int(this CastSource<string> source, Vector4I defaultValue = default(Vector4I))
	{
		var result = defaultValue;
		var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

		if (splittedValue.Length == 4 && int.TryParse(splittedValue[0], out int x) && int.TryParse(splittedValue[1], out int y) && int.TryParse(splittedValue[2], out int z) && int.TryParse(splittedValue[3], out int w))
		{
			result = new Vector4I(x, y, z, w);
		}

		return result;
	}
}

#endif
