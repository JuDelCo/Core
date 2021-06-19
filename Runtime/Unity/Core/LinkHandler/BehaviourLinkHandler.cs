// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using UnityEngine;

namespace Ju.Handlers
{
	public struct BehaviourLinkHandler : ILinkHandler
	{
		private readonly WeakReference behaviourRef;
		private readonly bool alwaysActive;

		public BehaviourLinkHandler(Behaviour behaviour, bool alwaysActive = false)
		{
			this.behaviourRef = new WeakReference(behaviour);
			this.alwaysActive = alwaysActive;
		}

		public bool IsActive => !IsDestroyed && (alwaysActive || ((Behaviour)behaviourRef.Target).isActiveAndEnabled);
		public bool IsDestroyed => !behaviourRef.IsAlive || ((Behaviour)behaviourRef.Target) == null || (!alwaysActive && !((Behaviour)behaviourRef.Target).isActiveAndEnabled);
	}
}

#endif
