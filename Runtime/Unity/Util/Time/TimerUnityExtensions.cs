// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using Ju.Handlers;
using UnityEngine;

namespace Ju.Time
{
	public partial class Timer<T> : ITimer where T : ITimeDeltaEvent
	{
		public Timer(float seconds, Action onCompleted, Behaviour behaviour, bool alwaysActive = false) : this(seconds, onCompleted)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			this.updateCondition = () => linkHandler.IsActive;
		}
	}
}

#endif
