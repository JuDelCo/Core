// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using UnityEngine;
using Prefab = UnityEngine.GameObject;
using GameObject = UnityEngine.GameObject;

namespace Ju.Services
{
	public static class IPrefabPoolServiceExtensions
	{
		public static GameObject Spawn(this IPrefabPoolService service, Prefab prefab, Transform parent, Vector3 position)
		{
			return service.Spawn(prefab, parent, position, Quaternion.identity);
		}

		public static GameObject Spawn(this IPrefabPoolService service, Prefab prefab, Transform parent)
		{
			return service.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
		}

		public static GameObject Spawn(this IPrefabPoolService service, Prefab prefab, Vector3 position, Quaternion rotation)
		{
			return service.Spawn(prefab, null, position, rotation);
		}

		public static GameObject Spawn(this IPrefabPoolService service, Prefab prefab, Vector3 position)
		{
			return service.Spawn(prefab, null, position, Quaternion.identity);
		}

		public static GameObject Spawn(this IPrefabPoolService service, Prefab prefab)
		{
			return service.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
		}

		public static T Spawn<T>(this IPrefabPoolService service, T prefabComponent, Transform parent, Vector3 position, Quaternion rotation) where T : Component
		{
			return service.Spawn(prefabComponent.gameObject, parent, position, rotation).GetComponent<T>();
		}

		public static T Spawn<T>(this IPrefabPoolService service, T prefabComponent, Transform parent, Vector3 position) where T : Component
		{
			return service.Spawn(prefabComponent.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
		}

		public static T Spawn<T>(this IPrefabPoolService service, T prefabComponent, Transform parent) where T : Component
		{
			return service.Spawn(prefabComponent.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
		}

		public static T Spawn<T>(this IPrefabPoolService service, T prefabComponent, Vector3 position, Quaternion rotation) where T : Component
		{
			return service.Spawn(prefabComponent.gameObject, null, position, rotation).GetComponent<T>();
		}

		public static T Spawn<T>(this IPrefabPoolService service, T prefabComponent, Vector3 position) where T : Component
		{
			return service.Spawn(prefabComponent.gameObject, null, position, Quaternion.identity).GetComponent<T>();
		}

		public static T Spawn<T>(this IPrefabPoolService service, T prefabComponent) where T : Component
		{
			return service.Spawn(prefabComponent.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
		}

		public static void Recycle<T>(this IPrefabPoolService service, T targetComponent) where T : Component
		{
			service.Recycle(targetComponent.gameObject);
		}

		public static int Count<T>(this IPrefabPoolService service, T prefabComponent, bool includeActive = false) where T : Component
		{
			return service.Count(prefabComponent.gameObject, includeActive);
		}

		public static void SetCapacity<T>(this IPrefabPoolService service, T prefabComponent, int capacity) where T : Component
		{
			service.SetCapacity(prefabComponent.gameObject, capacity);
		}

		public static void WarmupCapacity<T>(this IPrefabPoolService service, T prefabComponent) where T : Component
		{
			service.WarmupCapacity(prefabComponent.gameObject);
		}

		public static void Shrink<T>(this IPrefabPoolService service, T prefabComponent, int maxSize, bool clearSpawned = false) where T : Component
		{
			service.Shrink(prefabComponent.gameObject, maxSize, clearSpawned);
		}

		public static void Clear<T>(this IPrefabPoolService service, T prefabComponent, bool clearSpawned = false) where T : Component
		{
			service.Clear(prefabComponent.gameObject, clearSpawned);
		}
	}
}

#endif
