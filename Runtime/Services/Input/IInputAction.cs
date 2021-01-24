// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;

namespace Ju.Input
{
	public delegate void InputServiceActionEvent(IInputAction action);

	public interface IInputAction
	{
		event InputServiceActionEvent OnPressed;
		event InputServiceActionEvent OnHeld;
		event InputServiceActionEvent OnReleased;

		string Id { get; }
		IInputPlayer Player { get; }
		bool Enabled { get; set; }
		IEnumerable<MouseButton> MouseButtonBindings { get; }
		IEnumerable<KeyboardKey> KeyboardKeyBindings { get; }
		IEnumerable<GamepadButton> GamepadButtonBindings { get; }
		IEnumerable<GamepadAxis> GamepadAxisBindings { get; }

		void ResetBindings();

		IInputAction AddBinding(MouseButton button);
		IInputAction AddBinding(MouseButton positiveButton, MouseButton negativeButton);
		IInputAction AddBinding(KeyboardKey key);
		IInputAction AddBinding(KeyboardKey positiveKey, KeyboardKey negativeKey);
		IInputAction AddBinding(GamepadButton button);
		IInputAction AddBinding(GamepadButton positiveButton, GamepadButton negativeButton);
		void RemoveBinding(MouseButton button);
		void RemoveBinding(KeyboardKey key);
		void RemoveBinding(GamepadButton button);
		bool IsPressed();
		bool IsHeld();
		float HeldDuration();
		bool IsReleased();

		IInputAction AddBinding(GamepadAxis axis);
		void RemoveBinding(GamepadAxis axis);
		float GetAxisValue();
		void GetAxisRawValue(out float axisX, out float axisY);
	}
}
