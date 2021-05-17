// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;
using Ju.Services;
using Identifier = System.String;

public static class ICacheServiceExtensions
{
	private static readonly Identifier DEFAULT_ID = "base";

	public static void Set<T>(this ICacheService cacheService, T obj, bool overwrite = true)
	{
		cacheService.Set(obj, DEFAULT_ID, overwrite);
	}

	public static T Get<T>(this ICacheService cacheService) where T : class
	{
		return cacheService.Get<T>(DEFAULT_ID);
	}
	public static void Unset<T>(this ICacheService cacheService)
	{
		cacheService.Unset<T>(DEFAULT_ID);
	}

	public static void ListAdd<T>(this ICacheService cacheService, T obj)
	{
		cacheService.ListAdd(obj, DEFAULT_ID);
	}

	public static List<T> ListGet<T>(this ICacheService cacheService)
	{
		return cacheService.ListGet<T>(DEFAULT_ID);
	}

	public static void ListRemove<T>(this ICacheService cacheService, T obj)
	{
		cacheService.ListRemove(obj, DEFAULT_ID);
	}
}
