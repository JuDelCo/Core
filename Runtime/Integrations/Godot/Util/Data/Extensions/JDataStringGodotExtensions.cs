// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;

namespace Ju.Data.Conversion
{
	public static class JDataStringGodotExtensions
	{
		public static Godot.Color AsGodotColor(this JData<string> data, Godot.Color defaultValue = default(Godot.Color))
		{
			return Cast.This(data.Value).AsGodotColor(defaultValue);
		}

		public static Vector2 AsVector2(this JData<string> data, Vector2 defaultValue = default(Vector2))
		{
			return Cast.This(data.Value).AsVector2(defaultValue);
		}

		public static Vector2 AsVector2Int(this JData<string> data, Vector2I defaultValue = default(Vector2I))
		{
			return Cast.This(data.Value).AsVector2Int(defaultValue);
		}

		public static Vector3 AsVector3(this JData<string> data, Vector3 defaultValue = default(Vector3))
		{
			return Cast.This(data.Value).AsVector3(defaultValue);
		}

		public static Vector3I AsVector3Int(this JData<string> data, Vector3I defaultValue = default(Vector3I))
		{
			return Cast.This(data.Value).AsVector3Int(defaultValue);
		}

		public static Vector4 AsVector4(this JData<string> data, Vector4 defaultValue = default(Vector4))
		{
			return Cast.This(data.Value).AsVector4(defaultValue);
		}

		public static Vector4I AsVector4Int(this JData<string> data, Vector4I defaultValue = default(Vector4I))
		{
			return Cast.This(data.Value).AsVector4Int(defaultValue);
		}

		public static void Set(this JData<string> data, Godot.Color value, bool prependHashChar = false)
		{
			data.Value = Cast.This(value).AsString(prependHashChar);
		}

		public static void Set(this JData<string> data, Vector2 value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, Vector2I value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, Vector3 value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, Vector3I value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, Vector4 value)
		{
			data.Value = Cast.This(value).AsString();
		}

		public static void Set(this JData<string> data, Vector4I value)
		{
			data.Value = Cast.This(value).AsString();
		}
	}
}

#endif
