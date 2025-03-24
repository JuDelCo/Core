// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Ju.Data;
using Godot;

public static class JNodeDataGodotExtensions
{
	public static JData<Color> AsDataGodotColor(this JNode node)
	{
		return node.AsData<Color>();
	}

	public static JData<Vector2> AsDataVector2(this JNode node)
	{
		return node.AsData<Vector2>();
	}

	public static JData<Vector2I> AsDataVector2Int(this JNode node)
	{
		return node.AsData<Vector2I>();
	}

	public static JData<Vector3> AsDataVector3(this JNode node)
	{
		return node.AsData<Vector3>();
	}

	public static JData<Vector3I> AsDataVector3Int(this JNode node)
	{
		return node.AsData<Vector3I>();
	}

	public static JData<Vector4> AsDataVector4(this JNode node)
	{
		return node.AsData<Vector4>();
	}

	public static JData<Vector4I> AsDataVector4Int(this JNode node)
	{
		return node.AsData<Vector4I>();
	}
}

#endif
