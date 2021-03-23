// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System.Collections.Generic;
using Ju.Extensions;
using UnityEngine;
using Prefab = UnityEngine.GameObject;
using GameObject = UnityEngine.GameObject;

namespace Ju.Services
{
	public class PrefabPoolService : IPrefabPoolService, IServiceLoad, IServiceUnload
	{
		private GameObject container;
		private Dictionary<Prefab, List<GameObject>> pools;

		public void Load()
		{
			pools = new Dictionary<Prefab, List<GameObject>>();

			container = new GameObject("GameObjectPool");
			container.hideFlags = HideFlags.NotEditable;
			GameObject.DontDestroyOnLoad(container);
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

		public GameObject Spawn(Prefab prefab, Transform parent, Vector3 position, Quaternion rotation)
		{
			var pool = pools.GetOrInsertNew(prefab);
			var target = pool.Find(go => !go.activeInHierarchy && !go.activeSelf);

			if (target == null)
			{
				target = Object.Instantiate(prefab);
				pool.Add(target);
			}

			target.transform.SetParent(parent, true);
			target.transform.position = position;
			target.transform.rotation = rotation;
			target.SetActive(true);

			return target;
		}

		public void Recycle(GameObject target)
		{
			var pool = GetPoolContaining(target);

			if (pool != null)
			{
				target.SetActive(false);
				target.transform.SetParent(container.transform, true);
			}
			else
			{
				GameObject.Destroy(target);
			}
		}

		public int Count(Prefab prefab)
		{
			var total = 0;

			if (pools.ContainsKey(prefab))
			{
				var pool = pools[prefab];

				foreach (var go in pool)
				{
					if (!go.activeInHierarchy && !go.activeSelf)
					{
						++total;
					}
				}

				return pools[prefab].Count;
			}

			return total;
		}

		public int CountAll()
		{
			var total = 0;

			foreach (var kvp in pools)
			{
				total += Count(kvp.Key);
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
					GameObject.Destroy(go);
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

		private List<GameObject> GetPoolContaining(GameObject target)
		{
			foreach (var kvp in pools)
			{
				var pool = kvp.Value;

				if (pool.Contains(target))
				{
					return pool;
				}
			}

			return null;
		}
	}
}

#endif
