// SPDX-License-Identifier: MIT
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Based on System.Json from https://github.com/mono/mono (MIT X11)

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ju.Hjson
{
	using JsonPair = KeyValuePair<string, JsonValue>;

	/// <summary>Implements an object value.</summary>
	public class JsonObject : JsonValue, IDictionary<string, JsonValue>, ICollection<JsonPair>
	{
		readonly Dictionary<string, JsonValue> map;

		/// <summary>Initializes a new instance of this class.</summary>
		/// <remarks>You can also initialize an object using the C# add syntax: new JsonObject { { "key", "value" }, ... }</remarks>
		public JsonObject(params JsonPair[] items)
		{
			map = new Dictionary<string, JsonValue>();
			if (items != null) AddRange(items);
		}

		/// <summary>Initializes a new instance of this class.</summary>
		/// <remarks>You can also initialize an object using the C# add syntax: new JsonObject { { "key", "value" }, ... }</remarks>
		public JsonObject(IEnumerable<JsonPair> items)
		{
			if (items == null) throw new ArgumentNullException("items");
			map = new Dictionary<string, JsonValue>();
			AddRange(items);
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
			get { return map[key]; }
			set { map[key] = value; }
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

		/// <summary>Adds a new item.</summary>
		/// <remarks>You can also initialize an object using the C# add syntax: new JsonObject { { "key", "value" }, ... }</remarks>
		public void Add(string key, JsonValue value)
		{
			if (key == null) throw new ArgumentNullException("key");
			map[key] = value; // json allows duplicate keys
		}

		/// <summary>Adds a new item.</summary>
		public void Add(JsonPair pair)
		{
			Add(pair.Key, pair.Value);
		}

		/// <summary>Adds a range of items.</summary>
		public void AddRange(IEnumerable<JsonPair> items)
		{
			if (items == null) throw new ArgumentNullException("items");
			foreach (var pair in items) Add(pair);
		}

		/// <summary>Clears the object.</summary>
		public void Clear()
		{
			map.Clear();
		}

		bool ICollection<JsonPair>.Contains(JsonPair item)
		{
			return (map as ICollection<JsonPair>).Contains(item);
		}

		bool ICollection<JsonPair>.Remove(JsonPair item)
		{
			return (map as ICollection<JsonPair>).Remove(item);
		}

		/// <summary>Determines whether the array contains a specific key.</summary>
		public override bool ContainsKey(string key)
		{
			if (key == null) throw new ArgumentNullException("key");
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
			if (key == null) throw new ArgumentNullException("key");
			return map.Remove(key);
		}

		bool ICollection<JsonPair>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>Gets the value associated with the specified key.</summary>
		public bool TryGetValue(string key, out JsonValue value)
		{
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
