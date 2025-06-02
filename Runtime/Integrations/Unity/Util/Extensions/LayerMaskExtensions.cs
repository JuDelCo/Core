// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using UnityEngine;

namespace Ju.Extensions
{
	public static class LayerMaskExtensions
	{
		public static bool IsInLayerMask(this Component component, LayerMask layerMask)
		{
			return component.gameObject.IsInLayerMask(layerMask);
		}

		public static bool IsInLayerMask(this GameObject obj, LayerMask layerMask)
		{
			return ((layerMask.value & (1 << obj.layer)) > 0);
		}
	}
}

#endif
