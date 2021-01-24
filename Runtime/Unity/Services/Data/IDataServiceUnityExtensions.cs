// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Services;
using UnityEngine;

public static class IDataServiceUnityExtensions
{
	public static T InstanceSO<T>(this IDataService dataService) where T : ScriptableObject
	{
		return ScriptableObject.CreateInstance<T>();
	}

	public static T SetSO<T>(this IDataService dataService, T data) where T : ScriptableObject
	{
		return dataService.SetSO<T>(data, null);
	}

	public static T SetSO<T>(this IDataService dataService, T data, string id) where T : ScriptableObject
	{
		if (data == null)
		{
			data = dataService.InstanceSO<T>();
		}

		if (id == null)
		{
			dataService.Set(data);
		}
		else
		{
			dataService.Set(data, id);
		}

		return data;
	}

	public static T GetSO<T>(this IDataService dataService, bool autoCreate = true) where T : ScriptableObject
	{
		return dataService.GetSO<T>(null, autoCreate);
	}

	public static T GetSO<T>(this IDataService dataService, string id, bool autoCreate = true) where T : ScriptableObject
	{
		T instance;

		if (id == null)
		{
			instance = dataService.Get<T>();
		}
		else
		{
			instance = dataService.Get<T>(id);
		}

		if (instance == null && autoCreate)
		{
			instance = dataService.SetSO<T>(instance, id);
		}

		return instance;
	}
}

#endif
