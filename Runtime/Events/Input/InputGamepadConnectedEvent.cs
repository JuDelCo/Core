// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Input
{
	public struct InputGamepadConnectedEvent
	{
		public IGamepadController Controller { get; private set; }

		public InputGamepadConnectedEvent(IGamepadController controller)
		{
			Controller = controller;
		}
	}
}
