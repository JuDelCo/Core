// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Data;
using UnityEngine;

public static class JNodeDataUnityExtensions
{
	public static JData<Color> AsDataUnityColor(this JNode node)
	{
		return node.AsData<Color>();
	}

	public static JData<Color32> AsDataUnityColor32(this JNode node)
	{
		return node.AsData<Color32>();
	}

	public static JData<Vector2> AsDataVector2(this JNode node)
	{
		return node.AsData<Vector2>();
	}

	public static JData<Vector2Int> AsDataVector2Int(this JNode node)
	{
		return node.AsData<Vector2Int>();
	}

	public static JData<Vector3> AsDataVector3(this JNode node)
	{
		return node.AsData<Vector3>();
	}

	public static JData<Vector3Int> AsDataVector3Int(this JNode node)
	{
		return node.AsData<Vector3Int>();
	}

	public static JData<Vector4> AsDataVector4(this JNode node)
	{
		return node.AsData<Vector4>();
	}
}

#endif
