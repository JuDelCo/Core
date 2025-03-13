// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using UnityEngine;

namespace Ju.Extensions
{
	public static class BehaviourExtensions
	{
		public static Vector3 GetPosition(this Behaviour behaviour)
		{
			return behaviour.transform.position;
		}

		public static Vector3 GetLocalPosition(this Behaviour behaviour)
		{
			return behaviour.transform.localPosition;
		}

		public static Quaternion GetRotation(this Behaviour behaviour)
		{
			return behaviour.transform.rotation;
		}

		public static Quaternion GetLocalRotation(this Behaviour behaviour)
		{
			return behaviour.transform.localRotation;
		}

		public static Vector3 GetScale(this Behaviour behaviour)
		{
			return behaviour.transform.localScale;
		}
	}
}

#endif
