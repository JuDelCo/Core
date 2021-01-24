// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using UnityEngine;

namespace Ju.Extensions
{
	public static class LayerMaskExtensions
	{
		public static bool IsInLayerMask(this Collision collision, LayerMask layerMask)
		{
			return collision.gameObject.IsInLayerMask(layerMask);
		}

		public static bool IsInLayerMask(this Collider collider, LayerMask layerMask)
		{
			return collider.gameObject.IsInLayerMask(layerMask);
		}

		public static bool IsInLayerMask(this GameObject obj, LayerMask layerMask)
		{
			return ((layerMask.value & (1 << obj.layer)) > 0);
		}
	}
}

#endif
