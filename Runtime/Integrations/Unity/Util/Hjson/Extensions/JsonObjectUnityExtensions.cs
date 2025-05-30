// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using System.Collections.Specialized;
using Ju.Handlers;
using Ju.Hjson;
using UnityEngine;

public static class JsonObjectUnityExtensions
{
	public static void Subscribe(this JsonObject obj, Behaviour behaviour, Action<NotifyCollectionChangedEventArgs> action, bool alwaysActive = false)
	{
		obj.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action);
	}

	public static UnityEngine.Color GetValue(this JsonObject obj, string key, UnityEngine.Color defaultValue = default(UnityEngine.Color))
	{
		return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
	}

	public static Color32 GetValue(this JsonObject obj, string key, Color32 defaultValue = default(Color32))
	{
		return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
	}

	public static Vector2 GetValue(this JsonObject obj, string key, Vector2 defaultValue = default(Vector2))
	{
		return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
	}

	public static Vector2Int GetValue(this JsonObject obj, string key, Vector2Int defaultValue = default(Vector2Int))
	{
		return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
	}

	public static Vector3 GetValue(this JsonObject obj, string key, Vector3 defaultValue = default(Vector3))
	{
		return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
	}

	public static Vector3Int GetValue(this JsonObject obj, string key, Vector3Int defaultValue = default(Vector3Int))
	{
		return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
	}

	public static Vector4 GetValue(this JsonObject obj, string key, Vector4 defaultValue = default(Vector4))
	{
		return obj.ContainsKey(key) ? obj[key].GetValue(defaultValue) : defaultValue;
	}
}

#endif
