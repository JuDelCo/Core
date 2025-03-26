// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System.Collections.Generic;
using Ju.Extensions;
using Ju.Input;
using Ju.Services.Extensions;
using Ju.Time;

namespace Ju.Services
{
	public class InputGodotService : InputService
	{
		private ClockPrecise interval;
		private readonly float UPDATE_INTERVAL = 2f;

		private Godot.Window root;

		private string strCache = string.Empty;

		private Godot.Vector2 currentMousePosition = Godot.Vector2.Zero;
		private Godot.Vector2 previousMousePosition = Godot.Vector2.Zero;
		private float mouseWheelDelta = 0f;

		private List<Godot.MouseButton> pressedMouseButtons;
		private List<Godot.MouseButton> releasedMouseButtons;

		private struct KeyInfo
		{
			public Godot.Key key;
			public Godot.KeyLocation location;
		}

		private List<KeyInfo> pressedKeyboardKeys;
		private List<KeyInfo> releasedKeyboardKeys;

		private struct JoyButtonInfo
		{
			public int device;
			public Godot.JoyButton button;
		}

		private List<JoyButtonInfo> pressedJoyButtons;
		private List<JoyButtonInfo> releasedJoyButtons;

		public override void Initialize()
		{
			pressedMouseButtons = new List<Godot.MouseButton>();
			releasedMouseButtons = new List<Godot.MouseButton>();

			pressedKeyboardKeys = new List<KeyInfo>();
			releasedKeyboardKeys = new List<KeyInfo>();

			pressedJoyButtons = new List<JoyButtonInfo>();
			releasedJoyButtons = new List<JoyButtonInfo>();

			interval = new ClockPrecise();

			root = ((Godot.SceneTree) Godot.Engine.GetMainLoop()).Root;

			foreach (var deviceId in Godot.Input.GetConnectedJoypads())
			{
				ConnectGamepad(GetUniqueGamepadId(deviceId));
			}

			this.EventSubscribe<GodotInputEvent>(OnGodotInputEvent, -1000);

			currentMousePosition = root.GetMousePosition();
			previousMousePosition = currentMousePosition;
		}

		private void OnGodotInputEvent(GodotInputEvent inputEvent)
		{
			if (inputEvent.godotEvent is Godot.InputEventMouseButton eventMouseButton)
			{
				if (eventMouseButton.ButtonIndex == Godot.MouseButton.WheelUp)
				{
					mouseWheelDelta = eventMouseButton.Factor;
				}
				else if (eventMouseButton.ButtonIndex == Godot.MouseButton.WheelDown)
				{
					mouseWheelDelta = -eventMouseButton.Factor;
				}
				else if (eventMouseButton.ButtonIndex == Godot.MouseButton.WheelLeft || eventMouseButton.ButtonIndex == Godot.MouseButton.WheelRight)
				{
					// Left/Right wheel ignored
				}
				else // Mouse buttons
				{
					if (eventMouseButton.Pressed)
					{
						pressedMouseButtons.Add(eventMouseButton.ButtonIndex);
					}
					else
					{
						releasedMouseButtons.Add(eventMouseButton.ButtonIndex);
					}
				}
			}
			else if (inputEvent.godotEvent is Godot.InputEventKey eventKey)
			{
				if (eventKey.Unicode != 0)
				{
					strCache += char.ConvertFromUtf32((int) eventKey.Unicode);
				}

				if (!eventKey.Echo)
				{
					if (eventKey.Pressed)
					{
						pressedKeyboardKeys.Add(new KeyInfo() { key = eventKey.Keycode, location = eventKey.Location });
					}
					else
					{
						releasedKeyboardKeys.Add(new KeyInfo() { key = eventKey.Keycode, location = eventKey.Location });
					}
				}
			}
			else if (inputEvent.godotEvent is Godot.InputEventJoypadButton eventJoypadButton)
			{
				if (eventJoypadButton.Pressed)
				{
					pressedJoyButtons.Add(new JoyButtonInfo() { device = eventJoypadButton.Device, button = eventJoypadButton.ButtonIndex });
				}
				else
				{
					releasedJoyButtons.Add(new JoyButtonInfo() { device = eventJoypadButton.Device, button = eventJoypadButton.ButtonIndex });
				}
			}
		}

		public override void Update()
		{
			previousMousePosition = currentMousePosition;
			currentMousePosition = root.GetMousePosition();

			mouseWheelDelta = 0f;

			pressedMouseButtons.Clear();
			releasedMouseButtons.Clear();

			pressedKeyboardKeys.Clear();
			releasedKeyboardKeys.Clear();

			pressedJoyButtons.Clear();
			releasedJoyButtons.Clear();

			if (interval.GetElapsedTime().seconds < UPDATE_INTERVAL)
			{
				return;
			}

			interval.Reset();

			if (strCache.Length > 100)
			{
				strCache = string.Empty;
			}

			foreach (var deviceId in Godot.Input.GetConnectedJoypads())
			{
				ConnectGamepad(GetUniqueGamepadId(deviceId));
			}

			foreach (var gamepad in gamepads)
			{
				if (!IEnumerableExtensions.Any(Godot.Input.GetConnectedJoypads(), deviceId => GetUniqueGamepadId(deviceId) == gamepad.Id))
				{
					DisconnectGamepad(gamepad.Id);
				}
			}
		}

		private string GetUniqueGamepadId(int deviceId)
		{
			return $"ID{deviceId}_{Godot.Input.GetJoyName(deviceId)}";
		}

		public override bool GetMousePressedRaw(MouseButton button)
		{
			if (button == MouseButton.None)
			{
				return false;
			}

			return pressedMouseButtons.Contains(GetMouseButtonMapping(button));
		}

		public override bool GetMouseHeldRaw(MouseButton button)
		{
			if (button == MouseButton.None)
			{
				return false;
			}

			return Godot.Input.IsMouseButtonPressed(GetMouseButtonMapping(button));
		}

		public override bool GetMouseReleasedRaw(MouseButton button)
		{
			if (button == MouseButton.None)
			{
				return false;
			}

			return releasedMouseButtons.Contains(GetMouseButtonMapping(button));
		}

		private Godot.MouseButton GetMouseButtonMapping(MouseButton button)
		{
			Godot.MouseButton result = Godot.MouseButton.None;

			switch (button)
			{
				case MouseButton.Left:
					result = Godot.MouseButton.Left;
					break;
				case MouseButton.Middle:
					result = Godot.MouseButton.Middle;
					break;
				case MouseButton.Right:
					result = Godot.MouseButton.Right;
					break;
				case MouseButton.Button4:
				case MouseButton.Button6:
					result = Godot.MouseButton.Xbutton1; // Back
					break;
				case MouseButton.Button5:
				case MouseButton.Button7:
					result = Godot.MouseButton.Xbutton2; // Forward
					break;
			}

			return result;
		}

		public override void GetMousePosition(out int mouseX, out int mouseY)
		{
			mouseX = (int) currentMousePosition.X;
			mouseY = (int) currentMousePosition.Y;
		}

		public override void GetMousePositionDelta(out float mouseX, out float mouseY)
		{
			var delta = (currentMousePosition - previousMousePosition);

			mouseX = delta.X;
			mouseY = delta.Y;
		}

		public override float GetMouseWheelDelta()
		{
			return mouseWheelDelta;
		}

		public override MouseLockMode GetMouseCurrentLockMode()
		{
			var result = MouseLockMode.None;
			var currentMode = Godot.Input.GetMouseMode();

			switch (currentMode)
			{
				case Godot.Input.MouseModeEnum.Visible:
				case Godot.Input.MouseModeEnum.Hidden:
					result = MouseLockMode.None;
					break;
				case Godot.Input.MouseModeEnum.Confined:
				case Godot.Input.MouseModeEnum.ConfinedHidden:
					result = MouseLockMode.Confined;
					break;
				case Godot.Input.MouseModeEnum.Captured:
					result = MouseLockMode.Locked;
					break;
			}

			return result;
		}

		public override bool GetMouseVisibleStatus()
		{
			var result = true;
			var currentMode = Godot.Input.GetMouseMode();

			switch (currentMode)
			{
				case Godot.Input.MouseModeEnum.Visible:
				case Godot.Input.MouseModeEnum.Confined:
					result = true;
					break;
				case Godot.Input.MouseModeEnum.Hidden:
				case Godot.Input.MouseModeEnum.ConfinedHidden:
				case Godot.Input.MouseModeEnum.Captured:
					result = false;
					break;
			}

			return result;
		}

		public override void SetMouseLockMode(MouseLockMode mouseLockMode)
		{
			switch (mouseLockMode)
			{
				case MouseLockMode.None:
					if (GetMouseVisibleStatus())
					{
						Godot.Input.SetMouseMode(Godot.Input.MouseModeEnum.Visible);
					}
					else
					{
						Godot.Input.SetMouseMode(Godot.Input.MouseModeEnum.Hidden);
					}
					break;
				case MouseLockMode.Confined:
					if (GetMouseVisibleStatus())
					{
						Godot.Input.SetMouseMode(Godot.Input.MouseModeEnum.Confined);
					}
					else
					{
						Godot.Input.SetMouseMode(Godot.Input.MouseModeEnum.ConfinedHidden);
					}
					break;
				case MouseLockMode.Locked:
					Godot.Input.SetMouseMode(Godot.Input.MouseModeEnum.Captured);
					break;
			}
		}

		public override void SetMouseCursorVisible(bool visible)
		{
			var currentMode = Godot.Input.GetMouseMode();

			if (visible) // Show
			{
				if (currentMode == Godot.Input.MouseModeEnum.ConfinedHidden)
				{
					Godot.Input.SetMouseMode(Godot.Input.MouseModeEnum.Confined);
				}
				else
				{
					// This will unlock the mouse if it was Captured
					Godot.Input.SetMouseMode(Godot.Input.MouseModeEnum.Visible);
				}
			}
			else // Hide
			{
				if (currentMode == Godot.Input.MouseModeEnum.Confined)
				{
					Godot.Input.SetMouseMode(Godot.Input.MouseModeEnum.ConfinedHidden);
				}
				else if (currentMode == Godot.Input.MouseModeEnum.Visible)
				{
					Godot.Input.SetMouseMode(Godot.Input.MouseModeEnum.Hidden);
				}
			}
		}

		public override bool GetKeyboardPressedRaw(KeyboardKey key)
		{
			if (key == KeyboardKey.None)
			{
				return false;
			}

			var keyCode = GetKeyboardKeyMapping(key);

			if (keyCode == Godot.Key.None)
			{
				return false;
			}

			// TODO: Ignored location (https://github.com/godotengine/godot/pull/80231)
			return IEnumerableExtensions.Any(pressedKeyboardKeys, k => k.key == keyCode);
		}

		public override bool GetKeyboardHeldRaw(KeyboardKey key)
		{
			if (key == KeyboardKey.None)
			{
				return false;
			}

			var keyCode = GetKeyboardKeyMapping(key);

			if (keyCode == Godot.Key.None)
			{
				return false;
			}

			// TODO: Ignored location (https://github.com/godotengine/godot/pull/80231)
			return Godot.Input.IsPhysicalKeyPressed(keyCode);
		}

		public override bool GetKeyboardReleasedRaw(KeyboardKey key)
		{
			if (key == KeyboardKey.None)
			{
				return false;
			}

			var keyCode = GetKeyboardKeyMapping(key);

			if (keyCode == Godot.Key.None)
			{
				return false;
			}

			// TODO: Ignored location (https://github.com/godotengine/godot/pull/80231)
			return IEnumerableExtensions.Any(releasedKeyboardKeys, k => k.key == keyCode);
		}

		private Godot.Key GetKeyboardKeyMapping(KeyboardKey key)
		{
			var result = Godot.Key.None;

			switch (key)
			{
				case KeyboardKey.Backspace:
					result = Godot.Key.Backspace;
					break;
				case KeyboardKey.Tab:
					result = Godot.Key.Tab;
					break;
				case KeyboardKey.Return:
					result = Godot.Key.Enter;
					break;
				case KeyboardKey.Pause:
					result = Godot.Key.Pause;
					break;
				case KeyboardKey.Escape:
					result = Godot.Key.Escape;
					break;
				case KeyboardKey.Space:
					result = Godot.Key.Space;
					break;
				case KeyboardKey.Quote:                     // ' (Apostrophe)
					result = Godot.Key.Apostrophe;
					break;
				case KeyboardKey.Comma:                     // ,
					result = Godot.Key.Comma;
					break;
				case KeyboardKey.Minus:                     // -
					result = Godot.Key.Minus;
					break;
				case KeyboardKey.Period:                    // .
					result = Godot.Key.Period;
					break;
				case KeyboardKey.Slash:                     // /
					result = Godot.Key.Slash;
					break;
				case KeyboardKey.Semicolon:                 // ;
					result = Godot.Key.Semicolon;
					break;
				case KeyboardKey.Equals:                    // =
					result = Godot.Key.Equal;
					break;
				case KeyboardKey.LeftBracket:               // [
					result = Godot.Key.Bracketleft;
					break;
				case KeyboardKey.Backslash:                 // \
					result = Godot.Key.Backslash;
					break;
				case KeyboardKey.RightBracket:              // ]
					result = Godot.Key.Bracketright;
					break;
				case KeyboardKey.BackQuote:                 // ` (Grave accent)
					result = Godot.Key.Quoteleft;
					break;
				case KeyboardKey.Num0:
					result = Godot.Key.Key0;
					break;
				case KeyboardKey.Num1:
					result = Godot.Key.Key1;
					break;
				case KeyboardKey.Num2:
					result = Godot.Key.Key2;
					break;
				case KeyboardKey.Num3:
					result = Godot.Key.Key3;
					break;
				case KeyboardKey.Num4:
					result = Godot.Key.Key4;
					break;
				case KeyboardKey.Num5:
					result = Godot.Key.Key5;
					break;
				case KeyboardKey.Num6:
					result = Godot.Key.Key6;
					break;
				case KeyboardKey.Num7:
					result = Godot.Key.Key7;
					break;
				case KeyboardKey.Num8:
					result = Godot.Key.Key8;
					break;
				case KeyboardKey.Num9:
					result = Godot.Key.Key9;
					break;
				case KeyboardKey.A:
					result = Godot.Key.A;
					break;
				case KeyboardKey.B:
					result = Godot.Key.B;
					break;
				case KeyboardKey.C:
					result = Godot.Key.C;
					break;
				case KeyboardKey.D:
					result = Godot.Key.D;
					break;
				case KeyboardKey.E:
					result = Godot.Key.E;
					break;
				case KeyboardKey.F:
					result = Godot.Key.F;
					break;
				case KeyboardKey.G:
					result = Godot.Key.G;
					break;
				case KeyboardKey.H:
					result = Godot.Key.H;
					break;
				case KeyboardKey.I:
					result = Godot.Key.I;
					break;
				case KeyboardKey.J:
					result = Godot.Key.J;
					break;
				case KeyboardKey.K:
					result = Godot.Key.K;
					break;
				case KeyboardKey.L:
					result = Godot.Key.L;
					break;
				case KeyboardKey.M:
					result = Godot.Key.M;
					break;
				case KeyboardKey.N:
					result = Godot.Key.N;
					break;
				case KeyboardKey.O:
					result = Godot.Key.O;
					break;
				case KeyboardKey.P:
					result = Godot.Key.P;
					break;
				case KeyboardKey.Q:
					result = Godot.Key.Q;
					break;
				case KeyboardKey.R:
					result = Godot.Key.R;
					break;
				case KeyboardKey.S:
					result = Godot.Key.S;
					break;
				case KeyboardKey.T:
					result = Godot.Key.T;
					break;
				case KeyboardKey.U:
					result = Godot.Key.U;
					break;
				case KeyboardKey.V:
					result = Godot.Key.V;
					break;
				case KeyboardKey.W:
					result = Godot.Key.W;
					break;
				case KeyboardKey.X:
					result = Godot.Key.X;
					break;
				case KeyboardKey.Y:
					result = Godot.Key.Y;
					break;
				case KeyboardKey.Z:
					result = Godot.Key.Z;
					break;
				case KeyboardKey.Keypad0:
					result = Godot.Key.Kp0;
					break;
				case KeyboardKey.Keypad1:
					result = Godot.Key.Kp1;
					break;
				case KeyboardKey.Keypad2:
					result = Godot.Key.Kp2;
					break;
				case KeyboardKey.Keypad3:
					result = Godot.Key.Kp3;
					break;
				case KeyboardKey.Keypad4:
					result = Godot.Key.Kp4;
					break;
				case KeyboardKey.Keypad5:
					result = Godot.Key.Kp5;
					break;
				case KeyboardKey.Keypad6:
					result = Godot.Key.Kp6;
					break;
				case KeyboardKey.Keypad7:
					result = Godot.Key.Kp7;
					break;
				case KeyboardKey.Keypad8:
					result = Godot.Key.Kp8;
					break;
				case KeyboardKey.Keypad9:
					result = Godot.Key.Kp9;
					break;
				case KeyboardKey.KeypadPeriod:
					result = Godot.Key.KpPeriod;
					break;
				case KeyboardKey.KeypadDivide:
					result = Godot.Key.KpDivide;
					break;
				case KeyboardKey.KeypadMultiply:
					result = Godot.Key.KpMultiply;
					break;
				case KeyboardKey.KeypadMinus:
					result = Godot.Key.KpSubtract;
					break;
				case KeyboardKey.KeypadPlus:
					result = Godot.Key.KpAdd;
					break;
				case KeyboardKey.KeypadEquals:
					//result = Godot.Key.KpEqual;           // No equivalent in Godot
					break;
				case KeyboardKey.KeypadEnter:
					result = Godot.Key.KpEnter;
					break;
				case KeyboardKey.UpArrow:
					result = Godot.Key.Up;
					break;
				case KeyboardKey.DownArrow:
					result = Godot.Key.Down;
					break;
				case KeyboardKey.RightArrow:
					result = Godot.Key.Right;
					break;
				case KeyboardKey.LeftArrow:
					result = Godot.Key.Left;
					break;
				case KeyboardKey.Delete:
					result = Godot.Key.Delete;
					break;
				case KeyboardKey.Insert:
					result = Godot.Key.Insert;
					break;
				case KeyboardKey.Home:
					result = Godot.Key.Home;
					break;
				case KeyboardKey.End:
					result = Godot.Key.End;
					break;
				case KeyboardKey.PageUp:
					result = Godot.Key.Pageup;
					break;
				case KeyboardKey.PageDown:
					result = Godot.Key.Pagedown;
					break;
				case KeyboardKey.F1:
					result = Godot.Key.F1;
					break;
				case KeyboardKey.F2:
					result = Godot.Key.F2;
					break;
				case KeyboardKey.F3:
					result = Godot.Key.F3;
					break;
				case KeyboardKey.F4:
					result = Godot.Key.F4;
					break;
				case KeyboardKey.F5:
					result = Godot.Key.F5;
					break;
				case KeyboardKey.F6:
					result = Godot.Key.F6;
					break;
				case KeyboardKey.F7:
					result = Godot.Key.F7;
					break;
				case KeyboardKey.F8:
					result = Godot.Key.F8;
					break;
				case KeyboardKey.F9:
					result = Godot.Key.F9;
					break;
				case KeyboardKey.F10:
					result = Godot.Key.F10;
					break;
				case KeyboardKey.F11:
					result = Godot.Key.F11;
					break;
				case KeyboardKey.F12:
					result = Godot.Key.F12;
					break;
				case KeyboardKey.Numlock:
					result = Godot.Key.Numlock;
					break;
				case KeyboardKey.CapsLock:
					result = Godot.Key.Capslock;
					break;
				case KeyboardKey.ScrollLock:
					result = Godot.Key.Scrolllock;
					break;
				case KeyboardKey.RightShift:
					result = Godot.Key.Shift;               // Godot does not differentiate between left/right here
					break;
				case KeyboardKey.LeftShift:
					result = Godot.Key.Shift;
					break;
				case KeyboardKey.RightControl:
					result = Godot.Key.Ctrl;                // Godot does not differentiate between left/right here
					break;
				case KeyboardKey.LeftControl:
					result = Godot.Key.Ctrl;
					break;
				case KeyboardKey.RightAlt:
					result = Godot.Key.Alt;                 // Godot does not differentiate between left/right here
					break;
				case KeyboardKey.LeftAlt:
					result = Godot.Key.Alt;
					break;
				case KeyboardKey.RightCommand:
					result = Godot.Key.Meta;                // Godot does not differentiate between left/right here
					break;
				case KeyboardKey.LeftCommand:
					result = Godot.Key.Meta;
					break;
				case KeyboardKey.LeftWindows:
					result = Godot.Key.Meta;
					break;
				case KeyboardKey.RightWindows:
					result = Godot.Key.Meta;                // Godot does not differentiate between left/right
					break;
				case KeyboardKey.AltGr:
					//result = Godot.Key.AltGr;             // No equivalent in Godot (is the same as pressing at the same time Ctrl + Alt)
					break;
				case KeyboardKey.Print:
					result = Godot.Key.Print;
					break;
				case KeyboardKey.Menu:
					result = Godot.Key.Menu;
					break;
				case KeyboardKey.OEM1:
					//result = Godot.Key.OEM1;              // No equivalent in Godot
					break;
				case KeyboardKey.OEM2:
					//result = Godot.Key.OEM2;              // No equivalent in Godot
					break;
				case KeyboardKey.OEM3:
					//result = Godot.Key.OEM3;              // No equivalent in Godot
					break;
				case KeyboardKey.OEM4:
					//result = Godot.Key.OEM4;              // No equivalent in Godot
					break;
				case KeyboardKey.OEM5:
					//result = Godot.Key.OEM5;              // No equivalent in Godot
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
			if (button == GamepadButtonRaw.None || button == GamepadButtonRaw.Button_9 || (int) button >= (int) GamepadButtonRaw.Button_12)
			{
				return false;
			}

			var deviceId = GetGamepadDeviceId(gamepad);

			if (deviceId < 0)
			{
				return false;
			}

			var godotJoyButton = GetGamepadButtonMapping(button);

			return IEnumerableExtensions.Any(pressedJoyButtons, b => b.device == deviceId && b.button == godotJoyButton);
		}

		public override bool GetGamepadButtonHeldRaw(IGamepadController gamepad, GamepadButton button)
		{
			return GetGamepadButtonHeldRaw(gamepad, GetGamepadButtonRawMapping(button));
		}

		public override bool GetGamepadButtonHeldRaw(IGamepadController gamepad, GamepadButtonRaw button)
		{
			if (button == GamepadButtonRaw.None || button == GamepadButtonRaw.Button_9 || (int) button >= (int) GamepadButtonRaw.Button_12)
			{
				return false;
			}

			var deviceId = GetGamepadDeviceId(gamepad);

			if (deviceId < 0)
			{
				return false;
			}

			return Godot.Input.IsJoyButtonPressed(deviceId, GetGamepadButtonMapping(button));
		}

		public override bool GetGamepadButtonReleasedRaw(IGamepadController gamepad, GamepadButton button)
		{
			return GetGamepadButtonReleasedRaw(gamepad, GetGamepadButtonRawMapping(button));
		}

		public override bool GetGamepadButtonReleasedRaw(IGamepadController gamepad, GamepadButtonRaw button)
		{
			if (button == GamepadButtonRaw.None || button == GamepadButtonRaw.Button_9 || (int) button >= (int) GamepadButtonRaw.Button_12)
			{
				return false;
			}

			var deviceId = GetGamepadDeviceId(gamepad);

			if (deviceId < 0)
			{
				return false;
			}

			var godotJoyButton = GetGamepadButtonMapping(button);

			return IEnumerableExtensions.Any(releasedJoyButtons, b => b.device == deviceId && b.button == godotJoyButton);
		}

		private int GetGamepadDeviceId(IGamepadController gamepad)
		{
			foreach (int deviceId in Godot.Input.GetConnectedJoypads())
			{
				if (GetUniqueGamepadId(deviceId) == gamepad.Id)
				{
					return deviceId;
				}
			}

			return -1;
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

		private Godot.JoyButton GetGamepadButtonMapping(GamepadButtonRaw button)
		{
			var result = Godot.JoyButton.Invalid;

			switch (button)
			{
				case GamepadButtonRaw.Button_1:
					result = Godot.JoyButton.A;
					break;
				case GamepadButtonRaw.Button_2:
					result = Godot.JoyButton.B;
					break;
				case GamepadButtonRaw.Button_3:
					result = Godot.JoyButton.X;
					break;
				case GamepadButtonRaw.Button_4:
					result = Godot.JoyButton.Y;
					break;
				case GamepadButtonRaw.Button_5:
					result = Godot.JoyButton.LeftShoulder;
					break;
				case GamepadButtonRaw.Button_6:
					result = Godot.JoyButton.RightShoulder;
					break;
				case GamepadButtonRaw.Button_7:
					result = Godot.JoyButton.Back;
					break;
				case GamepadButtonRaw.Button_8:
					result = Godot.JoyButton.Start;
					break;
				case GamepadButtonRaw.Button_10:
					result = Godot.JoyButton.LeftStick;
					break;
				case GamepadButtonRaw.Button_11:
					result = Godot.JoyButton.RightStick;
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

			var deviceId = GetGamepadDeviceId(gamepad);

			if (deviceId >= 0)
			{
				switch (axis)
				{
					case GamepadAxisRaw.AxisX:
						result = Godot.Input.GetJoyAxis(deviceId, Godot.JoyAxis.LeftX);
						break;
					case GamepadAxisRaw.AxisY:
						result = Godot.Input.GetJoyAxis(deviceId, Godot.JoyAxis.LeftY);
						break;
					case GamepadAxisRaw.Axis4:
						result = Godot.Input.GetJoyAxis(deviceId, Godot.JoyAxis.RightX);
						break;
					case GamepadAxisRaw.Axis5:
						result = Godot.Input.GetJoyAxis(deviceId, Godot.JoyAxis.RightY);
						break;
					case GamepadAxisRaw.Axis3:
						result = Godot.Input.GetJoyAxis(deviceId, Godot.JoyAxis.TriggerLeft);
						break;
					case GamepadAxisRaw.Axis6:
						result = Godot.Input.GetJoyAxis(deviceId, Godot.JoyAxis.TriggerRight);
						break;
					case GamepadAxisRaw.Axis7: // Dpad-X
						result = 0;
						break;
					case GamepadAxisRaw.Axis8: // Dpad-Y
						result = 0;
						break;
				}
			}

			return result;
		}

		public override void GetGamepadAxisRawRaw(IGamepadController gamepad, GamepadAxis axis, out float axisX, out float axisY)
		{
			var axisValue = Godot.Vector2.Zero;

			if (axis == GamepadAxis.None)
			{
				axisX = 0f;
				axisY = 0f;

				return;
			}

			switch (axis)
			{
				case GamepadAxis.D_Pad:
					axisValue.X = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.D_Pad_Left));
					axisValue.Y = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.D_Pad_Top));
					break;
				case GamepadAxis.L_Stick:
					axisValue.X = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.L_Stick_Left));
					axisValue.Y = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.L_Stick_Top));
					break;
				case GamepadAxis.R_Stick:
					axisValue.X = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.R_Stick_Left));
					axisValue.Y = GetGamepadAxisRaw(gamepad, GetGamepadAxisRawMapping(GamepadAxis.R_Stick_Top));
					break;
			}

			axisX = axisValue.X;
			axisY = axisValue.Y;
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
