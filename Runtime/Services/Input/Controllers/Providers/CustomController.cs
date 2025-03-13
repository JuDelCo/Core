// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Input
{
	public abstract class CustomController : IGamepadController
	{
		public abstract string Id { get; }
		public virtual bool Enabled { get => true; set { } }
		public virtual float Deadzone { get => 0f; set { } }

		public virtual void SetDeadzone(float deadzone) { }

		public virtual bool IsAnyButtonPressed() => false;
		public virtual bool IsButtonPressed(GamepadButton button) => false;
		public virtual bool IsButtonPressed(GamepadButtonRaw button) => false;
		public virtual bool IsButtonHeld(GamepadButton button) => false;
		public virtual bool IsButtonHeld(GamepadButtonRaw button) => false;
		public virtual bool IsButtonReleased(GamepadButton button) => false;
		public virtual bool IsButtonReleased(GamepadButtonRaw button) => false;
		public virtual GamepadButton FirstPressedButton() => GamepadButton.None;

		public virtual bool IsAnyAxisPressed() => false;
		public virtual float GetAxis(GamepadAxis axis) => 0f;
		public virtual float GetAxis(GamepadAxisRaw axis) => 0f;
		public virtual void GetAxisRaw(GamepadAxis axis, out float axisX, out float axisY) { axisX = 0f; axisY = 0f; }
		public virtual GamepadAxis FirstPressedAxis() => GamepadAxis.None;
	}
}
