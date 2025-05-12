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
	using JsonPair = KeyValuePair<string, JsonValue>;

	internal class JsonObjectHandleActionPair
	{
		public ILinkHandler handle;
		public Action<NotifyCollectionChangedEventArgs> action;

		public JsonObjectHandleActionPair(ILinkHandler handle, Action<NotifyCollectionChangedEventArgs> action)
		{
			this.handle = handle;
			this.action = action;
		}
	}

	/// <summary>Implements an object value.</summary>
	public class JsonObject : JsonValue, IDictionary<string, JsonValue>, ICollection<JsonPair>, INotifyCollectionChanged
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

		private readonly Dictionary<string, JsonValue> map;
		private readonly List<JsonObjectHandleActionPair> actions;
		private uint callStackCounter = 0;

		/// <summary>Initializes a new instance of this class.</summary>
		public JsonObject()
		{
			map = new Dictionary<string, JsonValue>();
			actions = new List<JsonObjectHandleActionPair>();
		}

		/// <summary>Initializes a new instance of this class.</summary>
		/// <remarks>You can also initialize an object using the C# add syntax: new JsonObject { { "key", "value" }, ... }</remarks>
		public JsonObject(params JsonPair[] items)
		{
			map = new Dictionary<string, JsonValue>();
			actions = new List<JsonObjectHandleActionPair>();
			if (items != null) AddRange(items);
		}

		/// <summary>Initializes a new instance of this class.</summary>
		/// <remarks>You can also initialize an object using the C# add syntax: new JsonObject { { "key", "value" }, ... }</remarks>
		public JsonObject(IEnumerable<JsonPair> items)
		{
			if (items == null) throw new ArgumentNullException("items");
			map = new Dictionary<string, JsonValue>();
			actions = new List<JsonObjectHandleActionPair>();
			AddRange(items);
		}

		public void Subscribe(ILinkHandler handle, Action<NotifyCollectionChangedEventArgs> action)
		{
			actions.Add(new JsonObjectHandleActionPair(handle, action));
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
					throw new Exception("An JsonObject callback has modified the same collection that triggered it.");
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
			get { return map.Count; }
		}

		IEnumerator<JsonPair> IEnumerable<JsonPair>.GetEnumerator()
		{
			return map.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return map.GetEnumerator();
		}

		/// <summary>Gets or sets the value for the specified key.</summary>
		public override sealed JsonValue this[string key]
		{
			get
			{
				if (key == null) throw new ArgumentNullException(nameof(key));

				if (!map.TryGetValue(key, out var value))
				{
					throw new KeyNotFoundException($"The given key '{key}' was not present in the JsonObject.");
				}

				return value;
			}
			set
			{
				if (key == null) throw new ArgumentNullException(nameof(key));

				var newItem = new JsonPair(key, value);

				if (map.TryGetValue(key, out var oldValue))
				{
					if (!Equals(oldValue, value))
					{
						map[key] = value;
						var oldItem = new JsonPair(key, oldValue);

						OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem));
					}
				}
				else
				{
					map[key] = value;

					OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem));
				}
			}
		}

		/// <summary>The type of this value.</summary>
		public override JsonType JsonType
		{
			get { return JsonType.Object; }
		}

		/// <summary>Gets the keys of this object.</summary>
		public ICollection<string> Keys
		{
			get { return map.Keys; }
		}

		/// <summary>Gets the values of this object.</summary>
		public ICollection<JsonValue> Values
		{
			get { return map.Values; }
		}

		/// <summary>Adds a new item (json allows duplicate keys, so it will replace the value if it exists).</summary>
		/// <remarks>You can also initialize an object using the C# add syntax: new JsonObject { { "key", "value" }, ... }</remarks>
		public void Add(string key, JsonValue value)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));
			this[key] = value; // json allows duplicate keys
		}

		/// <summary>Adds a new item.</summary>
		public void Add(JsonPair pair)
		{
			Add(pair.Key, pair.Value);
		}

		/// <summary>Adds a range of items.</summary>
		public void AddRange(IEnumerable<JsonPair> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));

			var addedItems = new List<JsonPair>();
			var replacedNewItems = new List<JsonPair>();
			var replacedOldItems = new List<JsonPair>();

			foreach (var item in items)
			{
				if (map.TryGetValue(item.Key, out var oldValue))
				{
					if (!Equals(oldValue, item.Value))
					{
						map[item.Key] = item.Value;

						replacedNewItems.Add(item);
						replacedOldItems.Add(new JsonPair(item.Key, oldValue));
					}
				}
				else
				{
					map[item.Key] = item.Value;

					addedItems.Add(item);
				}
			}

			if (addedItems.Count > 0)
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItems));
			}

			if (replacedNewItems.Count > 0)
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, replacedNewItems, replacedOldItems));
			}
		}

		/// <summary>Clears the object.</summary>
		public void Clear()
		{
			if (map.Count > 0)
			{
				map.Clear();

				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		bool ICollection<JsonPair>.Contains(JsonPair item)
		{
			return (map as ICollection<JsonPair>).Contains(item);
		}

		bool ICollection<JsonPair>.Remove(JsonPair item)
		{
			return Remove(item.Key);
		}

		/// <summary>Determines whether the array contains a specific key.</summary>
		public override bool ContainsKey(string key)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));
			return map.ContainsKey(key);
		}

		/// <summary>Copies the elements to an System.Array, starting at a particular System.Array index.</summary>
		public void CopyTo(JsonPair[] array, int arrayIndex)
		{
			(map as ICollection<JsonPair>).CopyTo(array, arrayIndex);
		}

		/// <summary>Removes the item with the specified key.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>true if the element is successfully found and removed; otherwise, false.</returns>
		public bool Remove(string key)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));

			if (map.TryGetValue(key, out var oldValue) && map.Remove(key))
			{
				var oldItem = new JsonPair(key, oldValue);

				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem));

				return true;
			}

			return false;
		}

		bool ICollection<JsonPair>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>Gets the value associated with the specified key.</summary>
		public bool TryGetValue(string key, out JsonValue value)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));

			return map.TryGetValue(key, out value);
		}

		void ICollection<JsonPair>.Add(JsonPair item)
		{
			this.Add(item);
		}

		void ICollection<JsonPair>.Clear()
		{
			this.Clear();
		}

		void ICollection<JsonPair>.CopyTo(JsonPair[] array, int arrayIndex)
		{
			this.CopyTo(array, arrayIndex);
		}

		int ICollection<JsonPair>.Count
		{
			get { return this.Count; }
		}
	}
}
