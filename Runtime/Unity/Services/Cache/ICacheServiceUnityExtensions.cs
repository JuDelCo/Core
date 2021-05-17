// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Services;
using UnityEngine;

public static class ICacheServiceUnityExtensions
{
	public static T InstanceSO<T>(this ICacheService cacheService) where T : ScriptableObject
	{
		return ScriptableObject.CreateInstance<T>();
	}

	public static T SetSO<T>(this ICacheService cacheService, T data) where T : ScriptableObject
	{
		return cacheService.SetSO<T>(data, null);
	}

	public static T SetSO<T>(this ICacheService cacheService, T data, string id) where T : ScriptableObject
	{
		if (data == null)
		{
			data = cacheService.InstanceSO<T>();
		}

		if (id == null)
		{
			cacheService.Set(data);
		}
		else
		{
			cacheService.Set(data, id);
		}

		return data;
	}

	public static T GetSO<T>(this ICacheService cacheService, bool autoCreate = true) where T : ScriptableObject
	{
		return cacheService.GetSO<T>(null, autoCreate);
	}

	public static T GetSO<T>(this ICacheService cacheService, string id, bool autoCreate = true) where T : ScriptableObject
	{
		T instance;

		if (id == null)
		{
			instance = cacheService.Get<T>();
		}
		else
		{
			instance = cacheService.Get<T>(id);
		}

		if (instance == null && autoCreate)
		{
			instance = cacheService.SetSO<T>(null, id);
		}

		return instance;
	}
}

#endif
