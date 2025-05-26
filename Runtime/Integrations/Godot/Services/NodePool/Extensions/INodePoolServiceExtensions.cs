// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;
using Ju.Services;
using Prefab = Godot.PackedScene;

public static class INodePoolServiceExtensions
{
	public static Node Spawn(this INodePoolService service, Prefab prefab, Node parent, Vector3 position)
	{
		return service.Spawn(prefab, parent, position, Quaternion.Identity);
	}

	public static Node Spawn(this INodePoolService service, Prefab prefab, Node parent)
	{
		return service.Spawn(prefab, parent, Vector3.Zero, Quaternion.Identity);
	}

	public static Node Spawn(this INodePoolService service, Prefab prefab, Vector3 position, Quaternion rotation)
	{
		return service.Spawn(prefab, null, position, rotation);
	}

	public static Node Spawn(this INodePoolService service, Prefab prefab, Vector3 position)
	{
		return service.Spawn(prefab, null, position, Quaternion.Identity);
	}

	public static Node Spawn(this INodePoolService service, Prefab prefab)
	{
		return service.Spawn(prefab, null, Vector3.Zero, Quaternion.Identity);
	}
}

#endif
