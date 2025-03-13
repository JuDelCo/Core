// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Extensions;
using Ju.Services;

namespace Ju.Input
{
	public class KeyboardController : IKeyboardController
	{
		public string Id => "keyboard";

		private readonly IInputServiceRaw input;

		public KeyboardController(IInputServiceRaw input)
		{
			this.input = input;
		}

		public bool IsAnyKeyPressed()
		{
			var keys = Enum.GetValues(typeof(KeyboardKey)).Cast<KeyboardKey>();

			foreach (var key in keys)
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
			var keys = Enum.GetValues(typeof(KeyboardKey)).Cast<KeyboardKey>();

			foreach (var key in keys)
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
