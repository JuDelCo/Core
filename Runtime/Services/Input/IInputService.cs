// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;
using Ju.Input;

namespace Ju.Services
{
	public interface IInputService
	{
		IEnumerable<IInputPlayer> Players { get; }
		IMouseController Mouse { get; }
		IKeyboardController Keyboard { get; }
		IEnumerable<IGamepadController> Gamepads { get; }
		IEnumerable<IGamepadController> CustomControllers { get; }

		IInputPlayer AddPlayer(string playerId);
		void RemovePlayer(IInputPlayer player);

		void AddCustomController(IGamepadController controller);
		void RemoveCustomController(IGamepadController controller);

		bool IsAnyPressed();
	}
}
