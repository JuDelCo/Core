// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System.Collections.Generic;
using Ju.Extensions;
using Ju.Util;
using Godot;
using Prefab = Godot.PackedScene;

namespace Ju.Services
{
	public class NodePoolService : INodePoolService, IServiceLoad, IServiceUnload
	{
		[System.Serializable] internal class NodeList : List<Node> { }
		[System.Serializable] internal class PrefabPools : SerializableDictionary<Prefab, NodeList> { }

		private Node container;
		private PrefabPools pools = new PrefabPools();

		public void Load()
		{
			if (container == null)
			{
				container = new Node();
				container.Name = "NodePool";

				var sceneTree = ((SceneTree) Engine.GetMainLoop()).Root;
				sceneTree.CallDeferred(Node.MethodName.AddChild, container);
			}
		}

		public void Unload()
		{
			ClearAll(true);

			if (container != null && GodotObject.IsInstanceValid(container))
			{
				container.QueueFree();
			}
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
				var target = prefab.Instantiate();
				container.AddChild(target);

				target.ProcessMode = Node.ProcessModeEnum.Disabled;

				if (target is Node3D node3D)
				{
					node3D.Hide();
				}
				else if (target is Node2D node2D)
				{
					node2D.Hide();
				}

				pool.Add(target);
			}
		}

		public Node Spawn(Prefab prefab, Node parent, Vector3 position, Quaternion rotation)
		{
			var pool = pools.GetOrInsertNew(prefab);
			var target = pool.Find(node => node.GetParent() == container);

			if (target == null)
			{
				target = prefab.Instantiate();
				pool.Add(target);
			}

			if (parent != null)
			{
				parent.AddChild(target);
			}
			else
			{
				var sceneTree = ((SceneTree) Engine.GetMainLoop()).Root.GetTree();

				if (sceneTree.CurrentScene != null)
				{
					sceneTree.CurrentScene.AddChild(target);
				}
			}

			target.ProcessMode = Node.ProcessModeEnum.Inherit;

			if (target is Node3D node3D)
			{
				node3D.GlobalPosition = position;
				node3D.Quaternion = rotation;
				node3D.Show();
			}
			else if (target is Node2D node2D)
			{
				node2D.GlobalPosition = new Vector2(position.X, position.Y);
				node2D.Rotation = Mathf.Atan2(rotation.Z, rotation.W);
				node2D.Show();
			}

			return target;
		}

		public void Recycle(Node target)
		{
			NodeList pool = null;

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
				container.AddChild(target);

				target.ProcessMode = Node.ProcessModeEnum.Disabled;

				if (target is Node3D node3D)
				{
					node3D.Hide();
				}
				else if (target is Node2D node2D)
				{
					node2D.Hide();
				}
			}
			else
			{
				target.QueueFree();
			}
		}

		public int Count(Prefab prefab, bool includeActive = false)
		{
			var total = 0;

			if (pools.ContainsKey(prefab))
			{
				var pool = pools[prefab];

				foreach (var node in pool)
				{
					if (includeActive || (node.GetParent() == container))
					{
						++total;
					}
				}
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

			pool.ForEachReverse(node =>
			{
				if (total <= maxSize)
				{
					return;
				}

				if (node.GetParent() == container)
				{
					node.QueueFree();
					pool.Remove(node);
					--total;
				}
			});

			if (clearSpawned && total > maxSize)
			{
				pool.ForEachReverse(node =>
				{
					if (total <= maxSize)
					{
						return;
					}

					node.QueueFree();
					pool.Remove(node);
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

			pool.ForEachReverse(node =>
			{
				if (clearSpawned || (node.GetParent() == container))
				{
					node.QueueFree();
					pool.Remove(node);
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
