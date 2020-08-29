using System;
using Ju.Extensions;
using Ju.Input;

namespace Ju.Services
{
	public class MouseController : IMouseController
	{
		public string Id => "mouse";

		private readonly IInputServiceRaw input;

		public MouseController(IInputServiceRaw input)
		{
			this.input = input;
		}

		public bool IsAnyButtonPressed()
		{
			var buttons = Enum.GetValues(typeof(MouseButton)).Cast<MouseButton>();

			foreach (var button in buttons)
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
			var buttons = Enum.GetValues(typeof(MouseButton)).Cast<MouseButton>();

			foreach (var button in buttons)
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

		public void GetPositionDelta(out int mouseX, out int mouseY)
		{
			input.GetMousePositionDelta(out mouseX, out mouseY);
		}

		public float GetWheelDelta()
		{
			return input.GetMouseWheelDelta();
		}

		public void SetLockMode(MouseLockMode mouseLockMode)
		{
			input.SetMouseLockMode(mouseLockMode);
		}

		public void SetCursorVisible(bool visible)
		{
			input.SetMouseCursorVisible(visible);
		}
	}
}
