// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Godot;
using Ju.Handlers;
using Ju.Observables;

public static class ObservableGodotExtensions
{
	public static void Subscribe<T>(this Observable<T> observable, Node node, Action<T> action, bool alwaysActive = false)
	{
		observable.Subscribe(new NodeLinkHandler(node, alwaysActive), action);
	}
}

#endif
