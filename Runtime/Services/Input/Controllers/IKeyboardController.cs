
namespace Ju.Input
{
	public interface IKeyboardController : IController
	{
		bool IsAnyKeyPressed();
		bool IsKeyPressed(KeyboardKey key);
		bool IsKeyHeld(KeyboardKey key);
		bool IsKeyReleased(KeyboardKey key);
		KeyboardKey FirstPressedKey();

		string GetInputString();
	}
}
