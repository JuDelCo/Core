// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System.Collections.Generic;

namespace Ju.Input
{
	public class InputPlayer : IInputPlayer
	{
		public string Id { get; }

		public IEnumerable<IInputAction> Actions => actions;
		public IEnumerable<IController> Controllers => controllers;

		private readonly List<IInputAction> actions;
		private readonly List<IController> controllers;

		public InputPlayer(string id)
		{
			Id = id;

			actions = new List<IInputAction>();
			controllers = new List<IController>();
		}

		public IInputAction AddAction(string actionId)
		{
			IInputAction result = GetAction(actionId);

			if (result is null)
			{
				result = new InputAction(this, actionId);
				actions.Add(result);
			}

			return result;
		}

		public IInputAction GetAction(string actionId)
		{
			IInputAction result = null;

			foreach (var action in actions)
			{
				if (action.Id == actionId)
				{
					result = action;
					break;
				}
			}

			return result;
		}

		public void RemoveAction(IInputAction action)
		{
			actions.Remove(action);
		}

		public void BindController(IController controller)
		{
			if (!controllers.Contains(controller))
			{
				controllers.Add(controller);
			}
		}

		public void UnbindController(IController controller)
		{
			controllers.Remove(controller);
		}
	}
}
