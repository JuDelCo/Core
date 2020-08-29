using System;
using System.Collections.Generic;
using Ju.Input;

namespace Ju.Services
{
	public class InputAction : IInputAction
	{
		public event InputServiceActionEvent OnPressed = delegate { };
		public event InputServiceActionEvent OnHeld = delegate { };
		public event InputServiceActionEvent OnReleased = delegate { };

		public string Id { get; }
		public IInputPlayer Player { get; private set; }
		public bool Enabled { get; private set; }
		public IEnumerable<MouseButton> MouseButtonBindings => mouseButtons;
		public IEnumerable<KeyboardKey> KeyboardKeyBindings => keyboardKeys;
		public IEnumerable<GamepadButton> GamepadButtonBindings => gamepadButtons;
		public IEnumerable<GamepadAxis> GamepadAxisBindings => gamepadAxis;

		private readonly List<MouseButton> mouseButtons;
		private readonly List<KeyboardKey> keyboardKeys;
		private readonly List<GamepadButton> gamepadButtons;
		private readonly List<GamepadAxis> gamepadAxis;

		private bool pressed;
		private bool previousPressed;
		private float axisValue;
		private float axisRawValueX;
		private float axisRawValueY;

		public InputAction(IInputPlayer player, string id)
		{
			Player = player;
			Id = id;

			Enabled = true;

			mouseButtons = new List<MouseButton>();
			keyboardKeys = new List<KeyboardKey>();
			gamepadButtons = new List<GamepadButton>();
			gamepadAxis = new List<GamepadAxis>();

			ResetBindings();
		}

		public void SetEnabled(bool enabled)
		{
			Enabled = enabled;
		}

		public void ResetBindings()
		{
			mouseButtons.Clear();
			keyboardKeys.Clear();
			gamepadButtons.Clear();
			gamepadAxis.Clear();

			pressed = false;
			previousPressed = false;
			axisValue = 0f;
			axisRawValueX = 0f;
			axisRawValueY = 0f;
		}

		public void Update()
		{
			ResetInternal();

			if (Enabled)
			{
				pressed = GetButtonPressedInternal();
				axisValue = GetAxisValueInternal();
				GetAxisRawValueInternal(out axisRawValueX, out axisRawValueY);

				if (pressed && axisValue == 0f)
				{
					axisValue = 1f;
				}

				if (!pressed && (axisValue != 0f || axisRawValueX != 0f || axisRawValueY != 0f))
				{
					pressed = true;
				}
			}

			if (!previousPressed && pressed)
			{
				OnPressed(this);
			}

			if (previousPressed && pressed)
			{
				OnHeld(this);
			}

			if (previousPressed && !pressed)
			{
				OnReleased(this);
			}
		}

		private void ResetInternal()
		{
			axisValue = 0f;
			axisRawValueX = 0f;
			axisRawValueY = 0f;

			previousPressed = pressed;
			pressed = false;
		}

		private bool GetButtonPressedInternal()
		{
			var result = false;

			foreach (var controller in Player.Controllers)
			{
				if (controller is IMouseController mouse)
				{
					foreach (var button in mouseButtons)
					{
						if (mouse.IsButtonHeld(button))
						{
							result = true;
							break;
						}
					}
				}
				else if (controller is IKeyboardController keyboard)
				{
					foreach (var key in keyboardKeys)
					{
						if (keyboard.IsKeyHeld(key))
						{
							result = true;
							break;
						}
					}
				}
				else if (controller is IGamepadController gamepad)
				{
					foreach (var button in gamepadButtons)
					{
						if (gamepad.IsButtonHeld(button))
						{
							result = true;
							break;
						}
					}
				}

				if (result)
				{
					break;
				}
			}

			return result;
		}

		private float GetAxisValueInternal()
		{
			var result = 0f;

			if (Enabled)
			{
				foreach (var controller in Player.Controllers)
				{
					if (controller is IGamepadController gamepad)
					{
						foreach (var axis in gamepadAxis)
						{
							var axisValue = gamepad.GetAxis(axis);

							if (axisValue >= gamepad.Deadzone)
							{
								result = axisValue;
								break;
							}
						}
					}

					if (result != 0f)
					{
						break;
					}
				}
			}

			return result;
		}

		private void GetAxisRawValueInternal(out float axisX, out float axisY)
		{
			axisX = 0f;
			axisY = 0f;

			if (Enabled)
			{
				foreach (var controller in Player.Controllers)
				{
					if (controller is IGamepadController gamepad)
					{
						foreach (var axis in gamepadAxis)
						{
							gamepad.GetAxisRaw(axis, out float axisValueX, out float axisValueY);

							if (Math.Abs(axisValueX) >= gamepad.Deadzone)
							{
								axisX = axisValueX;
							}

							if (Math.Abs(axisValueY) >= gamepad.Deadzone)
							{
								axisY = axisValueY;
							}

							if (axisX != 0f || axisY != 0f)
							{
								break;
							}
						}
					}

					if (axisX != 0f || axisY != 0f)
					{
						break;
					}
				}
			}
		}

		public void AddBinding(MouseButton button)
		{
			if (!mouseButtons.Contains(button))
			{
				mouseButtons.Add(button);
			}
		}

		public void AddBinding(KeyboardKey key)
		{
			if (!keyboardKeys.Contains(key))
			{
				keyboardKeys.Add(key);
			}
		}

		public void AddBinding(GamepadButton button)
		{
			if (!gamepadButtons.Contains(button))
			{
				gamepadButtons.Add(button);
			}
		}

		public void RemoveBinding(MouseButton button)
		{
			mouseButtons.Remove(button);
		}

		public void RemoveBinding(KeyboardKey key)
		{
			keyboardKeys.Remove(key);
		}

		public void RemoveBinding(GamepadButton button)
		{
			gamepadButtons.Remove(button);
		}

		public bool IsPressed()
		{
			return pressed && !previousPressed;
		}

		public bool IsHeld()
		{
			return pressed && previousPressed;
		}

		public bool IsReleased()
		{
			return !pressed && previousPressed;
		}

		public void AddBinding(GamepadAxis axis)
		{
			if (!gamepadAxis.Contains(axis))
			{
				gamepadAxis.Add(axis);
			}
		}

		public void RemoveBinding(GamepadAxis axis)
		{
			gamepadAxis.Remove(axis);
		}

		public float GetAxisValue()
		{
			return axisValue;
		}

		public void GetAxisRawValue(out float axisX, out float axisY)
		{
			axisX = axisRawValueX;
			axisY = axisRawValueY;
		}
	}
}
