// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

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
