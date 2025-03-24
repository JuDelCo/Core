// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using Ju.Handlers;
using UnityEngine;

namespace Ju.Time
{
	public partial class Clock<T> : IClock where T : ITimeDeltaEvent
	{
		public Clock(Behaviour behaviour, bool alwaysActive = false) : this()
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			this.updateCondition = () => linkHandler.IsActive;
		}

		public Clock(float elapsedSeconds, Behaviour behaviour, bool alwaysActive = false) : this(elapsedSeconds)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			this.updateCondition = () => linkHandler.IsActive;
		}
	}
}

#endif
