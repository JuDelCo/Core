// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Data.Conversion;
using UnityEngine;

namespace Ju.Hjson
{
	public static class JsonValueUnityExtensions
	{
		public static UnityEngine.Color GetValue(this JsonValue value, UnityEngine.Color defaultValue = default(UnityEngine.Color))
		{
			return Cast.This(value.Qstr()).AsColor(defaultValue);
		}

		public static Color32 GetValue(this JsonValue value, Color32 defaultValue = default(Color32))
		{
			return Cast.This(value.Qstr()).AsColor32(defaultValue);
		}

		public static Vector2 GetValue(this JsonValue value, Vector2 defaultValue = default(Vector2))
		{
			return Cast.This(value.Qstr()).AsVector2(defaultValue);
		}

		public static Vector2Int GetValue(this JsonValue value, Vector2Int defaultValue = default(Vector2Int))
		{
			return Cast.This(value.Qstr()).AsVector2Int(defaultValue);
		}

		public static Vector3 GetValue(this JsonValue value, Vector3 defaultValue = default(Vector3))
		{
			return Cast.This(value.Qstr()).AsVector3(defaultValue);
		}

		public static Vector3Int GetValue(this JsonValue value, Vector3Int defaultValue = default(Vector3Int))
		{
			return Cast.This(value.Qstr()).AsVector3Int(defaultValue);
		}

		public static Vector4 GetValue(this JsonValue value, Vector4 defaultValue = default(Vector4))
		{
			return Cast.This(value.Qstr()).AsVector4(defaultValue);
		}
	}
}

#endif
