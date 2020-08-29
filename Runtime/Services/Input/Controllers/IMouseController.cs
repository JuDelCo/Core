
namespace Ju.Input
{
	public interface IMouseController : IController
	{
		bool IsAnyButtonPressed();
		bool IsButtonPressed(MouseButton button);
		bool IsButtonHeld(MouseButton button);
		bool IsButtonReleased(MouseButton button);
		MouseButton FirstPressedButton();

		void GetPosition(out int mouseX, out int mouseY);
		void GetPositionDelta(out int mouseX, out int mouseY);

		float GetWheelDelta();

		void SetLockMode(MouseLockMode mode);
		void SetCursorVisible(bool visible);
	}
}
