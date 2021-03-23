// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Input
{
	public struct InputActionHeldEvent
	{
		public IInputAction Action { get; private set; }

		public InputActionHeldEvent(IInputAction action)
		{
			Action = action;
		}
	}
}
