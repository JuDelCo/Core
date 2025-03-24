// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;

namespace Ju.Extensions
{
	public static class Node2DExtensions
	{
		public static Vector2 GetPosition(this Node2D node)
		{
			return node.GlobalPosition;
		}

		public static Vector2 GetLocalPosition(this Node2D node)
		{
			return node.Position;
		}

		public static float GetRotation(this Node2D node)
		{
			return node.GlobalRotation;
		}

		public static float GetRotationDeg(this Node2D node)
		{
			return node.GlobalRotationDegrees;
		}

		public static float GetLocalRotation(this Node2D node)
		{
			return node.Rotation;
		}

		public static float GetLocalRotationDeg(this Node2D node)
		{
			return node.RotationDegrees;
		}

		public static Vector2 GetScale(this Node2D node)
		{
			return node.Scale;
		}
	}
}

#endif
