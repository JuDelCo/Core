// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;
using Prefab = Godot.PackedScene;

namespace Ju.Services
{
	public interface INodePoolService
	{
		Node Spawn(Prefab prefab, Node parent, Vector3 position, Quaternion rotation);
		void Recycle(Node target);

		int Count(Prefab prefab, bool includeActive = false);
		int CountAll(bool includeActive = false);

		void SetCapacity(Prefab prefab, int capacity);
		void WarmupCapacity(Prefab prefab);
		void Shrink(Prefab prefab, int maxSize, bool clearSpawned = false);
		void Clear(Prefab prefab, bool clearSpawned = false);
		void ClearAll(bool clearSpawned = false);
	}
}

#endif
