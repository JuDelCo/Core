// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using UnityEngine;

namespace Ju.Data.Conversion
{
	public static class JDataStringUnityExtensions
	{
		public static UnityEngine.Color AsUnityColor(this JData<string> data, UnityEngine.Color defaultValue = default(UnityEngine.Color))
		{
			return Cast.This(data.Value).AsUnityColor(defaultValue);
		}

		public static Color32 AsUnityColor32(this JData<string> data, Color32 defaultValue = default(Color32))
		{
			return Cast.This(data.Value).AsUnityColor32(defaultValue);
		}

		public static Vector2 AsVector2(this JData<string> data, Vector2 defaultValue = default(Vector2))
		{
			return Cast.This(data.Value).AsVector2(defaultValue);
		}

		public static Vector2Int AsVector2Int(this JData<string> data, Vector2Int defaultValue = default(Vector2Int))
		{
			return Cast.This(data.Value).AsVector2Int(defaultValue);
		}

		public static Vector3 AsVector3(this JData<string> data, Vector3 defaultValue = default(Vector3))
		{
			return Cast.This(data.Value).AsVector3(defaultValue);
		}

		public static Vector3Int AsVector3Int(this JData<string> data, Vector3Int defaultValue = default(Vector3Int))
		{
			return Cast.This(data.Value).AsVector3Int(defaultValue);
		}

		public static Vector4 AsVector4(this JData<string> data, Vector4 defaultValue = default(Vector4))
		{
			return Cast.This(data.Value).AsVector4(defaultValue);
		}

		public static void Set(this JData<string> data, UnityEngine.Color value, bool prependHashChar = false)
		{
			data.Value = Cast.This(value).AsString(prependHashChar);
		}

		public static void Set(this JData<string> data, Color32 value, bool prependHashChar = false)
		{
			data.Value = Cast.This(value).AsString(prependHashChar);
		}

		public static void Set(this JData<string> data, Vector2 value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, Vector2Int value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, Vector3 value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, Vector3Int value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, Vector4 value)
		{
			data.Value = Cast.This(value).AsString();
		}
	}
}

#endif
