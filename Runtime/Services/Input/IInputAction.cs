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
		bool Enabled { get; }
		IEnumerable<MouseButton> MouseButtonBindings { get; }
		IEnumerable<KeyboardKey> KeyboardKeyBindings { get; }
		IEnumerable<GamepadButton> GamepadButtonBindings { get; }
		IEnumerable<GamepadAxis> GamepadAxisBindings { get; }

		void SetEnabled(bool enabled);
		void ResetBindings();

		void AddBinding(MouseButton button);
		void AddBinding(KeyboardKey key);
		void AddBinding(GamepadButton button);
		void RemoveBinding(MouseButton button);
		void RemoveBinding(KeyboardKey key);
		void RemoveBinding(GamepadButton button);
		bool IsPressed();
		bool IsHeld();
		bool IsReleased();

		void AddBinding(GamepadAxis axis);
		void RemoveBinding(GamepadAxis axis);
		float GetAxisValue();
		void GetAxisRawValue(out float axisX, out float axisY);
	}
}
