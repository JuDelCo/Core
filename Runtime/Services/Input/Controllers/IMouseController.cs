// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Input
{
	public interface IMouseController : IController
	{
		MouseLockMode LockMode { get; set; }
		bool Visible { get; set; }

		bool IsAnyButtonPressed();
		bool IsButtonPressed(MouseButton button);
		bool IsButtonHeld(MouseButton button);
		bool IsButtonReleased(MouseButton button);
		MouseButton FirstPressedButton();

		void GetPosition(out int mouseX, out int mouseY);
		void GetPositionDelta(out float mouseX, out float mouseY);

		float GetWheelDelta();
	}
}
