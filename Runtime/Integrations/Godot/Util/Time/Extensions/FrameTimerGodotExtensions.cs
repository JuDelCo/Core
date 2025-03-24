// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Ju.Handlers;
using Godot;

namespace Ju.Time
{
	public partial class FrameTimer<T> : IFrameTimer where T : ITimeEvent
	{
		public FrameTimer(int frames, Action onCompleted, Node node, bool alwaysActive = false) : this(frames, onCompleted)
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			this.updateCondition = () => linkHandler.IsActive;
		}
	}
}

#endif
