// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Ju.Handlers;
using Ju.Promises;
using Ju.Services.Internal;
using Ju.Time;
using Godot;

namespace Ju.Extensions
{
	public static class NodeTaskExtensions
	{
		public static IPromise WaitUntil(this Node node, Func<bool> condition, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitUntil(new NodeLinkHandler(node, alwaysActive), condition);
		}

		public static IPromise WaitWhile(this Node node, Func<bool> condition, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitWhile(new NodeLinkHandler(node, alwaysActive), condition);
		}

		public static IPromise WaitForSeconds<T>(this Node node, float seconds, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			return ServiceCache.Task.WaitForSeconds<T>(new NodeLinkHandler(node, alwaysActive), seconds);
		}

		public static IPromise WaitForSeconds(this Node node, float seconds, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitForSeconds<TimeUpdateEvent>(new NodeLinkHandler(node, alwaysActive), seconds);
		}

		public static IPromise WaitForTicks<T>(this Node node, int ticks, bool alwaysActive = false) where T : ITimeEvent
		{
			return ServiceCache.Task.WaitForTicks<T>(new NodeLinkHandler(node, alwaysActive), ticks);
		}

		public static IPromise WaitForNextUpdate(this Node node, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitForNextUpdate(new NodeLinkHandler(node, alwaysActive));
		}

		public static IPromise WaitForNextFixedUpdate(this Node node, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitForNextFixedUpdate(new NodeLinkHandler(node, alwaysActive));
		}
	}
}

#endif
