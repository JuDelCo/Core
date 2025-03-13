// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Input
{
	public struct InputGamepadDisconnectedEvent
	{
		public IGamepadController Controller { get; private set; }

		public InputGamepadDisconnectedEvent(IGamepadController controller)
		{
			Controller = controller;
		}
	}
}
