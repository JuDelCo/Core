// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using System.Collections.Specialized;
using Godot;
using Ju.Handlers;

namespace Ju.Hjson
{
	public static class JsonArrayGodotExtensions
	{
		public static void Subscribe(this JsonArray array, Node node, Action<NotifyCollectionChangedEventArgs> action, bool alwaysActive = false)
		{
			array.Subscribe(new NodeLinkHandler(node, alwaysActive), action);
		}
	}
}

#endif
