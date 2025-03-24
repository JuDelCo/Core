// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System.Collections;
using Ju.Handlers;
using Ju.Services.Internal;
using Godot;

namespace Ju.Extensions
{
	public static class NodeCoroutineExtensions
	{
		public static Services.Coroutine CoroutineStart(this Node node, IEnumerator routine, bool alwaysActive = true)
		{
			return ServiceCache.Coroutine.StartCoroutine(new NodeLinkHandler(node, alwaysActive), routine);
		}
	}
}

#endif
