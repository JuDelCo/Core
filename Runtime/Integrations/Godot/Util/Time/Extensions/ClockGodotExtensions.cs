// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Ju.Handlers;
using Godot;

namespace Ju.Time
{
	public partial class Clock<T> : IClock where T : ITimeDeltaEvent
	{
		public Clock(Node node, bool alwaysActive = false) : this()
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			this.updateCondition = () => linkHandler.IsActive;
		}

		public Clock(float elapsedSeconds, Node node, bool alwaysActive = false) : this(elapsedSeconds)
		{
			var linkHandler = new NodeLinkHandler(node, alwaysActive);
			this.updateCondition = () => linkHandler.IsActive;
		}
	}
}

#endif
