// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using UnityEngine;

namespace Ju.Random
{
	public static partial class Random
	{
		public static Vector2Int Vector2i(Vector2Int min, Vector2Int max, System.Random random = null)
		{
			return min + new Vector2Int(Int(max.x - min.x, random), Int(max.y - min.y, random));
		}

		public static Vector2 Direction2D(System.Random random = null)
		{
			return (new Vector2(Float(-1f, 1f, random), Float(-1f, 1f, random))).normalized;
		}

		public static Vector2 Vector2f(System.Random random = null)
		{
			return (new Vector2(Float(-1f, 1f, random), Float(-1f, 1f, random))).normalized;
		}

		public static Vector2 Vector2f(Vector2 min, Vector2 max, System.Random random = null)
		{
			return min + new Vector2(Float(max.x - min.x, random), Float(max.y - min.y, random));
		}

		public static Vector3Int Vector3i(Vector3Int min, Vector3Int max, System.Random random = null)
		{
			return min + new Vector3Int(Int(max.x - min.x, random), Int(max.y - min.y, random), Int(max.z - min.z, random));
		}

		public static Vector3 Direction3D(System.Random random = null)
		{
			return (new Vector3(Float(-1f, 1f, random), Float(-1f, 1f, random), Float(-1f, 1f, random))).normalized;
		}

		public static Vector3 Vector3f(System.Random random = null)
		{
			return (new Vector3(Float(-1f, 1f, random), Float(-1f, 1f, random), Float(-1f, 1f, random))).normalized;
		}

		public static Vector3 Vector3f(Vector3 min, Vector3 max, System.Random random = null)
		{
			return min + new Vector3(Float(max.x - min.x, random), Float(max.y - min.y, random), Float(max.z - min.z, random));
		}

		//public static Vector4Int Vector4i(Vector4Int min, Vector4Int max, System.Random random = null)
		//{
		//	return min + new Vector4Int(Int(max.x - min.x, random), Int(max.y - min.y, random), Int(max.z - min.z, random), Int(max.w - min.w, random));
		//}

		public static Vector4 Vector4f(System.Random random = null)
		{
			return (new Vector4(Float(-1f, 1f, random), Float(-1f, 1f, random), Float(-1f, 1f, random), Float(-1, 1f, random))).normalized;
		}

		public static Vector4 Vector4f(Vector4 min, Vector4 max, System.Random random = null)
		{
			return min + new Vector4(Float(max.x - min.x, random), Float(max.y - min.y, random), Float(max.z - min.z, random), Float(max.w - min.w, random));
		}

		public static Quaternion DirectionQuat(System.Random random = null)
		{
			return (new Quaternion(Float(-1f, 1f, random), Float(-1f, 1f, random), Float(-1f, 1f, random), Float(-1, 1f, random))).normalized;
		}

		public static Quaternion Quat(System.Random random = null)
		{
			return (new Quaternion(Float(-1f, 1f, random), Float(-1f, 1f, random), Float(-1f, 1f, random), Float(-1, 1f, random))).normalized;
		}
	}
}

#endif
