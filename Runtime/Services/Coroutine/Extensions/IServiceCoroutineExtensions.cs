// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections;
using Ju.Handlers;
using Ju.Services.Internal;

namespace Ju.Services.Extensions
{
	public static class IServiceCoroutineExtensions
	{
		public static Coroutine CoroutineStart(this IService service, IEnumerator routine)
		{
			return ServiceCache.Coroutine.StartCoroutine(new ObjectLinkHandler<IService>(service), routine);
		}
	}
}
