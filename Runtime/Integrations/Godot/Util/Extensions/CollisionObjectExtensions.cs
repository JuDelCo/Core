// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;

namespace Ju.Extensions
{
	public static class LayerMaskExtensions
	{
		public static bool IsInLayerMask(this CollisionObject3D collisionObject, int layerMask)
		{
			return (collisionObject.CollisionLayer & layerMask) != 0;
		}

		public static bool IsInLayerMask(this Node3D node, int layerMask)
		{
			if (node is CollisionObject3D collisionObject)
			{
				return collisionObject.IsInLayerMask(layerMask);
			}

			return false;
		}

		public static bool IsInLayerMask(this CollisionObject2D collisionObject, int layerMask)
		{
			return (collisionObject.CollisionLayer & layerMask) != 0;
		}

		public static bool IsInLayerMask(this Node2D node, int layerMask)
		{
			if (node is CollisionObject2D collisionObject)
			{
				return collisionObject.IsInLayerMask(layerMask);
			}

			return false;
		}
	}
}

#endif
