// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

namespace Ju.Services
{
	public struct GodotAppFocusEvent : ISerializableEvent
	{
		public bool hasFocus;

		public GodotAppFocusEvent(bool hasFocus)
		{
			this.hasFocus = hasFocus;
		}

		public string Serialize()
		{
			return string.Format("[ hasFocus: {0} ]", hasFocus);
		}
	}
}

#endif
