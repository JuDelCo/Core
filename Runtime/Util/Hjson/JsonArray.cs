// SPDX-License-Identifier: MIT
// Copyright (c) 2021-2025 Juan Delgado (@JuDelCo)
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Based on System.Json from https://github.com/mono/mono (MIT X11)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ju.Handlers;

namespace Ju.Hjson
{
	internal class JsonArrayHandleActionPair
	{
		public ILinkHandler handle;
		public Action<NotifyCollectionChangedEventArgs> action;

		public JsonArrayHandleActionPair(ILinkHandler handle, Action<NotifyCollectionChangedEventArgs> action)
		{
			this.handle = handle;
			this.action = action;
		}
	}

	/// <summary>Implements an array value.</summary>
	public class JsonArray : JsonValue, IList<JsonValue>, INotifyCollectionChanged
	{
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (CollectionChanged != null)
			{
				CollectionChanged.Invoke(this, e);
			}

			Trigger(e);
		}

		private readonly List<JsonValue> list;
		private readonly List<JsonArrayHandleActionPair> actions;
		private uint callStackCounter = 0;

		/// <summary>Initializes a new instance of this class.</summary>
		public JsonArray()
		{
			list = new List<JsonValue>();
			actions = new List<JsonArrayHandleActionPair>();
		}

		/// <summary>Initializes a new instance of this class.</summary>
		public JsonArray(params JsonValue[] items)
		{
			list = new List<JsonValue>();
			actions = new List<JsonArrayHandleActionPair>();
			AddRange(items);
		}

		/// <summary>Initializes a new instance of this class.</summary>
		public JsonArray(IEnumerable<JsonValue> items)
		{
			if (items == null) throw new ArgumentNullException("items");
			list = new List<JsonValue>(items);
			actions = new List<JsonArrayHandleActionPair>();
		}

		public void Subscribe(ILinkHandler handle, Action<NotifyCollectionChangedEventArgs> action)
		{
			actions.Add(new JsonArrayHandleActionPair(handle, action));
		}

		public int SubscribersCount()
		{
			return actions.Count;
		}

		private void Trigger(NotifyCollectionChangedEventArgs e)
		{
			for (int i = actions.Count - 1; i >= 0; --i)
			{
				if (callStackCounter > 0)
				{
					throw new Exception("An JsonArray callback has modified the same collection that triggered it.");
				}

				var handle = actions[i].handle;

				if (handle.IsDestroyed)
				{
					actions.RemoveAt(i);

					continue;
				}

				if (!handle.IsActive)
				{
					continue;
				}

				++callStackCounter;

				actions[i].action(e);

				--callStackCounter;
			}
		}

		/// <summary>Gets the count of the contained items.</summary>
		public override int Count
		{
			get { return list.Count; }
		}

		bool ICollection<JsonValue>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>Gets or sets the value for the specified index.</summary>
		public override sealed JsonValue this[int index]
		{
			get
			{
				if (index < 0 || index >= list.Count)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				return list[index];
			}
			set
			{
				if (index < 0 || index >= list.Count)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				var oldItem = list[index];

				if (!Equals(oldItem, value))
				{
					list[index] = value;

					OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem, index));
				}
			}
		}

		/// <summary>The type of this value.</summary>
		public override JsonType JsonType
		{
			get { return JsonType.Array; }
		}

		/// <summary>Adds a new item.</summary>
		public void Add(JsonValue item)
		{
			list.Add(item);

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, list.Count - 1));
		}

		/// <summary>Adds a range of items.</summary>
		public void AddRange(IEnumerable<JsonValue> items)
		{
			if (items == null) throw new ArgumentNullException("items");

			var addedItems = new List<JsonValue>(items);

			if (addedItems.Count == 0)
			{
				return;
			}

			int startIndex = list.Count;
			list.AddRange(addedItems);

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItems, startIndex));
		}

		/// <summary>Clears the array.</summary>
		public void Clear()
		{
			if (list.Count > 0)
			{
				list.Clear();

				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		/// <summary>Determines whether the array contains a specific value.</summary>
		public bool Contains(JsonValue item)
		{
			return list.Contains(item);
		}

		/// <summary>Copies the elements to an System.Array, starting at a particular System.Array index.</summary>
		public void CopyTo(JsonValue[] array, int arrayIndex)
		{
			list.CopyTo(array, arrayIndex);
		}

		/// <summary>Determines the index of a specific item.</summary>
		public int IndexOf(JsonValue item)
		{
			return list.IndexOf(item);
		}

		/// <summary>Inserts an item.</summary>
		public void Insert(int index, JsonValue item)
		{
			if (index < 0 || index > list.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			list.Insert(index, item);

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
		}

		/// <summary>Removes the specified item.</summary>
		public bool Remove(JsonValue item)
		{
			var index = list.IndexOf(item);

			if (index >= 0 && list.Remove(item))
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));

				return true;
			}

			return false;
		}

		/// <summary>Removes the item with the specified index.</summary>
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= list.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			var removedItem = list[index];

			list.RemoveAt(index);

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
		}

		IEnumerator<JsonValue> IEnumerable<JsonValue>.GetEnumerator()
		{
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}
}
