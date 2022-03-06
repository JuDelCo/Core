// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System.Collections.Generic;
using Ju.Extensions;
using Ju.Util;
using UnityEngine;
using Prefab = UnityEngine.GameObject;
using GameObject = UnityEngine.GameObject;

namespace Ju.Services
{
	public class PrefabPoolService : IPrefabPoolService, IServiceLoad, IServiceUnload
	{
		[System.Serializable] internal class GameObjectList : List<GameObject> { }
		[System.Serializable] internal class PrefabPools : SerializableDictionary<Prefab, GameObjectList> { }

		private GameObject container;
		[SerializeReference]
		private PrefabPools pools = new PrefabPools();

		public void Load()
		{
			if (container == null)
			{
				container = new GameObject("GameObjectPool");
				container.hideFlags = HideFlags.NotEditable;
				container.SetActive(false);
				GameObject.DontDestroyOnLoad(container);
			}
		}

		public void Unload()
		{
			ClearAll(true);
			GameObject.Destroy(container);
		}

		public void SetCapacity(Prefab prefab, int capacity)
		{
			pools.GetOrInsertNew(prefab).Capacity = capacity;
		}

		public void WarmupCapacity(Prefab prefab)
		{
			var pool = pools.GetOrInsertNew(prefab);
			var capacity = pool.Capacity;

			for (int i = pool.Count; i < capacity; ++i)
			{
				var target = Object.Instantiate(prefab, container.transform, false);
				target.SetActive(false);
				pool.Add(target);
			}
		}

		public GameObject Spawn(Prefab prefab, Transform parent, Vector3 position, Quaternion rotation)
		{
			var pool = pools.GetOrInsertNew(prefab);
			var target = pool.Find(go => !go.activeInHierarchy && !go.activeSelf);

			if (target == null)
			{
				target = Object.Instantiate(prefab);
				pool.Add(target);
			}

			target.transform.SetParent(parent, false);
			target.transform.position = position;
			target.transform.rotation = rotation;

			if (parent == null)
			{
				UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(target, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
			}

			target.SetActive(true);

			return target;
		}

		public void Recycle(GameObject target)
		{
			List<GameObject> pool = null;

			foreach (var kvp in pools)
			{
				var prefabPool = kvp.Value;

				if (prefabPool.Contains(target))
				{
					pool = prefabPool;
					break;
				}
			}

			if (pool != null)
			{
				target.SetActive(false);
				target.transform.SetParent(container.transform, false);
			}
			else
			{
				GameObject.Destroy(target);
			}
		}

		public int Count(Prefab prefab, bool includeActive = false)
		{
			var total = 0;

			if (pools.ContainsKey(prefab))
			{
				var pool = pools[prefab];

				foreach (var go in pool)
				{
					if (includeActive || (!go.activeInHierarchy && !go.activeSelf))
					{
						++total;
					}
				}

				return pools[prefab].Count;
			}

			return total;
		}

		public int CountAll(bool includeActive = false)
		{
			var total = 0;

			foreach (var kvp in pools)
			{
				total += Count(kvp.Key, includeActive);
			}

			return total;
		}

		public void Shrink(Prefab prefab, int maxSize, bool clearSpawned = false)
		{
			if (!pools.ContainsKey(prefab))
			{
				return;
			}

			var pool = pools[prefab];
			var total = clearSpawned ? pool.Count : Count(prefab);

			pool.ForEachReverse(go =>
			{
				if (total <= maxSize)
				{
					return;
				}

				if (!go.activeInHierarchy && !go.activeSelf)
				{
					GameObject.Destroy(go);
					pool.Remove(go);
					--total;
				}
			});

			if (clearSpawned && total > maxSize)
			{
				pool.ForEachReverse(go =>
				{
					if (total <= maxSize)
					{
						return;
					}

					GameObject.Destroy(go);
					pool.Remove(go);
					--total;
				});
			}
		}

		public void Clear(Prefab prefab, bool clearSpawned = false)
		{
			if (!pools.ContainsKey(prefab))
			{
				return;
			}

			var pool = pools[prefab];

			pool.ForEachReverse(go =>
			{
				if (clearSpawned || (!go.activeInHierarchy && !go.activeSelf))
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						GameObject.Destroy(go);
					}
					else
					{
						GameObject.DestroyImmediate(go);
					}
#else
						GameObject.Destroy(go);
#endif

					pool.Remove(go);
				}
			});
		}

		public void ClearAll(bool clearSpawned = false)
		{
			foreach (var kvp in pools)
			{
				Clear(kvp.Key, clearSpawned);
			}
		}
	}
}

#endif
