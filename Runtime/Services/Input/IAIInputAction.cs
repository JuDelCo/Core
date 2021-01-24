// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

namespace Ju.Input
{
	public interface IAIInputAction
	{
		string Id { get; }

		void ResetState();
		void CheckState(float deltaTime);

		void Set(bool value);
		void Set(float value);
		void Set(float valueX, float valueY);
	}
}
