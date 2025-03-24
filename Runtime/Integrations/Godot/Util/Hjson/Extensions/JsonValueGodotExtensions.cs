// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Ju.Data.Conversion;
using Godot;

namespace Ju.Hjson
{
	public static class JsonValueGodotExtensions
	{
		public static Godot.Color GetValue(this JsonValue value, Godot.Color defaultValue = default(Godot.Color))
		{
			return Cast.This(value.Qstr()).AsColor(defaultValue);
		}

		public static Vector2 GetValue(this JsonValue value, Vector2 defaultValue = default(Vector2))
		{
			return Cast.This(value.Qstr()).AsVector2(defaultValue);
		}

		public static Vector2I GetValue(this JsonValue value, Vector2I defaultValue = default(Vector2I))
		{
			return Cast.This(value.Qstr()).AsVector2Int(defaultValue);
		}

		public static Vector3 GetValue(this JsonValue value, Vector3 defaultValue = default(Vector3))
		{
			return Cast.This(value.Qstr()).AsVector3(defaultValue);
		}

		public static Vector3I GetValue(this JsonValue value, Vector3I defaultValue = default(Vector3I))
		{
			return Cast.This(value.Qstr()).AsVector3Int(defaultValue);
		}

		public static Vector4 GetValue(this JsonValue value, Vector4 defaultValue = default(Vector4))
		{
			return Cast.This(value.Qstr()).AsVector4(defaultValue);
		}

		public static Vector4I GetValue(this JsonValue value, Vector4I defaultValue = default(Vector4I))
		{
			return Cast.This(value.Qstr()).AsVector4Int(defaultValue);
		}
	}
}

#endif
