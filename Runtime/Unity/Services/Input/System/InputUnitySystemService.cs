// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER
#if ENABLE_INPUT_SYSTEM

using Ju.Extensions;
using Ju.Input;
using Ju.Time;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Ju.Services
{
	public class InputUnityService : InputService, IServiceUnload
	{
		private ClockPrecise interval;
		private readonly float UPDATE_INTERVAL = 2f;

		private string strCache = "";

		public override void Initialize()
		{
			interval = new ClockPrecise();

			UnityEngine.InputSystem.Keyboard.current.onTextInput += OnTextInput;

			foreach (var gamepad in UnityEngine.InputSystem.Gamepad.all)
			{
				ConnectGamepad(GetUniqueGamepadId(gamepad));
			}
		}

		public void Unload()
		{
			UnityEngine.InputSystem.Keyboard.current.onTextInput -= OnTextInput;
		}

		private void OnTextInput(char character)
		{
			strCache += character;
		}

		public override void Update()
		{
			if (interval.GetElapsedTime().seconds < UPDATE_INTERVAL)
			{
				return;
			}

			interval.Reset();

			if (strCache.Length > 100)
			{
				strCache = string.Empty;
			}

			foreach (var gamepad in UnityEngine.InputSystem.Gamepad.all)
			{
				ConnectGamepad(GetUniqueGamepadId(gamepad));
			}

			foreach (var gamepad in gamepads)
			{
				if (!UnityEngine.InputSystem.Gamepad.all.Any(g => GetUniqueGamepadId(g) == gamepad.Id))
				{
					DisconnectGamepad(gamepad.Id);
				}
			}
		}

		private string GetUniqueGamepadId(UnityEngine.InputSystem.Gamepad gamepad)
		{
			return $"ID{gamepad.deviceId}_{gamepad.name}";
		}

		public override bool GetMousePressedRaw(MouseButton button)
		{
			if (button == MouseButton.None)
			{
				return false;
			}

			var result = false;
			var control = GetMouseButtonMapping(button);

			if (control != null)
			{
				result = control.wasPressedThisFrame;
			}

			return result;
		}

		public override bool GetMouseHeldRaw(MouseButton button)
		{
			if (button == MouseButton.None)
			{
				return false;
			}

			var result = false;
			var control = GetMouseButtonMapping(button);

			if (control != null)
			{
				result = control.isPressed;
			}

			return result;
		}

		public override bool GetMouseReleasedRaw(MouseButton button)
		{
			if (button == MouseButton.None)
			{
				return false;
			}

			var result = false;
			var control = GetMouseButtonMapping(button);

			if (control != null)
			{
				result = control.wasReleasedThisFrame;
			}

			return result;
		}

		private ButtonControl GetMouseButtonMapping(MouseButton button)
		{
			ButtonControl result = null;

			switch (button)
			{
				case MouseButton.Left:
					result = UnityEngine.InputSystem.Mouse.current.leftButton;
					break;
				case MouseButton.Middle:
					result = UnityEngine.InputSystem.Mouse.current.middleButton;
					break;
				case MouseButton.Right:
					result = UnityEngine.InputSystem.Mouse.current.rightButton;
					break;
				case MouseButton.Button4:
				case MouseButton.Button6:
					result = UnityEngine.InputSystem.Mouse.current.backButton;
					break;
				case MouseButton.Button5:
				case MouseButton.Button7:
					result = UnityEngine.InputSystem.Mouse.current.forwardButton;
					break;
			}

			return result;
		}

		public override void GetMousePosition(out int mouseX, out int mouseY)
		{
			var mousePosition = UnityEngine.InputSystem.Mouse.current.position;

			mouseX = (int)mousePosition.x.ReadUnprocessedValue();
			mouseY = (int)mousePosition.y.ReadUnprocessedValue();
		}

		public override void GetMousePositionDelta(out float mouseX, out float mouseY)
		{
			var mouseDelta = UnityEngine.InputSystem.Mouse.current.delta;

			mouseX = mouseDelta.x.ReadUnprocessedValue();
			mouseY = mouseDelta.y.ReadUnprocessedValue();
		}

		public override float GetMouseWheelDelta()
		{
			return UnityEngine.InputSystem.Mouse.current.scroll.y.ReadUnprocessedValue();
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
			if (key == KeyboardKey.None)
			{
				return false;
			}

			var keyIndex = GetKeyboardKeyMapping(key);

			if (keyIndex == Key.None)
			{
				return false;
			}

			return UnityEngine.InputSystem.Keyboard.current[keyIndex].wasPressedThisFrame;
		}

		public override bool GetKeyboardHeldRaw(KeyboardKey key)
		{
			if (key == KeyboardKey.None)
			{
				return false;
			}

			var keyIndex = GetKeyboardKeyMapping(key);

			if (keyIndex == Key.None)
			{
				return false;
			}

			return UnityEngine.InputSystem.Keyboard.current[keyIndex].isPressed;
		}

		public override bool GetKeyboardReleasedRaw(KeyboardKey key)
		{
			if (key == KeyboardKey.None)
			{
				return false;
			}

			var keyIndex = GetKeyboardKeyMapping(key);

			if (keyIndex == Key.None)
			{
				return false;
			}

			return UnityEngine.InputSystem.Keyboard.current[keyIndex].wasReleasedThisFrame;
		}

		private Key GetKeyboardKeyMapping(KeyboardKey key)
		{
			var result = Key.None;

			switch (key)
			{
				case KeyboardKey.Backspace:
					result = UnityEngine.InputSystem.Key.Backspace;
					break;
				case KeyboardKey.Tab:
					result = UnityEngine.InputSystem.Key.Tab;
					break;
				case KeyboardKey.Return:
					result = UnityEngine.InputSystem.Key.Enter;
					break;
				case KeyboardKey.Pause:
					result = UnityEngine.InputSystem.Key.Pause;
					break;
				case KeyboardKey.Escape:
					result = UnityEngine.InputSystem.Key.Escape;
					break;
				case KeyboardKey.Space:
					result = UnityEngine.InputSystem.Key.Space;
					break;
				case KeyboardKey.Quote:                                            // ' (Apostrophe)
					result = UnityEngine.InputSystem.Key.Quote;
					break;
				case KeyboardKey.Comma:                                            // ,
					result = UnityEngine.InputSystem.Key.Comma;
					break;
				case KeyboardKey.Minus:                                            // -
					result = UnityEngine.InputSystem.Key.Minus;
					break;
				case KeyboardKey.Period:                                           // .
					result = UnityEngine.InputSystem.Key.Period;
					break;
				case KeyboardKey.Slash:                                            // /
					result = UnityEngine.InputSystem.Key.Slash;
					break;
				case KeyboardKey.Semicolon:                                        // ;
					result = UnityEngine.InputSystem.Key.Semicolon;
					break;
				case KeyboardKey.Equals:                                           // =
					result = UnityEngine.InputSystem.Key.Equals;
					break;
				case KeyboardKey.LeftBracket:                                      // [
					result = UnityEngine.InputSystem.Key.LeftBracket;
					break;
				case KeyboardKey.Backslash:                                        // \
					result = UnityEngine.InputSystem.Key.Backslash;
					break;
				case KeyboardKey.RightBracket:                                     // ]
					result = UnityEngine.InputSystem.Key.RightBracket;
					break;
				case KeyboardKey.BackQuote:                                        // ` (Grave accent)
					result = UnityEngine.InputSystem.Key.Backquote;
					break;
				case KeyboardKey.Num0:
					result = UnityEngine.InputSystem.Key.Digit0;
					break;
				case KeyboardKey.Num1:
					result = UnityEngine.InputSystem.Key.Digit1;
					break;
				case KeyboardKey.Num2:
					result = UnityEngine.InputSystem.Key.Digit2;
					break;
				case KeyboardKey.Num3:
					result = UnityEngine.InputSystem.Key.Digit3;
					break;
				case KeyboardKey.Num4:
					result = UnityEngine.InputSystem.Key.Digit4;
					break;
				case KeyboardKey.Num5:
					result = UnityEngine.InputSystem.Key.Digit5;
					break;
				case KeyboardKey.Num6:
					result = UnityEngine.InputSystem.Key.Digit6;
					break;
				case KeyboardKey.Num7:
					result = UnityEngine.InputSystem.Key.Digit7;
					break;
				case KeyboardKey.Num8:
					result = UnityEngine.InputSystem.Key.Digit8;
					break;
				case KeyboardKey.Num9:
					result = UnityEngine.InputSystem.Key.Digit9;
					break;
				case KeyboardKey.A:
					result = UnityEngine.InputSystem.Key.A;
					break;
				case KeyboardKey.B:
					result = UnityEngine.InputSystem.Key.B;
					break;
				case KeyboardKey.C:
					result = UnityEngine.InputSystem.Key.C;
					break;
				case KeyboardKey.D:
					result = UnityEngine.InputSystem.Key.D;
					break;
				case KeyboardKey.E:
					result = UnityEngine.InputSystem.Key.E;
					break;
				case KeyboardKey.F:
					result = UnityEngine.InputSystem.Key.F;
					break;
				case KeyboardKey.G:
					result = UnityEngine.InputSystem.Key.G;
					break;
				case KeyboardKey.H:
					result = UnityEngine.InputSystem.Key.H;
					break;
				case KeyboardKey.I:
					result = UnityEngine.InputSystem.Key.I;
					break;
				case KeyboardKey.J:
					result = UnityEngine.InputSystem.Key.J;
					break;
				case KeyboardKey.K:
					result = UnityEngine.InputSystem.Key.K;
					break;
				case KeyboardKey.L:
					result = UnityEngine.InputSystem.Key.L;
					break;
				case KeyboardKey.M:
					result = UnityEngine.InputSystem.Key.M;
					break;
				case KeyboardKey.N:
					result = UnityEngine.InputSystem.Key.N;
					break;
				case KeyboardKey.O:
					result = UnityEngine.InputSystem.Key.O;
					break;
				case KeyboardKey.P:
					result = UnityEngine.InputSystem.Key.P;
					break;
				case KeyboardKey.Q:
					result = UnityEngine.InputSystem.Key.Q;
					break;
				case KeyboardKey.R:
					result = UnityEngine.InputSystem.Key.R;
					break;
				case KeyboardKey.S:
					result = UnityEngine.InputSystem.Key.S;
					break;
				case KeyboardKey.T:
					result = UnityEngine.InputSystem.Key.T;
					break;
				case KeyboardKey.U:
					result = UnityEngine.InputSystem.Key.U;
					break;
				case KeyboardKey.V:
					result = UnityEngine.InputSystem.Key.V;
					break;
				case KeyboardKey.W:
					result = UnityEngine.InputSystem.Key.W;
					break;
				case KeyboardKey.X:
					result = UnityEngine.InputSystem.Key.X;
					break;
				case KeyboardKey.Y:
					result = UnityEngine.InputSystem.Key.Y;
					break;
				case KeyboardKey.Z:
					result = UnityEngine.InputSystem.Key.Z;
					break;
				case KeyboardKey.Keypad0:
					result = UnityEngine.InputSystem.Key.Numpad0;
					break;
				case KeyboardKey.Keypad1:
					result = UnityEngine.InputSystem.Key.Numpad1;
					break;
				case KeyboardKey.Keypad2:
					result = UnityEngine.InputSystem.Key.Numpad2;
					break;
				case KeyboardKey.Keypad3:
					result = UnityEngine.InputSystem.Key.Numpad3;
					break;
				case KeyboardKey.Keypad4:
					result = UnityEngine.InputSystem.Key.Numpad4;
					break;
				case KeyboardKey.Keypad5:
					result = UnityEngine.InputSystem.Key.Numpad5;
					break;
				case KeyboardKey.Keypad6:
					result = UnityEngine.InputSystem.Key.Numpad6;
					break;
				case KeyboardKey.Keypad7:
					result = UnityEngine.InputSystem.Key.Numpad7;
					break;
				case KeyboardKey.Keypad8:
					result = UnityEngine.InputSystem.Key.Numpad8;
					break;
				case KeyboardKey.Keypad9:
					result = UnityEngine.InputSystem.Key.Numpad9;
					break;
				case KeyboardKey.KeypadPeriod:
					result = UnityEngine.InputSystem.Key.NumpadPeriod;
					break;
				case KeyboardKey.KeypadDivide:
					result = UnityEngine.InputSystem.Key.NumpadDivide;
					break;
				case KeyboardKey.KeypadMultiply:
					result = UnityEngine.InputSystem.Key.NumpadMultiply;
					break;
				case KeyboardKey.KeypadMinus:
					result = UnityEngine.InputSystem.Key.NumpadMinus;
					break;
				case KeyboardKey.KeypadPlus:
					result = UnityEngine.InputSystem.Key.NumpadPlus;
					break;
				case KeyboardKey.KeypadEquals:
					result = UnityEngine.InputSystem.Key.NumpadEquals;
					break;
				case KeyboardKey.KeypadEnter:
					result = UnityEngine.InputSystem.Key.NumpadEnter;
					break;
				case KeyboardKey.UpArrow:
					result = UnityEngine.InputSystem.Key.UpArrow;
					break;
				case KeyboardKey.DownArrow:
					result = UnityEngine.InputSystem.Key.DownArrow;
					break;
				case KeyboardKey.RightArrow:
					result = UnityEngine.InputSystem.Key.RightArrow;
					break;
				case KeyboardKey.LeftArrow:
					result = UnityEngine.InputSystem.Key.LeftArrow;
					break;
				case KeyboardKey.Delete:
					result = UnityEngine.InputSystem.Key.Delete;
					break;
				case KeyboardKey.Insert:
					result = UnityEngine.InputSystem.Key.Insert;
					break;
				case KeyboardKey.Home:
					result = UnityEngine.InputSystem.Key.Home;
					break;
				case KeyboardKey.End:
					result = UnityEngine.InputSystem.Key.End;
					break;
				case KeyboardKey.PageUp:
					result = UnityEngine.InputSystem.Key.PageUp;
					break;
				case KeyboardKey.PageDown:
					result = UnityEngine.InputSystem.Key.PageDown;
					break;
				case KeyboardKey.F1:
					result = UnityEngine.InputSystem.Key.F1;
					break;
				case KeyboardKey.F2:
					result = UnityEngine.InputSystem.Key.F2;
					break;
				case KeyboardKey.F3:
					result = UnityEngine.InputSystem.Key.F3;
					break;
				case KeyboardKey.F4:
					result = UnityEngine.InputSystem.Key.F4;
					break;
				case KeyboardKey.F5:
					result = UnityEngine.InputSystem.Key.F5;
					break;
				case KeyboardKey.F6:
					result = UnityEngine.InputSystem.Key.F6;
					break;
				case KeyboardKey.F7:
					result = UnityEngine.InputSystem.Key.F7;
					break;
				case KeyboardKey.F8:
					result = UnityEngine.InputSystem.Key.F8;
					break;
				case KeyboardKey.F9:
					result = UnityEngine.InputSystem.Key.F9;
					break;
				case KeyboardKey.F10:
					result = UnityEngine.InputSystem.Key.F10;
					break;
				case KeyboardKey.F11:
					result = UnityEngine.InputSystem.Key.F11;
					break;
				case KeyboardKey.F12:
					result = UnityEngine.InputSystem.Key.F12;
					break;
				case KeyboardKey.Numlock:
					result = UnityEngine.InputSystem.Key.NumLock;
					break;
				case KeyboardKey.CapsLock:
					result = UnityEngine.InputSystem.Key.CapsLock;
					break;
				case KeyboardKey.ScrollLock:
					result = UnityEngine.InputSystem.Key.ScrollLock;
					break;
				case KeyboardKey.RightShift:
					result = UnityEngine.InputSystem.Key.RightShift;
					break;
				case KeyboardKey.LeftShift:
					result = UnityEngine.InputSystem.Key.LeftShift;
					break;
				case KeyboardKey.RightControl:
					result = UnityEngine.InputSystem.Key.RightCtrl;
					break;
				case KeyboardKey.LeftControl:
					result = UnityEngine.InputSystem.Key.LeftCtrl;
					break;
				case KeyboardKey.RightAlt:
					result = UnityEngine.InputSystem.Key.RightAlt;
					break;
				case KeyboardKey.LeftAlt:
					result = UnityEngine.InputSystem.Key.LeftAlt;
					break;
				case KeyboardKey.RightCommand:
					result = UnityEngine.InputSystem.Key.RightCommand;
					break;
				case KeyboardKey.LeftCommand:
					result = UnityEngine.InputSystem.Key.LeftCommand;
					break;
				case KeyboardKey.LeftWindows:
					result = UnityEngine.InputSystem.Key.LeftWindows;
					break;
				case KeyboardKey.RightWindows:
					result = UnityEngine.InputSystem.Key.RightWindows;
					break;
				case KeyboardKey.AltGr:
					result = UnityEngine.InputSystem.Key.AltGr;
					break;
				case KeyboardKey.Print:
					result = UnityEngine.InputSystem.Key.PrintScreen;
					break;
				case KeyboardKey.Menu:
					result = UnityEngine.InputSystem.Key.ContextMenu;
					break;
				case KeyboardKey.OEM1:
					result = UnityEngine.InputSystem.Key.OEM1;
					break;
				case KeyboardKey.OEM2:
					result = UnityEngine.InputSystem.Key.OEM2;
					break;
				case KeyboardKey.OEM3:
					result = UnityEngine.InputSystem.Key.OEM3;
					break;
				case KeyboardKey.OEM4:
					result = UnityEngine.InputSystem.Key.OEM4;
					break;
				case KeyboardKey.OEM5:
					result = UnityEngine.InputSystem.Key.OEM5;
					break;
			}

			return result;
		}

		public override string GetKeyboardInputStringRaw()
		{
			var result = strCache;
			strCache = string.Empty;

			return result;
		}

		public override bool GetGamepadButtonPressedRaw(IGamepadController gamepad, GamepadButton button)
		{
			return GetGamepadButtonPressedRaw(gamepad, GetGamepadButtonRawMapping(button));
		}

		public override bool GetGamepadButtonPressedRaw(IGamepadController gamepad, GamepadButtonRaw button)
		{
			if (button == GamepadButtonRaw.None || button == GamepadButtonRaw.Button_9 || (int)button >= (int)GamepadButtonRaw.Button_12)
			{
				return false;
			}

			var gamepadRaw = GetGamepad(gamepad);

			if (gamepadRaw == null)
			{
				return false;
			}

			return gamepadRaw[GetGamepadButtonMapping(button)].wasPressedThisFrame;
		}

		public override bool GetGamepadButtonHeldRaw(IGamepadController gamepad, GamepadButton button)
		{
			return GetGamepadButtonHeldRaw(gamepad, GetGamepadButtonRawMapping(button));
		}

		public override bool GetGamepadButtonHeldRaw(IGamepadController gamepad, GamepadButtonRaw button)
		{
			if (button == GamepadButtonRaw.None || button == GamepadButtonRaw.Button_9 || (int)button >= (int)GamepadButtonRaw.Button_12)
			{
				return false;
			}

			var gamepadRaw = GetGamepad(gamepad);

			if (gamepadRaw == null)
			{
				return false;
			}

			return gamepadRaw[GetGamepadButtonMapping(button)].isPressed;
		}

		public override bool GetGamepadButtonReleasedRaw(IGamepadController gamepad, GamepadButton button)
		{
			return GetGamepadButtonReleasedRaw(gamepad, GetGamepadButtonRawMapping(button));
		}

		public override bool GetGamepadButtonReleasedRaw(IGamepadController gamepad, GamepadButtonRaw button)
		{
			if (button == GamepadButtonRaw.None || button == GamepadButtonRaw.Button_9 || (int)button >= (int)GamepadButtonRaw.Button_12)
			{
				return false;
			}

			var gamepadRaw = GetGamepad(gamepad);

			if (gamepadRaw == null)
			{
				return false;
			}

			return gamepadRaw[GetGamepadButtonMapping(button)].wasReleasedThisFrame;
		}

		private Gamepad GetGamepad(IGamepadController gamepad)
		{
			return UnityEngine.InputSystem.Gamepad.all.Find(null, g => GetUniqueGamepadId(g) == gamepad.Id);
		}

		private GamepadButtonRaw GetGamepadButtonRawMapping(GamepadButton button)
		{
			var result = GamepadButtonRaw.None;

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
					result = GamepadButtonRaw.Button_10;
					break;
				case GamepadButton.R_Stick:
					result = GamepadButtonRaw.Button_11;
					break;
			}

			return result;
		}

		private UnityEngine.InputSystem.LowLevel.GamepadButton GetGamepadButtonMapping(GamepadButtonRaw button)
		{
			var result = UnityEngine.InputSystem.LowLevel.GamepadButton.Start; // No "None"

			switch (button)
			{
				case GamepadButtonRaw.Button_1:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.A;
					break;
				case GamepadButtonRaw.Button_2:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.B;
					break;
				case GamepadButtonRaw.Button_3:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.X;
					break;
				case GamepadButtonRaw.Button_4:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.Y;
					break;
				case GamepadButtonRaw.Button_5:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.LeftShoulder;
					break;
				case GamepadButtonRaw.Button_6:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.RightShoulder;
					break;
				case GamepadButtonRaw.Button_7:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.Select;
					break;
				case GamepadButtonRaw.Button_8:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.Start;
					break;
				case GamepadButtonRaw.Button_10:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.LeftStick;
					break;
				case GamepadButtonRaw.Button_11:
					result = UnityEngine.InputSystem.LowLevel.GamepadButton.RightStick;
					break;
			}

			return result;
		}

		public override float GetGamepadAxisRaw(IGamepadController gamepad, GamepadAxis axis)
		{
			return GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(axis));
		}

		public override float GetGamepadAxisRaw(IGamepadController gamepad, GamepadAxisRaw axis)
		{
			var result = 0f;

			var gamepadRaw = GetGamepad(gamepad);

			if (gamepadRaw != null)
			{
				switch (axis)
				{
					case GamepadAxisRaw.AxisX:
						result = gamepadRaw.leftStick.x.ReadUnprocessedValue();
						break;
					case GamepadAxisRaw.AxisY:
						result = gamepadRaw.leftStick.y.ReadUnprocessedValue();
						break;
					case GamepadAxisRaw.Axis4:
						result = gamepadRaw.rightStick.x.ReadUnprocessedValue();
						break;
					case GamepadAxisRaw.Axis5:
						result = gamepadRaw.rightStick.y.ReadUnprocessedValue();
						break;
					case GamepadAxisRaw.Axis3:
						result = gamepadRaw.leftTrigger.ReadUnprocessedValue();
						break;
					case GamepadAxisRaw.Axis6:
						result = gamepadRaw.rightTrigger.ReadUnprocessedValue();
						break;
					case GamepadAxisRaw.Axis7:
						result = gamepadRaw.dpad.x.ReadUnprocessedValue();
						break;
					case GamepadAxisRaw.Axis8:
						result = gamepadRaw.dpad.y.ReadUnprocessedValue();
						break;
				}
			}

			return result;
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
					axisValue.y = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.D_Pad_Top));
					break;
				case GamepadAxis.L_Stick:
					axisValue.x = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.L_Stick_Left));
					axisValue.y = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.L_Stick_Top));
					break;
				case GamepadAxis.R_Stick:
					axisValue.x = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.R_Stick_Left));
					axisValue.y = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.R_Stick_Top));
					break;
			}

			axisX = axisValue.x;
			axisY = axisValue.y;
		}

		private GamepadAxisRaw GetGamepadAxisRawMapping(GamepadAxis axis)
		{
			var result = GamepadAxisRaw.None;

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
			}

			return result;
		}
	}
}

#endif
#endif
