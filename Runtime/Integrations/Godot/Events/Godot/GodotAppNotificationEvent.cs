// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

namespace Ju.Services
{
	public struct GodotAppNotificationEvent : ISerializableEvent
	{
		public int what;

		public GodotAppNotificationEvent(int what)
		{
			this.what = what;
		}

		public string Serialize()
		{
			return string.Format("[ what: {0} ]", what);
		}
	}
}

#endif
