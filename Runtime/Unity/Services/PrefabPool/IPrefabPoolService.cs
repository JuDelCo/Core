// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using UnityEngine;
using Prefab = UnityEngine.GameObject;
using GameObject = UnityEngine.GameObject;

namespace Ju.Services
{
	public interface IPrefabPoolService
	{
		GameObject Spawn(Prefab prefab, Transform parent, Vector3 position, Quaternion rotation);
		void Recycle(GameObject target);

		int Count(Prefab prefab);
		int CountAll();

		void SetCapacity(Prefab prefab, int capacity);
		void Shrink(Prefab prefab, int maxSize, bool clearSpawned = false);
		void Clear(Prefab prefab, bool clearSpawned = false);
		void ClearAll(bool clearSpawned = false);
	}
}

#endif
