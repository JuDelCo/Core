// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER
#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM

using Ju.Extensions;
using Ju.Input;
using Ju.Time;
using UnityEngine;

namespace Ju.Services
{
	using Ju.Log;

	public class InputUnityService : InputService
	{
		private ClockPrecise interval;
		private readonly float UPDATE_INTERVAL = 2f;

		private Vector2Int mousePosition;

		public override void Initialize()
		{
			interval = new ClockPrecise();

			var joystickNames = UnityEngine.Input.GetJoystickNames();

			foreach (var gamepadId in joystickNames)
			{
				ConnectGamepad(gamepadId);
			}
		}

		public override void Update()
		{
			UpdateMousePosition();

			if (interval.GetElapsedTime().seconds < UPDATE_INTERVAL)
			{
				return;
			}

			interval.Reset();

			var joystickNames = UnityEngine.Input.GetJoystickNames();

			foreach (var gamepadId in joystickNames)
			{
				ConnectGamepad(gamepadId);
			}

			foreach (var gamepad in gamepads)
			{
				if (!joystickNames.Contains(gamepad.Id))
				{
					DisconnectGamepad(gamepad.Id);
				}
			}
		}

		private void UpdateMousePosition()
		{
			var position = UnityEngine.Input.mousePosition;
			mousePosition = new Vector2Int((int)position.x, (int)position.y);
		}

		public override bool GetMousePressedRaw(MouseButton button)
		{
			if (button == MouseButton.None)
			{
				return false;
			}

			return UnityEngine.Input.GetKeyDown((UnityEngine.KeyCode)button);
		}

		public override bool GetMouseHeldRaw(MouseButton button)
		{
			if (button == MouseButton.None)
			{
				return false;
			}

			return UnityEngine.Input.GetKey((UnityEngine.KeyCode)button);
		}

		public override bool GetMouseReleasedRaw(MouseButton button)
		{
			if (button == MouseButton.None)
			{
				return false;
			}

			return UnityEngine.Input.GetKeyUp((UnityEngine.KeyCode)button);
		}

		public override void GetMousePosition(out int mouseX, out int mouseY)
		{
			mouseX = mousePosition.x;
			mouseY = mousePosition.y;
		}

		public override void GetMousePositionDelta(out float mouseX, out float mouseY)
		{
			mouseX = UnityEngine.Input.GetAxis("Mouse X");
			mouseY = UnityEngine.Input.GetAxis("Mouse Y");
		}

		public override float GetMouseWheelDelta()
		{
			return UnityEngine.Input.mouseScrollDelta.y;
		}

		public override MouseLockMode GetMouseCurrentLockMode()
		{
			var result = MouseLockMode.None;

			switch (UnityEngine.Cursor.lockState)
			{
				case UnityEngine.CursorLockMode.None:
					result = MouseLockMode.None;
					break;
				case UnityEngine.CursorLockMode.Confined:
					result = MouseLockMode.Confined;
					break;
				case UnityEngine.CursorLockMode.Locked:
					result = MouseLockMode.Locked;
					break;
			}

			return result;
		}

		public override bool GetMouseVisibleStatus()
		{
			return UnityEngine.Cursor.visible;
		}

		public override void SetMouseLockMode(MouseLockMode mouseLockMode)
		{
			switch (mouseLockMode)
			{
				case MouseLockMode.None:
					UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
					break;
				case MouseLockMode.Confined:
					UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.Confined;
					break;
				case MouseLockMode.Locked:
					UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.Locked;
					break;
			}
		}

		public override void SetMouseCursorVisible(bool visible)
		{
			UnityEngine.Cursor.visible = visible;
		}

		public override bool GetKeyboardPressedRaw(KeyboardKey key)
		{
			if (key == KeyboardKey.None || (int)key >= 400)
			{
				return false;
			}

			return UnityEngine.Input.GetKeyDown((UnityEngine.KeyCode)key);
		}

		public override bool GetKeyboardHeldRaw(KeyboardKey key)
		{
			if (key == KeyboardKey.None || (int)key >= 400)
			{
				return false;
			}

			return UnityEngine.Input.GetKey((UnityEngine.KeyCode)key);
		}

		public override bool GetKeyboardReleasedRaw(KeyboardKey key)
		{
			if (key == KeyboardKey.None || (int)key >= 400)
			{
				return false;
			}

			return UnityEngine.Input.GetKeyUp((UnityEngine.KeyCode)key);
		}

		public override string GetKeyboardInputStringRaw()
		{
			// "\b" == Backspace
			// "\n" == Return | Enter

			return UnityEngine.Input.inputString;
		}

		public override bool GetGamepadButtonPressedRaw(IGamepadController gamepad, GamepadButton button)
		{
			return GetGamepadButtonPressedRaw(gamepad, GetGamepadButtonRawMapping(button));
		}

		public override bool GetGamepadButtonPressedRaw(IGamepadController gamepad, GamepadButtonRaw button)
		{
			if (button == GamepadButtonRaw.None)
			{
				return false;
			}

			return UnityEngine.Input.GetKeyDown((UnityEngine.KeyCode)(GetGamepadKeyCodeOffset(gamepad) - 1 + ((int)button)));
		}

		public override bool GetGamepadButtonHeldRaw(IGamepadController gamepad, GamepadButton button)
		{
			return GetGamepadButtonHeldRaw(gamepad, GetGamepadButtonRawMapping(button));
		}

		public override bool GetGamepadButtonHeldRaw(IGamepadController gamepad, GamepadButtonRaw button)
		{
			if (button == GamepadButtonRaw.None)
			{
				return false;
			}

			return UnityEngine.Input.GetKey((UnityEngine.KeyCode)(GetGamepadKeyCodeOffset(gamepad) - 1 + ((int)button)));
		}

		public override bool GetGamepadButtonReleasedRaw(IGamepadController gamepad, GamepadButton button)
		{
			return GetGamepadButtonReleasedRaw(gamepad, GetGamepadButtonRawMapping(button));
		}

		public override bool GetGamepadButtonReleasedRaw(IGamepadController gamepad, GamepadButtonRaw button)
		{
			if (button == GamepadButtonRaw.None)
			{
				return false;
			}

			return UnityEngine.Input.GetKeyUp((UnityEngine.KeyCode)(GetGamepadKeyCodeOffset(gamepad) - 1 + ((int)button)));
		}

		private GamepadButtonRaw GetGamepadButtonRawMapping(GamepadButton button)
		{
			var result = GamepadButtonRaw.None;

#if UNITY_STANDALONE_LINUX
			switch (button)
			{
				case GamepadButton.A:
					result = GamepadButtonRaw.Button_1;
					break;
				case GamepadButton.B:
					result = GamepadButtonRaw.Button_2;
					break;
				case GamepadButton.X:
					result = GamepadButtonRaw.Button_3;
					break;
				case GamepadButton.Y:
					result = GamepadButtonRaw.Button_4;
					break;
				case GamepadButton.L:
					result = GamepadButtonRaw.Button_5;
					break;
				case GamepadButton.R:
					result = GamepadButtonRaw.Button_6;
					break;
				case GamepadButton.Select:
					result = GamepadButtonRaw.Button_7;
					break;
				case GamepadButton.Start:
					result = GamepadButtonRaw.Button_8;
					break;
				case GamepadButton.Home:
					result = GamepadButtonRaw.Button_9;
					break;
				case GamepadButton.L_Stick:
					result = GamepadButtonRaw.Button_10;
					break;
				case GamepadButton.R_Stick:
					result = GamepadButtonRaw.Button_11;
					break;
				case GamepadButton.Button_12:
					result = GamepadButtonRaw.Button_12;
					break;
				case GamepadButton.Button_13:
					result = GamepadButtonRaw.Button_13;
					break;
				case GamepadButton.Button_14:
					result = GamepadButtonRaw.Button_14;
					break;
				case GamepadButton.Button_15:
					result = GamepadButtonRaw.Button_15;
					break;
				case GamepadButton.Button_16:
					result = GamepadButtonRaw.Button_16;
					break;
				case GamepadButton.Button_17:
					result = GamepadButtonRaw.Button_17;
					break;
				case GamepadButton.Button_18:
					result = GamepadButtonRaw.Button_18;
					break;
				case GamepadButton.Button_19:
					result = GamepadButtonRaw.Button_19;
					break;
				case GamepadButton.Button_20:
					result = GamepadButtonRaw.Button_20;
					break;
			}
#elif UNITY_STANDALONE_WIN
			switch (button)
			{
				case GamepadButton.A:
					result = GamepadButtonRaw.Button_1;
					break;
				case GamepadButton.B:
					result = GamepadButtonRaw.Button_2;
					break;
				case GamepadButton.X:
					result = GamepadButtonRaw.Button_3;
					break;
				case GamepadButton.Y:
					result = GamepadButtonRaw.Button_4;
					break;
				case GamepadButton.L:
					result = GamepadButtonRaw.Button_5;
					break;
				case GamepadButton.R:
					result = GamepadButtonRaw.Button_6;
					break;
				case GamepadButton.Select:
					result = GamepadButtonRaw.Button_7;
					break;
				case GamepadButton.Start:
					result = GamepadButtonRaw.Button_8;
					break;
				case GamepadButton.L_Stick:
					result = GamepadButtonRaw.Button_9;
					break;
				case GamepadButton.R_Stick:
					result = GamepadButtonRaw.Button_10;
					break;
				case GamepadButton.Button_9:
					result = GamepadButtonRaw.Button_11;
					break;
				case GamepadButton.Button_12:
					result = GamepadButtonRaw.Button_12;
					break;
				case GamepadButton.Button_13:
					result = GamepadButtonRaw.Button_13;
					break;
				case GamepadButton.Button_14:
					result = GamepadButtonRaw.Button_14;
					break;
				case GamepadButton.Button_15:
					result = GamepadButtonRaw.Button_15;
					break;
				case GamepadButton.Button_16:
					result = GamepadButtonRaw.Button_16;
					break;
				case GamepadButton.Button_17:
					result = GamepadButtonRaw.Button_17;
					break;
				case GamepadButton.Button_18:
					result = GamepadButtonRaw.Button_18;
					break;
				case GamepadButton.Button_19:
					result = GamepadButtonRaw.Button_19;
					break;
				case GamepadButton.Button_20:
					result = GamepadButtonRaw.Button_20;
					break;
			}
#elif UNITY_STANDALONE_OSX
			switch (button)
			{
				case GamepadButton.Button_1:
					result = GamepadButtonRaw.Button_1;
					break;
				case GamepadButton.Button_2:
					result = GamepadButtonRaw.Button_2;
					break;
				case GamepadButton.Button_3:
					result = GamepadButtonRaw.Button_3;
					break;
				case GamepadButton.Button_4:
					result = GamepadButtonRaw.Button_4;
					break;
				case GamepadButton.Button_5:
					result = GamepadButtonRaw.Button_5;
					break;
				case GamepadButton.Button_6:
					result = GamepadButtonRaw.Button_6;
					break;
				case GamepadButton.Button_7:
					result = GamepadButtonRaw.Button_7;
					break;
				case GamepadButton.Button_8:
					result = GamepadButtonRaw.Button_8;
					break;
				case GamepadButton.Button_9:
					result = GamepadButtonRaw.Button_9;
					break;
				case GamepadButton.Start:
					result = GamepadButtonRaw.Button_10;
					break;
				case GamepadButton.Select:
					result = GamepadButtonRaw.Button_11;
					break;
				case GamepadButton.L_Stick:
					result = GamepadButtonRaw.Button_12;
					break;
				case GamepadButton.R_Stick:
					result = GamepadButtonRaw.Button_13;
					break;
				case GamepadButton.L:
					result = GamepadButtonRaw.Button_14;
					break;
				case GamepadButton.R:
					result = GamepadButtonRaw.Button_15;
					break;
				case GamepadButton.Button_16:
					result = GamepadButtonRaw.Button_16;
					break;
				case GamepadButton.A:
					result = GamepadButtonRaw.Button_17;
					break;
				case GamepadButton.B:
					result = GamepadButtonRaw.Button_18;
					break;
				case GamepadButton.X:
					result = GamepadButtonRaw.Button_19;
					break;
				case GamepadButton.Y:
					result = GamepadButtonRaw.Button_20;
					break;
			}
#endif

			return result;
		}

		public override float GetGamepadAxisRaw(IGamepadController gamepad, GamepadAxis axis)
		{
			var axisValue = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(axis));

#if UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
			if (axis == GamepadAxis.D_Pad_Left || axis == GamepadAxis.D_Pad_Top ||
				axis == GamepadAxis.L_Stick_Left || axis == GamepadAxis.L_Stick_Top ||
				axis == GamepadAxis.R_Stick_Left || axis == GamepadAxis.R_Stick_Top)
			{
				axisValue *= -1;
			}
#elif UNITY_STANDALONE_WIN
			if (axis == GamepadAxis.D_Pad_Left ||
				axis == GamepadAxis.L_Stick_Left || axis == GamepadAxis.L_Stick_Top ||
				axis == GamepadAxis.R_Stick_Left || axis == GamepadAxis.R_Stick_Top)
			{
				axisValue *= -1;
			}
#endif

			return axisValue;
		}

		public override float GetGamepadAxisRaw(IGamepadController gamepad, GamepadAxisRaw axis)
		{
			var axisValue = 0f;

			if (axis == GamepadAxisRaw.None)
			{
				return axisValue;
			}

			var gamepadIndex = GetGamepadIndex(gamepad);
			var axisIndex = (int)axis;

			if (axisIndex > 0 && gamepadIndex > 0)
			{
				// Input Axis Names (Project Settings -> Input Manager)
				//
				// Axis{#1}_{#2}
				// 		Number {#1} should be 1~10 (Axis index)
				// 		Number {#2} can be 1~16 (Gamepad index)
				//
				// Important: Set sensibility to 1 and gravity to 0

				var axisName = $"Axis{axisIndex}_{gamepadIndex}";

				try
				{
					axisValue = UnityEngine.Input.GetAxisRaw(axisName);
				}
				catch (System.Exception e)
				{
					Log.Exception(null, e);
				}
			}

			return axisValue;
		}

		public override void GetGamepadAxisRawRaw(IGamepadController gamepad, GamepadAxis axis, out float axisX, out float axisY)
		{
			var axisValue = Vector2.zero;

			if (axis == GamepadAxis.None)
			{
				axisX = 0f;
				axisY = 0f;

				return;
			}

			switch (axis)
			{
				case GamepadAxis.D_Pad:
					axisValue.x = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.D_Pad_Left));
					axisValue.y = -GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.D_Pad_Top));
					break;
				case GamepadAxis.L_Stick:
					axisValue.x = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.L_Stick_Left));
					axisValue.y = -GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.L_Stick_Top));
					break;
				case GamepadAxis.R_Stick:
					axisValue.x = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.R_Stick_Left));
					axisValue.y = -GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.R_Stick_Top));
					break;
			}

			axisX = axisValue.x;
			axisY = axisValue.y;
		}

		private GamepadAxisRaw GetGamepadAxisRawMapping(GamepadAxis axis)
		{
			var result = GamepadAxisRaw.None;

#if UNITY_STANDALONE_LINUX
			switch (axis)
			{
				case GamepadAxis.L_Stick_Left:
				case GamepadAxis.L_Stick_Right:
					result = GamepadAxisRaw.AxisX;
					break;
				case GamepadAxis.L_Stick_Top:
				case GamepadAxis.L_Stick_Bottom:
					result = GamepadAxisRaw.AxisY;
					break;
				case GamepadAxis.R_Stick_Left:
				case GamepadAxis.R_Stick_Right:
					result = GamepadAxisRaw.Axis4;
					break;
				case GamepadAxis.R_Stick_Top:
				case GamepadAxis.R_Stick_Bottom:
					result = GamepadAxisRaw.Axis5;
					break;
				case GamepadAxis.L_Trigger:
					result = GamepadAxisRaw.Axis3;
					break;
				case GamepadAxis.R_Trigger:
					result = GamepadAxisRaw.Axis6;
					break;
				case GamepadAxis.D_Pad_Left:
				case GamepadAxis.D_Pad_Right:
					result = GamepadAxisRaw.Axis7;
					break;
				case GamepadAxis.D_Pad_Top:
				case GamepadAxis.D_Pad_Bottom:
					result = GamepadAxisRaw.Axis8;
					break;
				case GamepadAxis.Axis9:
					result = GamepadAxisRaw.Axis9;
					break;
				case GamepadAxis.Axis10:
					result = GamepadAxisRaw.Axis10;
					break;
				case GamepadAxis.Axis11:
					result = GamepadAxisRaw.Axis11;
					break;
				case GamepadAxis.Axis12:
					result = GamepadAxisRaw.Axis12;
					break;
				case GamepadAxis.Axis13:
					result = GamepadAxisRaw.Axis13;
					break;
				case GamepadAxis.Axis14:
					result = GamepadAxisRaw.Axis14;
					break;
				case GamepadAxis.Axis15:
					result = GamepadAxisRaw.Axis15;
					break;
				case GamepadAxis.Axis16:
					result = GamepadAxisRaw.Axis16;
					break;
				case GamepadAxis.Axis17:
					result = GamepadAxisRaw.Axis17;
					break;
				case GamepadAxis.Axis18:
					result = GamepadAxisRaw.Axis18;
					break;
				case GamepadAxis.Axis19:
					result = GamepadAxisRaw.Axis19;
					break;
				case GamepadAxis.Axis20:
					result = GamepadAxisRaw.Axis20;
					break;
				case GamepadAxis.Axis21:
					result = GamepadAxisRaw.Axis21;
					break;
				case GamepadAxis.Axis22:
					result = GamepadAxisRaw.Axis22;
					break;
				case GamepadAxis.Axis23:
					result = GamepadAxisRaw.Axis23;
					break;
				case GamepadAxis.Axis24:
					result = GamepadAxisRaw.Axis24;
					break;
				case GamepadAxis.Axis25:
					result = GamepadAxisRaw.Axis25;
					break;
				case GamepadAxis.Axis26:
					result = GamepadAxisRaw.Axis26;
					break;
				case GamepadAxis.Axis27:
					result = GamepadAxisRaw.Axis27;
					break;
				case GamepadAxis.Axis28:
					result = GamepadAxisRaw.Axis28;
					break;
			}
#elif UNITY_STANDALONE_WIN
			switch (axis)
			{
				case GamepadAxis.L_Stick_Left:
				case GamepadAxis.L_Stick_Right:
					result = GamepadAxisRaw.AxisX;
					break;
				case GamepadAxis.L_Stick_Top:
				case GamepadAxis.L_Stick_Bottom:
					result = GamepadAxisRaw.AxisY;
					break;
				case GamepadAxis.R_Stick_Left:
				case GamepadAxis.R_Stick_Right:
					result = GamepadAxisRaw.Axis4;
					break;
				case GamepadAxis.R_Stick_Top:
				case GamepadAxis.R_Stick_Bottom:
					result = GamepadAxisRaw.Axis5;
					break;
				case GamepadAxis.L_Trigger:
					result = GamepadAxisRaw.Axis9;
					break;
				case GamepadAxis.R_Trigger:
					result = GamepadAxisRaw.Axis10;
					break;
				case GamepadAxis.D_Pad_Left:
				case GamepadAxis.D_Pad_Right:
					result = GamepadAxisRaw.Axis6;
					break;
				case GamepadAxis.D_Pad_Top:
				case GamepadAxis.D_Pad_Bottom:
					result = GamepadAxisRaw.Axis7;
					break;
				case GamepadAxis.Axis9:
					result = GamepadAxisRaw.Axis3;
					break;
				case GamepadAxis.Axis10:
					result = GamepadAxisRaw.Axis8;
					break;
				case GamepadAxis.Axis11:
					result = GamepadAxisRaw.Axis11;
					break;
				case GamepadAxis.Axis12:
					result = GamepadAxisRaw.Axis12;
					break;
				case GamepadAxis.Axis13:
					result = GamepadAxisRaw.Axis13;
					break;
				case GamepadAxis.Axis14:
					result = GamepadAxisRaw.Axis14;
					break;
				case GamepadAxis.Axis15:
					result = GamepadAxisRaw.Axis15;
					break;
				case GamepadAxis.Axis16:
					result = GamepadAxisRaw.Axis16;
					break;
				case GamepadAxis.Axis17:
					result = GamepadAxisRaw.Axis17;
					break;
				case GamepadAxis.Axis18:
					result = GamepadAxisRaw.Axis18;
					break;
				case GamepadAxis.Axis19:
					result = GamepadAxisRaw.Axis19;
					break;
				case GamepadAxis.Axis20:
					result = GamepadAxisRaw.Axis20;
					break;
				case GamepadAxis.Axis21:
					result = GamepadAxisRaw.Axis21;
					break;
				case GamepadAxis.Axis22:
					result = GamepadAxisRaw.Axis22;
					break;
				case GamepadAxis.Axis23:
					result = GamepadAxisRaw.Axis23;
					break;
				case GamepadAxis.Axis24:
					result = GamepadAxisRaw.Axis24;
					break;
				case GamepadAxis.Axis25:
					result = GamepadAxisRaw.Axis25;
					break;
				case GamepadAxis.Axis26:
					result = GamepadAxisRaw.Axis26;
					break;
				case GamepadAxis.Axis27:
					result = GamepadAxisRaw.Axis27;
					break;
				case GamepadAxis.Axis28:
					result = GamepadAxisRaw.Axis28;
					break;
			}
#elif UNITY_STANDALONE_OSX
			switch (axis)
			{
				case GamepadAxis.L_Stick_Left:
				case GamepadAxis.L_Stick_Right:
					result = GamepadAxisRaw.AxisX;
					break;
				case GamepadAxis.L_Stick_Top:
				case GamepadAxis.L_Stick_Bottom:
					result = GamepadAxisRaw.AxisY;
					break;
				case GamepadAxis.R_Stick_Left:
				case GamepadAxis.R_Stick_Right:
					result = GamepadAxisRaw.Axis3;
					break;
				case GamepadAxis.R_Stick_Top:
				case GamepadAxis.R_Stick_Bottom:
					result = GamepadAxisRaw.Axis4;
					break;
				case GamepadAxis.L_Trigger:
					result = GamepadAxisRaw.Axis5;
					break;
				case GamepadAxis.R_Trigger:
					result = GamepadAxisRaw.Axis6;
					break;
				case GamepadAxis.D_Pad_Left:
				case GamepadAxis.D_Pad_Right:
					result = GamepadAxisRaw.Axis7;
					break;
				case GamepadAxis.D_Pad_Top:
				case GamepadAxis.D_Pad_Bottom:
					result = GamepadAxisRaw.Axis8;
					break;
				case GamepadAxis.Axis9:
					result = GamepadAxisRaw.Axis9;
					break;
				case GamepadAxis.Axis10:
					result = GamepadAxisRaw.Axis10;
					break;
				case GamepadAxis.Axis11:
					result = GamepadAxisRaw.Axis11;
					break;
				case GamepadAxis.Axis12:
					result = GamepadAxisRaw.Axis12;
					break;
				case GamepadAxis.Axis13:
					result = GamepadAxisRaw.Axis13;
					break;
				case GamepadAxis.Axis14:
					result = GamepadAxisRaw.Axis14;
					break;
				case GamepadAxis.Axis15:
					result = GamepadAxisRaw.Axis15;
					break;
				case GamepadAxis.Axis16:
					result = GamepadAxisRaw.Axis16;
					break;
				case GamepadAxis.Axis17:
					result = GamepadAxisRaw.Axis17;
					break;
				case GamepadAxis.Axis18:
					result = GamepadAxisRaw.Axis18;
					break;
				case GamepadAxis.Axis19:
					result = GamepadAxisRaw.Axis19;
					break;
				case GamepadAxis.Axis20:
					result = GamepadAxisRaw.Axis20;
					break;
				case GamepadAxis.Axis21:
					result = GamepadAxisRaw.Axis21;
					break;
				case GamepadAxis.Axis22:
					result = GamepadAxisRaw.Axis22;
					break;
				case GamepadAxis.Axis23:
					result = GamepadAxisRaw.Axis23;
					break;
				case GamepadAxis.Axis24:
					result = GamepadAxisRaw.Axis24;
					break;
				case GamepadAxis.Axis25:
					result = GamepadAxisRaw.Axis25;
					break;
				case GamepadAxis.Axis26:
					result = GamepadAxisRaw.Axis26;
					break;
				case GamepadAxis.Axis27:
					result = GamepadAxisRaw.Axis27;
					break;
				case GamepadAxis.Axis28:
					result = GamepadAxisRaw.Axis28;
					break;
			}
#endif

			return result;
		}

		private int GetGamepadIndex(IGamepadController gamepad)
		{
			var gamepadIndex = 0;

			if (gamepads.Contains(gamepad))
			{
				gamepadIndex = (gamepads.IndexOf(gamepad) + 1);
			}

			return gamepadIndex;
		}

		private int GetGamepadKeyCodeOffset(IGamepadController gamepad)
		{
			// Note: Index min = 0, max = 7

			return 350 + ((GetGamepadIndex(gamepad) - 1) * 20);
		}
	}
}

#endif
#endif
