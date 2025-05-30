// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Services;

namespace Ju.Input
{
	public class MouseController : IMouseController
	{
		public string Id => "mouse";
		public MouseLockMode LockMode { get => input.GetMouseCurrentLockMode(); set => input.SetMouseLockMode(value); }
		public bool Visible { get => input.GetMouseVisibleStatus(); set => input.SetMouseCursorVisible(value); }

		private static readonly MouseButton[] mouseButtonEnum = (MouseButton[]) Enum.GetValues(typeof(MouseButton));

		private readonly IInputServiceRaw input;

		public MouseController(IInputServiceRaw input)
		{
			this.input = input;
		}

		public bool IsAnyButtonPressed()
		{
			foreach (var button in mouseButtonEnum)
			{
				if (button == MouseButton.None)
				{
					continue;
				}

				if (input.GetMouseHeldRaw(button))
				{
					return true;
				}
			}

			return false;
		}

		public bool IsButtonPressed(MouseButton button)
		{
			return input.GetMousePressedRaw(button);
		}

		public bool IsButtonHeld(MouseButton button)
		{
			return input.GetMouseHeldRaw(button);
		}

		public bool IsButtonReleased(MouseButton button)
		{
			return input.GetMouseReleasedRaw(button);
		}

		public MouseButton FirstPressedButton()
		{
			foreach (var button in mouseButtonEnum)
			{
				if (button == MouseButton.None)
				{
					continue;
				}

				if (input.GetMousePressedRaw(button))
				{
					return button;
				}
			}

			return MouseButton.None;
		}

		public void GetPosition(out int mouseX, out int mouseY)
		{
			input.GetMousePosition(out mouseX, out mouseY);
		}

		public void GetPositionDelta(out float mouseX, out float mouseY)
		{
			input.GetMousePositionDelta(out mouseX, out mouseY);
		}

		public float GetWheelDelta()
		{
			return input.GetMouseWheelDelta();
		}
	}
}
