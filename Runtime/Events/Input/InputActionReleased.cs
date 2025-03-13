// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Input
{
	public struct InputActionReleased
	{
		public IInputAction Action { get; private set; }

		public InputActionReleased(IInputAction action)
		{
			Action = action;
		}
	}
}
