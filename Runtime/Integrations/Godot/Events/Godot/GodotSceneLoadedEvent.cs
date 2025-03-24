// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

namespace Ju.Services
{
	public struct GodotSceneLoadedEvent : ISerializableEvent
	{
		public string sceneName;
		public string scenePath;

		public GodotSceneLoadedEvent(string sceneName, string scenePath)
		{
			this.sceneName = sceneName;
			this.scenePath = scenePath;
		}

		public string Serialize()
		{
			return string.Format("[ name: {0} , path: {1} ]", sceneName, scenePath);
		}
	}
}

#endif
