// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Ju.Services;
using Godot;

public static class ICacheServiceGodotExtensions
{
	public static T InstanceResource<T>(this ICacheService cacheService) where T : Resource, new()
	{
		return new T();
	}

	public static T SetResource<T>(this ICacheService cacheService, T data) where T : Resource, new()
	{
		return cacheService.SetResource<T>(data, null);
	}

	public static T SetResource<T>(this ICacheService cacheService, T data, string id) where T : Resource, new()
	{
		if (data == null)
		{
			data = cacheService.InstanceResource<T>();
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

	public static T GetResource<T>(this ICacheService cacheService, bool autoCreate = true) where T : Resource, new()
	{
		return cacheService.GetResource<T>(null, autoCreate);
	}

	public static T GetResource<T>(this ICacheService cacheService, string id, bool autoCreate = true) where T : Resource, new()
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
			instance = cacheService.SetResource<T>(null, id);
		}

		return instance;
	}
}

#endif
