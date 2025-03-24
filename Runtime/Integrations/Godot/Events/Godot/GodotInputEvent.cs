// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;

namespace Ju.Services
{
	public struct GodotInputEvent : ISerializableEvent
	{
		public InputEvent godotEvent;

		public GodotInputEvent(InputEvent godotEvent)
		{
			this.godotEvent = godotEvent;
		}

		public string Serialize()
		{
			return string.Format("[ {0} ]", godotEvent.AsText());
		}
	}
}

#endif
