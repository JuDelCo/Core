// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;

namespace Ju.FSM
{
	public struct StateLinkHandler : ILinkHandler
	{
		private readonly WeakReference stateRef;

		public StateLinkHandler(State state)
		{
			this.stateRef = new WeakReference(state);
		}

		public bool IsActive => !IsDestroyed && ((State)stateRef.Target).IsCurrent();
		public bool IsDestroyed => !stateRef.IsAlive;
	}
}
