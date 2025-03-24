// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Godot;

namespace Ju.Handlers
{
	public struct NodeLinkHandler : ILinkHandler
	{
		private readonly WeakReference nodeRef;
		private readonly bool alwaysActive;

		public NodeLinkHandler(Node node, bool alwaysActive = false)
		{
			this.nodeRef = new WeakReference(node);
			this.alwaysActive = alwaysActive;
		}

		public bool IsActive => !IsDestroyed && (alwaysActive || (((Node) nodeRef.Target).IsInsideTree() && ((Node) nodeRef.Target).CanProcess()));
		public bool IsDestroyed => !nodeRef.IsAlive || !GodotObject.IsInstanceValid((Node) nodeRef.Target) || ((Node) nodeRef.Target) == null;
	}
}

#endif
