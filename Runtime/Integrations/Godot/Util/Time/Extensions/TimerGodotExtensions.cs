// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Ju.Handlers;
using Godot;

namespace Ju.Time
{
	public partial class Timer<T> : ITimer where T : ITimeDeltaEvent
	{
		public Timer(float seconds, Action onCompleted, Node node, bool alwaysActive = false) : this(seconds, onCompleted)
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			this.updateCondition = () => linkHandler.IsActive;
		}
	}
}

#endif
