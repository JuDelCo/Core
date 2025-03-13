// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using Ju.Input;

namespace Ju.Services
{
	public abstract class IInputServiceRaw
	{
		public abstract void Initialize();
		public abstract void Update();

		public abstract bool GetMousePressedRaw(MouseButton button);
		public abstract bool GetMouseHeldRaw(MouseButton button);
		public abstract bool GetMouseReleasedRaw(MouseButton button);
		public abstract void GetMousePosition(out int mouseX, out int mouseY);
		public abstract void GetMousePositionDelta(out float mouseX, out float mouseY);
		public abstract float GetMouseWheelDelta();
		public abstract MouseLockMode GetMouseCurrentLockMode();
		public abstract bool GetMouseVisibleStatus();
		public abstract void SetMouseLockMode(MouseLockMode mouseLockMode);
		public abstract void SetMouseCursorVisible(bool visible);

		public abstract bool GetKeyboardPressedRaw(KeyboardKey key);
		public abstract bool GetKeyboardHeldRaw(KeyboardKey key);
		public abstract bool GetKeyboardReleasedRaw(KeyboardKey key);
		public abstract string GetKeyboardInputStringRaw();

		public abstract bool GetGamepadButtonPressedRaw(IGamepadController gamepad, GamepadButton button);
		public abstract bool GetGamepadButtonPressedRaw(IGamepadController gamepad, GamepadButtonRaw button);
		public abstract bool GetGamepadButtonHeldRaw(IGamepadController gamepad, GamepadButton button);
		public abstract bool GetGamepadButtonHeldRaw(IGamepadController gamepad, GamepadButtonRaw button);
		public abstract bool GetGamepadButtonReleasedRaw(IGamepadController gamepad, GamepadButton button);
		public abstract bool GetGamepadButtonReleasedRaw(IGamepadController gamepad, GamepadButtonRaw button);
		public abstract float GetGamepadAxisRaw(IGamepadController gamepad, GamepadAxis axis);
		public abstract float GetGamepadAxisRaw(IGamepadController gamepad, GamepadAxisRaw axis);
		public abstract void GetGamepadAxisRawRaw(IGamepadController gamepad, GamepadAxis axis, out float axisX, out float axisY);
	}
}
