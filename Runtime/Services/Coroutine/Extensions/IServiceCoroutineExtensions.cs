// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System.Collections;
using Ju.Handlers;
using Ju.Services;
using Ju.Services.Internal;

public static class IServiceCoroutineExtensions
{
	public static Coroutine CoroutineStart(this IService service, IEnumerator routine)
	{
		return ServiceCache.Coroutine.StartCoroutine(new ObjectLinkHandler<IService>(service), routine);
	}
}
