// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System.Collections;
using Ju.Handlers;
using Ju.Services.Internal;
using UnityEngine;

namespace Ju.Extensions
{
	public static class BehaviourCoroutineExtensions
	{
		public static Services.Coroutine CoroutineStart(this Behaviour behaviour, IEnumerator routine, bool alwaysActive = true)
		{
			return ServiceCache.Coroutine.StartCoroutine(new BehaviourLinkHandler(behaviour, alwaysActive), routine);
		}
	}
}

#endif
