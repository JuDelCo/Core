// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System.Collections.Generic;

namespace Ju.Input
{
	public interface IInputPlayer
	{
		string Id { get; }

		IEnumerable<IInputAction> Actions { get; }
		IEnumerable<IController> Controllers { get; }

		IInputAction AddAction(string actionId);
		IInputAction GetAction(string actionId);
		void RemoveAction(IInputAction action);

		void BindController(IController controller);
		void UnbindController(IController controller);
	}
}
