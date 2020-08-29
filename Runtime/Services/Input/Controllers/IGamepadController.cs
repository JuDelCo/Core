
namespace Ju.Input
{
	public interface IGamepadController : IController
	{
		bool Enabled { get; }
		float Deadzone { get; }

		void SetDeadzone(float deadzone);

		bool IsAnyButtonPressed();
		bool IsButtonPressed(GamepadButton button);
		bool IsButtonPressed(GamepadButtonRaw button);
		bool IsButtonHeld(GamepadButton button);
		bool IsButtonHeld(GamepadButtonRaw button);
		bool IsButtonReleased(GamepadButton button);
		bool IsButtonReleased(GamepadButtonRaw button);
		GamepadButton FirstPressedButton();

		bool IsAnyAxisPressed();
		float GetAxis(GamepadAxis axis);
		float GetAxis(GamepadAxisRaw axis);
		void GetAxisRaw(GamepadAxis axis, out float axisX, out float axisY);
		GamepadAxis FirstPressedAxis();
	}
}
