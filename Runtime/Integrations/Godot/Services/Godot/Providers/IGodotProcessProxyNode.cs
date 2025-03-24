// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System;
using Godot;

namespace Ju.Services
{
	public interface IGodotProcessProxyNode
	{
		event Action<int> OnNotificationEvent;
		event Action<InputEvent> OnInputEvent;

		double GetProcessDeltaTime();
		double GetPhysicsProcessDeltaTime();
		void ProxyCallDeferred(Action action);
	}
}

#endif
