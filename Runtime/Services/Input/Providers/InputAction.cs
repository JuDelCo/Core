// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Input;
using Ju.Extensions;
using Ju.Services.Internal;

namespace Ju.Services
{
	public class InputAction : IInputAction
	{
		public string Id { get; }
		public IInputPlayer Player { get; private set; }
		public bool Enabled { get; set; }
		public IEnumerable<MouseButton> MouseButtonBindings => mouseButtons.Concatenate(mouseButtonsNegative);
		public IEnumerable<KeyboardKey> KeyboardKeyBindings => keyboardKeys.Concatenate(keyboardKeysNegative);
		public IEnumerable<GamepadButton> GamepadButtonBindings => gamepadButtons.Concatenate(gamepadButtonsNegative);
		public IEnumerable<GamepadAxis> GamepadAxisBindings => gamepadAxis;

		private readonly List<MouseButton> mouseButtons;
		private readonly List<MouseButton> mouseButtonsNegative;
		private readonly List<KeyboardKey> keyboardKeys;
		private readonly List<KeyboardKey> keyboardKeysNegative;
		private readonly List<GamepadButton> gamepadButtons;
		private readonly List<GamepadButton> gamepadButtonsNegative;
		private readonly List<GamepadAxis> gamepadAxis;

		private bool pressed;
		private float pressedDuration;
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
			mouseButtonsNegative = new List<MouseButton>();
			keyboardKeys = new List<KeyboardKey>();
			keyboardKeysNegative = new List<KeyboardKey>();
			gamepadButtons = new List<GamepadButton>();
			gamepadButtonsNegative = new List<GamepadButton>();
			gamepadAxis = new List<GamepadAxis>();

			ResetBindings();
		}

		public void ResetBindings()
		{
			mouseButtons.Clear();
			mouseButtonsNegative.Clear();
			keyboardKeys.Clear();
			keyboardKeysNegative.Clear();
			gamepadButtons.Clear();
			gamepadButtonsNegative.Clear();
			gamepadAxis.Clear();

			pressed = false;
			pressedDuration = 0f;
			previousPressed = false;
			axisValue = 0f;
			axisRawValueX = 0f;
			axisRawValueY = 0f;
		}

		public void Update(float deltaTime)
		{
			// Reset
			{
				axisValue = 0f;
				axisRawValueX = 0f;
				axisRawValueY = 0f;

				previousPressed = pressed;
				pressed = false;
			}

			if (Enabled)
			{
				var buttonAxis = GetButtonPressedInternal();
				pressed = (buttonAxis != 0);
				axisValue = GetAxisValueInternal();
				GetAxisRawValueInternal(out axisRawValueX, out axisRawValueY);

				if (pressed && axisValue == 0f)
				{
					axisValue = buttonAxis;
				}

				if (!pressed && (axisValue != 0f || axisRawValueX != 0f || axisRawValueY != 0f))
				{
					pressed = true;
				}

				if (pressed && previousPressed)
				{
					pressedDuration += deltaTime;
				}
				else
				{
					pressedDuration = 0f;
				}
			}

			if (!previousPressed && pressed)
			{
				ServiceCache.EventBus.Fire(new InputActionPressedEvent(this));
			}

			if (previousPressed && pressed)
			{
				ServiceCache.EventBus.Fire(new InputActionHeldEvent(this));
			}

			if (previousPressed && !pressed)
			{
				ServiceCache.EventBus.Fire(new InputActionReleased(this));
			}
		}

		private float GetButtonPressedInternal()
		{
			var positive = 0f;
			var negative = 0f;

			var controllers = (List<IController>)Player.Controllers;

			foreach (var controller in controllers)
			{
				if (controller is IMouseController mouse)
				{
					foreach (var button in mouseButtons)
					{
						if (mouse.IsButtonHeld(button))
						{
							positive = 1f;
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
							positive = 1f;
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
							positive = 1f;
							break;
						}
					}
				}

				if (positive == 1f)
				{
					break;
				}
			}

			foreach (var controller in controllers)
			{
				if (controller is IMouseController mouse)
				{
					foreach (var button in mouseButtonsNegative)
					{
						if (mouse.IsButtonHeld(button))
						{
							negative = -1f;
							break;
						}
					}
				}
				else if (controller is IKeyboardController keyboard)
				{
					foreach (var key in keyboardKeysNegative)
					{
						if (keyboard.IsKeyHeld(key))
						{
							negative = -1f;
							break;
						}
					}
				}
				else if (controller is IGamepadController gamepad)
				{
					foreach (var button in gamepadButtonsNegative)
					{
						if (gamepad.IsButtonHeld(button))
						{
							negative = -1f;
							break;
						}
					}
				}

				if (negative == -1f)
				{
					break;
				}
			}

			return positive + negative;
		}

		private float GetAxisValueInternal()
		{
			var result = 0f;

			if (Enabled)
			{
				var controllers = (List<IController>)Player.Controllers;

				foreach (var controller in controllers)
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
				var controllers = (List<IController>)Player.Controllers;

				foreach (var controller in controllers)
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

		public IInputAction AddBinding(MouseButton button)
		{
			if (!mouseButtons.Contains(button))
			{
				mouseButtons.Add(button);
			}

			return this;
		}

		public IInputAction AddBinding(MouseButton positiveButton, MouseButton negativeButton)
		{
			AddBinding(positiveButton);

			if (!mouseButtonsNegative.Contains(negativeButton))
			{
				mouseButtonsNegative.Add(negativeButton);
			}

			return this;
		}

		public IInputAction AddBinding(KeyboardKey key)
		{
			if (!keyboardKeys.Contains(key))
			{
				keyboardKeys.Add(key);
			}

			return this;
		}

		public IInputAction AddBinding(KeyboardKey positiveKey, KeyboardKey negativeKey)
		{
			AddBinding(positiveKey);

			if (!keyboardKeysNegative.Contains(negativeKey))
			{
				keyboardKeysNegative.Add(negativeKey);
			}

			return this;
		}

		public IInputAction AddBinding(GamepadButton button)
		{
			if (!gamepadButtons.Contains(button))
			{
				gamepadButtons.Add(button);
			}

			return this;
		}

		public IInputAction AddBinding(GamepadButton positiveButton, GamepadButton negativeButton)
		{
			AddBinding(positiveButton);

			if (!gamepadButtonsNegative.Contains(negativeButton))
			{
				gamepadButtonsNegative.Add(negativeButton);
			}

			return this;
		}

		public void RemoveBinding(MouseButton button)
		{
			mouseButtons.Remove(button);
			mouseButtonsNegative.Remove(button);
		}

		public void RemoveBinding(KeyboardKey key)
		{
			keyboardKeys.Remove(key);
			keyboardKeysNegative.Remove(key);
		}

		public void RemoveBinding(GamepadButton button)
		{
			gamepadButtons.Remove(button);
			gamepadButtonsNegative.Remove(button);
		}

		public bool IsPressed()
		{
			return pressed && !previousPressed;
		}

		public bool IsHeld()
		{
			return pressed;
		}

		public float HeldDuration()
		{
			return pressedDuration;
		}

		public bool IsReleased()
		{
			return !pressed && previousPressed;
		}

		public IInputAction AddBinding(GamepadAxis axis)
		{
			if (!gamepadAxis.Contains(axis))
			{
				gamepadAxis.Add(axis);
			}

			return this;
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
