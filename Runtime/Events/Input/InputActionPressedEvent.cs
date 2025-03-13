// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Input
{
	public struct InputActionPressedEvent
	{
		public IInputAction Action { get; private set; }

		public InputActionPressedEvent(IInputAction action)
		{
			Action = action;
		}
	}
}
