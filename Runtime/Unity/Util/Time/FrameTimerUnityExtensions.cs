// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using Ju.Handlers;
using UnityEngine;

namespace Ju.Time
{
	public partial class FrameTimer<T> : IFrameTimer where T : ITimeEvent
	{
		public FrameTimer(int frames, Action onCompleted, Behaviour behaviour, bool alwaysActive = false) : this(frames, onCompleted)
		{
			var linkHandler = new BehaviourLinkHandler(behaviour, alwaysActive);
			this.updateCondition = () => linkHandler.IsActive;
		}
	}
}

#endif
