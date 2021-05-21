// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using Ju.Handlers;
using Ju.Promises;
using Ju.Services.Internal;
using Ju.Time;
using UnityEngine;

namespace Ju.Extensions
{
	public static class BehaviourTaskExtensions
	{
		public static IPromise WaitUntil(this Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitUntil(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitWhile(this Behaviour behaviour, Func<bool> condition, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitWhile(new BehaviourLinkHandler(behaviour, alwaysActive), condition);
		}

		public static IPromise WaitForSeconds<T>(this Behaviour behaviour, float seconds, bool alwaysActive = false) where T : ITimeDeltaEvent
		{
			return ServiceCache.Task.WaitForSeconds<T>(new BehaviourLinkHandler(behaviour, alwaysActive), seconds);
		}

		public static IPromise WaitForSeconds(this Behaviour behaviour, float seconds, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitForSeconds<TimeUpdateEvent>(new BehaviourLinkHandler(behaviour, alwaysActive), seconds);
		}

		public static IPromise WaitForTicks<T>(this Behaviour behaviour, int ticks, bool alwaysActive = false) where T : ITimeEvent
		{
			return ServiceCache.Task.WaitForTicks<T>(new BehaviourLinkHandler(behaviour, alwaysActive), ticks);
		}

		public static IPromise WaitForNextUpdate(this Behaviour behaviour, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitForNextUpdate(new BehaviourLinkHandler(behaviour, alwaysActive));
		}

		public static IPromise WaitForNextFixedUpdate(this Behaviour behaviour, bool alwaysActive = false)
		{
			return ServiceCache.Task.WaitForNextFixedUpdate(new BehaviourLinkHandler(behaviour, alwaysActive));
		}
	}
}

#endif
