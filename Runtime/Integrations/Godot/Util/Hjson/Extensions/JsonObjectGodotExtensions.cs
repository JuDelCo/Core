// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;

namespace Ju.Hjson
{
	public static class JsonObjectGodotExtensions
	{
		public static Godot.Color GetValue(this JsonObject obj, string key, Godot.Color defaultValue = default(Godot.Color))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static Vector2 GetValue(this JsonObject obj, string key, Vector2 defaultValue = default(Vector2))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static Vector2I GetValue(this JsonObject obj, string key, Vector2I defaultValue = default(Vector2I))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static Vector3 GetValue(this JsonObject obj, string key, Vector3 defaultValue = default(Vector3))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static Vector3I GetValue(this JsonObject obj, string key, Vector3I defaultValue = default(Vector3I))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static Vector4 GetValue(this JsonObject obj, string key, Vector4 defaultValue = default(Vector4))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}

		public static Vector4I GetValue(this JsonObject obj, string key, Vector4I defaultValue = default(Vector4I))
		{
			return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
		}
	}
}

#endif
