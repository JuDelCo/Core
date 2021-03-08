// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using UnityEngine;

namespace Ju.Random
{
	public partial class RandomGenerator
	{
		public Vector2Int Vector2i(Vector2Int min, Vector2Int max)
		{
			return Random.Vector2i(min, max, random);
		}

		public Vector2 Direction2D()
		{
			return Random.Direction2D(random);
		}

		public Vector2 Vector2f()
		{
			return Random.Vector2f(random);
		}

		public Vector2 Vector2f(Vector2 min, Vector2 max)
		{
			return Random.Vector2f(min, max, random);
		}

		public Vector3Int Vector3i(Vector3Int min, Vector3Int max)
		{
			return Random.Vector3i(min, max, random);
		}

		public Vector3 Direction3D()
		{
			return Random.Direction3D(random);
		}

		public Vector3 Vector3f()
		{
			return Random.Vector3f(random);
		}

		public Vector3 Vector3f(Vector3 min, Vector3 max)
		{
			return Random.Vector3f(min, max, random);
		}

		//public Vector4Int Vector4i(Vector4Int min, Vector4Int max)
		//{
		//	return Random.Vector4i(min, max, random);
		//}

		public Vector4 Vector4f()
		{
			return Random.Vector4f(random);
		}

		public Vector4 Vector4f(Vector4 min, Vector4 max)
		{
			return Random.Vector4f(min, max, random);
		}

		public Quaternion DirectionQuat()
		{
			return Random.DirectionQuat(random);
		}

		public Quaternion Quat()
		{
			return Random.Quat(random);
		}
	}
}

#endif
