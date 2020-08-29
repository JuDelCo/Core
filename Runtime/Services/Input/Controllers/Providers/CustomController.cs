
namespace Ju.Input
{
	public abstract class CustomController : IGamepadController
	{
		public abstract string Id { get; }
		public abstract bool Enabled { get; }
		public abstract float Deadzone { get; }

		public abstract void SetDeadzone(float deadzone);

		public abstract bool IsAnyButtonPressed();
		public abstract bool IsButtonPressed(GamepadButton button);
		public abstract bool IsButtonPressed(GamepadButtonRaw button);
		public abstract bool IsButtonHeld(GamepadButton button);
		public abstract bool IsButtonHeld(GamepadButtonRaw button);
		public abstract bool IsButtonReleased(GamepadButton button);
		public abstract bool IsButtonReleased(GamepadButtonRaw button);
		public abstract GamepadButton FirstPressedButton();

		public abstract bool IsAnyAxisPressed();
		public abstract float GetAxis(GamepadAxis axis);
		public abstract float GetAxis(GamepadAxisRaw axis);
		public abstract void GetAxisRaw(GamepadAxis axis, out float axisX, out float axisY);
		public abstract GamepadAxis FirstPressedAxis();
	}
}
