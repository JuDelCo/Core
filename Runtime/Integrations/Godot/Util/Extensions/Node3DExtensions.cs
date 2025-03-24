// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using Godot;

namespace Ju.Extensions
{
	public static class Node3DExtensions
	{
		public static Vector3 GetPosition(this Node3D node)
		{
			return node.GlobalPosition;
		}

		public static Vector3 GetLocalPosition(this Node3D node)
		{
			return node.Position;
		}

		public static Quaternion GetRotation(this Node3D node)
		{
			return node.GlobalTransform.Basis.GetRotationQuaternion();
		}

		public static Quaternion GetLocalRotation(this Node3D node)
		{
			return node.Transform.Basis.GetRotationQuaternion();
		}

		public static Vector3 GetScale(this Node3D node)
		{
			return node.Scale;
		}
	}
}

#endif
