// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Extensions;
using Ju.Handlers;

namespace Ju.Data
{
	public class JList : JNode, IList<JNode>
	{
		private static DisposableLinkHandler internalLinkHandler = null;
		private readonly List<JNode> list;
		private readonly bool reseteable;
		private readonly bool subscribeToChildren;
		private bool autoSubscribeToChildren = false;
		private bool isRemoving = false;

		public JList() : this(true, 0, true)
		{
		}

		public JList(bool reseteable, bool subscribeToChildren = true) : this(reseteable, 0, subscribeToChildren)
		{
		}

		public JList(bool reseteable, int capacity, bool subscribeToChildren = true)
		{
			list = new List<JNode>(capacity);
			this.reseteable = reseteable;
			this.subscribeToChildren = subscribeToChildren;
		}

		public override void Reset()
		{
			if (reseteable)
			{
				Clear();
			}
			else
			{
				foreach (var node in list)
				{
					node.Reset();
				}
			}
		}

		public override void Subscribe(ILinkHandler handle, Action<JNode> action)
		{
			base.Subscribe(handle, action);

			if (subscribeToChildren && !autoSubscribeToChildren)
			{
				if (internalLinkHandler == null)
				{
					internalLinkHandler = new DisposableLinkHandler(false);
				}

				foreach (var node in list)
				{
					node.Subscribe(internalLinkHandler, Trigger);
				}

				autoSubscribeToChildren = true;
			}
		}

		public override JNode Clone()
		{
			var newList = new JList(reseteable, list.Count, subscribeToChildren);

			foreach (var item in list)
			{
				newList.Add(item.Clone());
			}

			return newList;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposedValue)
			{
				return;
			}

			if (disposing)
			{
				base.Dispose(disposing);

				if (internalLinkHandler != null && !internalLinkHandler.IsDestroyed)
				{
					internalLinkHandler.Dispose();
				}

				list.ForEachReverse(node =>
				{
					node.Dispose();
				});

				list.Clear();
			}

			disposedValue = true;
		}

		public override IEnumerable<JNode> Children
		{
			get => list.AsReadOnly();
		}

		protected override void OnSubscribersEmpty()
		{
			autoSubscribeToChildren = false;

			if (internalLinkHandler != null && !internalLinkHandler.IsDestroyed)
			{
				internalLinkHandler.Dispose();
				internalLinkHandler = null;
			}
		}

		public bool IsReseteable()
		{
			return reseteable;
		}

		public bool IsSubscribingToChildren()
		{
			return subscribeToChildren;
		}

		public int Count => list.Count;
		public bool IsReadOnly => false;

		public JNode this[int index]
		{
			get => list[index];
			set
			{
				list[index] = value;
				value.Parent = this;

				if (autoSubscribeToChildren)
				{
					value.Subscribe(internalLinkHandler, Trigger);
				}

				Trigger(this);
			}
		}

		public void Add<T>(T value)
		{
			Add(value, default(T));
		}

		public void Add<T>(T value, T defaultValue)
		{
			if (value is JNode node)
			{
				Add(node);
			}
			else
			{
				Add((JNode)new JData<T>(value, defaultValue));
			}
		}

		public void Add(JNode node)
		{
			list.Add(node);
			node.Parent = this;

			if (autoSubscribeToChildren)
			{
				node.Subscribe(internalLinkHandler, Trigger);
			}

			Trigger(this);
		}

		public void Add<T>(IEnumerable<T> values)
		{
			Add(values, default(T));
		}

		public void Add<T>(IEnumerable<T> values, T defaultValue)
		{
			foreach (var value in values)
			{
				if (value is JNode node)
				{
					list.Add(node);
				}
				else
				{
					node = (new JData<T>(value, defaultValue));
					list.Add(node);
				}

				node.Parent = this;

				if (autoSubscribeToChildren)
				{
					node.Subscribe(internalLinkHandler, Trigger);
				}
			}

			Trigger(this);
		}

		public void Add(IEnumerable<JNode> nodes)
		{
			foreach (var node in nodes)
			{
				list.Add(node);
				node.Parent = this;

				if (autoSubscribeToChildren)
				{
					node.Subscribe(internalLinkHandler, Trigger);
				}
			}

			Trigger(this);
		}

		public void Insert<T>(int index, T value)
		{
			Insert(index, value);
		}

		public void Insert<T>(int index, T value, T defaultValue)
		{
			if (value is JNode node)
			{
				Insert(index, node);
			}
			else
			{
				Insert(index, (JNode)new JData<T>(value, defaultValue));
			}
		}

		public void Insert(int index, JNode node)
		{
			list.Insert(index, node);
			node.Parent = this;

			if (autoSubscribeToChildren)
			{
				node.Subscribe(internalLinkHandler, Trigger);
			}

			Trigger(this);
		}

		public bool Contains<T>(T value)
		{
			var equalityComparer = EqualityComparer<T>.Default;

			foreach (var node in list)
			{
				if (node.IsData() && node.GetDataType() == typeof(T))
				{
					if (equalityComparer.Equals(node.AsData<T>().Value, value))
					{
						return true;
					}
				}
			}

			return false;
		}

		public bool Contains(JNode node)
		{
			return list.Contains(node);
		}

		public void CopyTo(JNode[] array, int arrayIndex)
		{
			list.CopyTo(array, arrayIndex);
		}

		public override IEnumerator<JNode> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public int IndexOf<T>(T value)
		{
			var equalityComparer = EqualityComparer<T>.Default;

			for (int i = 0; i < list.Count; ++i)
			{
				var node = list[i];

				if (node.IsData() && node.GetDataType() == typeof(T))
				{
					if (equalityComparer.Equals(node.AsData<T>().Value, value))
					{
						return i;
					}
				}
			}

			return -1;
		}

		public int IndexOf(JNode node)
		{
			return list.IndexOf(node);
		}

		public void Move(int oldIndex, int newIndex)
		{
			var node = this[oldIndex];
			list.RemoveAt(oldIndex);
			list.Insert(newIndex, node);
			Trigger(this);
		}

		public bool Remove<T>(T value)
		{
			var index = IndexOf(value);
			var found = (index >= 0);

			if (found)
			{
				RemoveAt(index);
			}

			return found;
		}

		public bool Remove(JNode node)
		{
			if (isRemoving)
			{
				return false;
			}

			var result = false;

			if (list.Contains(node))
			{
				isRemoving = true;

				node.Parent = null;

				result = list.Remove(node);

				isRemoving = false;

				if (result)
				{
					Trigger(this);
				}
			}

			return result;
		}

		public void RemoveAt(int index)
		{
			list[index].Parent = null;
		}

		public void Clear()
		{
			Clear(true);
		}

		public void Clear(bool triggerOnlyOnce = true)
		{
			if (list.Count <= 0)
			{
				return;
			}

			if (triggerOnlyOnce)
			{
				isRemoving = true;
			}

			list.ForEachReverse(node =>
			{
				node.Parent = null;
			});

			isRemoving = false;

			list.Clear();

			if (triggerOnlyOnce)
			{
				Trigger(this);
			}
		}

		public void TrimExcess()
		{
			list.TrimExcess();
		}

		public override string ToString()
		{
			return $"{{ {string.Join(", ", list)} }}";
		}

		public override string ToString(int maxDepth)
		{
			if (maxDepth == 0)
			{
				return "[...]";
			}

			return $"{{ {string.Join(", ", list.Map(n => n.ToString(maxDepth - 1)))} }}";
		}
	}
}
