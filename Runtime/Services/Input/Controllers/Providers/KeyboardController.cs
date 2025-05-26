// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Services;

namespace Ju.Input
{
	public class KeyboardController : IKeyboardController
	{
		public string Id => "keyboard";

		private static readonly KeyboardKey[] keyboardKeyEnum = (KeyboardKey[]) Enum.GetValues(typeof(KeyboardKey));

		private readonly IInputServiceRaw input;

		public KeyboardController(IInputServiceRaw input)
		{
			this.input = input;
		}

		public bool IsAnyKeyPressed()
		{
			foreach (var key in keyboardKeyEnum)
			{
				if (key == KeyboardKey.None)
				{
					continue;
				}

				if (input.GetKeyboardHeldRaw(key))
				{
					return true;
				}
			}

			return false;
		}

		public bool IsKeyPressed(KeyboardKey key)
		{
			return input.GetKeyboardPressedRaw(key);
		}

		public bool IsKeyHeld(KeyboardKey key)
		{
			return input.GetKeyboardHeldRaw(key);
		}

		public bool IsKeyReleased(KeyboardKey key)
		{
			return input.GetKeyboardReleasedRaw(key);
		}

		public KeyboardKey FirstPressedKey()
		{
			foreach (var key in keyboardKeyEnum)
			{
				if (key == KeyboardKey.None)
				{
					continue;
				}

				if (input.GetKeyboardPressedRaw(key))
				{
					return key;
				}
			}

			return KeyboardKey.None;
		}

		public string GetInputString()
		{
			return input.GetKeyboardInputStringRaw();
		}
	}
}
