// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;
using Ju.Extensions;
using Ju.Input;
using Ju.Services.Extensions;
using Ju.Services.Internal;
using Ju.Time;

namespace Ju.Services
{
	public abstract class InputService : IInputServiceRaw, IInputService, IServiceLoad
	{
		public IEnumerable<IInputPlayer> Players => players;
		public IMouseController Mouse => mouse;
		public IKeyboardController Keyboard => keyboard;
		public IEnumerable<IGamepadController> Gamepads => gamepads;
		public IEnumerable<IGamepadController> CustomControllers => customControllers;

		protected List<IInputPlayer> players = new List<IInputPlayer>();
		protected IMouseController mouse;
		protected IKeyboardController keyboard;
		protected List<IGamepadController> gamepads = new List<IGamepadController>();
		protected List<IGamepadController> customControllers = new List<IGamepadController>();

		public void Load()
		{
			mouse = new MouseController(this);
			keyboard = new KeyboardController(this);

			Initialize();

			this.EventSubscribe<TimePreUpdateEvent>(e =>
			{
				foreach (var player in players)
				{
					var actions = (List<IInputAction>)player.Actions;

					foreach (InputAction action in actions)
					{
						action.Update(e.DeltaTime);
					}
				}

				Update();
			});
		}

		protected void ConnectGamepad(string gamepadId)
		{
			if (string.IsNullOrEmpty(gamepadId))
			{
				return;
			}

			if (!gamepads.Any(g => g.Id == gamepadId))
			{
				var gamepad = new GamepadController(gamepadId, this);
				gamepads.Add(gamepad);

				ServiceCache.EventBus.Fire(new InputGamepadConnectedEvent(gamepad));
			}
			else
			{
				var gamepad = gamepads.Find(g => g.Id == gamepadId) as GamepadController;

				if (!gamepad.Enabled)
				{
					gamepad.Enabled = true;

					ServiceCache.EventBus.Fire(new InputGamepadConnectedEvent(gamepad));
				}
			}
		}

		protected void DisconnectGamepad(string gamepadId)
		{
			var gamepad = gamepads.Find(g => g.Id == gamepadId) as GamepadController;

			if (gamepad.Enabled)
			{
				gamepad.Enabled = false;

				ServiceCache.EventBus.Fire(new InputGamepadDisconnectedEvent(gamepad));
			}
		}

		public IInputPlayer AddPlayer(string playerId)
		{
			IInputPlayer result = null;

			foreach (var player in players)
			{
				if (player.Id == playerId)
				{
					result = player;
					break;
				}
			}

			if (result == null)
			{
				result = new InputPlayer(playerId);
				players.Add(result);
			}

			return result;
		}

		public void RemovePlayer(IInputPlayer player)
		{
			players.Remove(player);
		}

		public void AddCustomController(IGamepadController controller)
		{
			if (!customControllers.Contains(controller))
			{
				customControllers.Add(controller);
			}
		}

		public void RemoveCustomController(IGamepadController controller)
		{
			customControllers.Remove(controller);
		}

		public bool IsAnyPressed()
		{
			return mouse.IsAnyButtonPressed() || keyboard.IsAnyKeyPressed() || gamepads.Any(g => g.IsAnyButtonPressed());
		}
	}
}
