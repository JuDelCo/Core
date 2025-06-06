// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Services;

namespace Ju.Input
{
	public class GamepadController : IGamepadController
	{
		public string Id { get; }
		public bool Enabled { get; set; }
		public float Deadzone { get; set; }

		private static readonly GamepadButton[] gamepadButtonEnum = (GamepadButton[]) Enum.GetValues(typeof(GamepadButton));
		private static readonly GamepadButtonRaw[] gamepadButtonRawEnum = (GamepadButtonRaw[]) Enum.GetValues(typeof(GamepadButtonRaw));
		private static readonly GamepadAxis[] gamepadAxisEnum = (GamepadAxis[]) Enum.GetValues(typeof(GamepadAxis));
		private static readonly GamepadAxisRaw[] gamepadAxisRawEnum = (GamepadAxisRaw[]) Enum.GetValues(typeof(GamepadAxisRaw));

		private readonly IInputServiceRaw input;

		public GamepadController(string id, IInputServiceRaw input)
		{
			Id = id;
			Enabled = true;
			this.input = input;
			Deadzone = 0.2f;
		}

		public bool IsAnyButtonPressed()
		{
			if (!Enabled)
			{
				return false;
			}

			foreach (var button in gamepadButtonRawEnum)
			{
				if (button == GamepadButtonRaw.None)
				{
					continue;
				}

				if (input.GetGamepadButtonHeldRaw(this, button))
				{
					return true;
				}
			}

			return false;
		}

		public bool IsButtonPressed(GamepadButton button)
		{
			if (!Enabled)
			{
				return false;
			}

			return input.GetGamepadButtonPressedRaw(this, button);
		}

		public bool IsButtonPressed(GamepadButtonRaw button)
		{
			if (!Enabled)
			{
				return false;
			}

			return input.GetGamepadButtonPressedRaw(this, button);
		}

		public bool IsButtonHeld(GamepadButton button)
		{
			if (!Enabled)
			{
				return false;
			}

			return input.GetGamepadButtonHeldRaw(this, button);
		}

		public bool IsButtonHeld(GamepadButtonRaw button)
		{
			if (!Enabled)
			{
				return false;
			}

			return input.GetGamepadButtonHeldRaw(this, button);
		}

		public bool IsButtonReleased(GamepadButton button)
		{
			if (!Enabled)
			{
				return false;
			}

			return input.GetGamepadButtonReleasedRaw(this, button);
		}

		public bool IsButtonReleased(GamepadButtonRaw button)
		{
			if (!Enabled)
			{
				return false;
			}

			return input.GetGamepadButtonReleasedRaw(this, button);
		}

		public GamepadButton FirstPressedButton()
		{
			if (!Enabled)
			{
				return GamepadButton.None;
			}

			foreach (var button in gamepadButtonEnum)
			{
				if (button == GamepadButton.None)
				{
					continue;
				}

				if (input.GetGamepadButtonPressedRaw(this, button))
				{
					return button;
				}
			}

			return GamepadButton.None;
		}

		public bool IsAnyAxisPressed()
		{
			if (!Enabled)
			{
				return false;
			}

			foreach (var axis in gamepadAxisRawEnum)
			{
				if (axis == GamepadAxisRaw.None)
				{
					continue;
				}

				if (System.Math.Abs(input.GetGamepadAxisRaw(this, axis)) - Deadzone > 0f)
				{
					return true;
				}
			}

			return false;
		}

		public float GetAxis(GamepadAxis axis)
		{
			if (!Enabled || axis == GamepadAxis.D_Pad || axis == GamepadAxis.L_Stick || axis == GamepadAxis.R_Stick)
			{
				return 0f;
			}

			return input.GetGamepadAxisRaw(this, axis);
		}

		public float GetAxis(GamepadAxisRaw axis)
		{
			if (!Enabled)
			{
				return 0f;
			}

			return input.GetGamepadAxisRaw(this, axis);
		}

		public void GetAxisRaw(GamepadAxis axis, out float axisX, out float axisY)
		{
			if (!Enabled || !(axis == GamepadAxis.D_Pad || axis == GamepadAxis.L_Stick || axis == GamepadAxis.R_Stick))
			{
				axisX = 0f;
				axisY = 0f;

				return;
			}

			input.GetGamepadAxisRawRaw(this, axis, out axisX, out axisY);
		}

		public GamepadAxis FirstPressedAxis()
		{
			if (!Enabled)
			{
				return GamepadAxis.None;
			}

			foreach (var axis in gamepadAxisEnum)
			{
				if (axis == GamepadAxis.None || axis == GamepadAxis.D_Pad || axis == GamepadAxis.L_Stick || axis == GamepadAxis.R_Stick)
				{
					continue;
				}

				if (input.GetGamepadAxisRaw(this, axis) - Deadzone > 0f)
				{
					return axis;
				}
			}

			return GamepadAxis.None;
		}
	}
}
