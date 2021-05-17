// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Extensions;
using Ju.Handlers;

namespace Ju.Data
{
	public interface IJDict : IDictionary<string, JNode>
	{
	}

	public partial class JDict : JNode, IDictionary<string, JNode>, IJDict
	{
		private readonly static string[] ignoredProperties = new string[]
		{
			"Path", "Children", "Count", "IsReadOnly", "Keys", "Values"
		};
		private static DisposableLinkHandler internalLinkHandler = null;
		private readonly Dictionary<string, JNode> dictionary;
		private readonly bool reseteable;
		private readonly bool subscribeToChildren;
		private bool autoSubscribeToChildren = false;
		private bool isRemoving = false;
		protected bool initialized = false;

		public JDict() : this(false, 0, true)
		{
		}

		public JDict(bool reseteable, bool subscribeToChildren = true) : this(reseteable, 0, subscribeToChildren)
		{
		}

		public JDict(bool reseteable, int capacity, bool subscribeToChildren = true)
		{
			dictionary = new Dictionary<string, JNode>(capacity);
			this.reseteable = reseteable;
			this.subscribeToChildren = subscribeToChildren;

			foreach (var property in this.GetType().GetProperties())
			{
				if (ignoredProperties.Contains(property.Name))
				{
					continue;
				}

				if (!property.CanWrite)
				{
					property.GetValue(this);
				}
			}

			initialized = true;
		}

		public override void Reset()
		{
			if (reseteable)
			{
				Clear();
			}
			else
			{
				foreach (var kvp in dictionary)
				{
					kvp.Value.Reset();
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

				foreach (var kvp in dictionary)
				{
					kvp.Value.Subscribe(internalLinkHandler, Trigger);
				}

				autoSubscribeToChildren = true;
			}
		}

		public override JNode Clone()
		{
			var newDict = new JDict(reseteable, dictionary.Count, subscribeToChildren);

			foreach (var kvp in dictionary)
			{
				newDict.Add(kvp.Key, kvp.Value.Clone());
			}

			return newDict;
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

				dictionary.ForEachReverse(kvp =>
				{
					kvp.Value.Dispose();
				});

				dictionary.Clear();
			}

			disposedValue = true;
		}

		public override IEnumerable<JNode> Children
		{
			get => dictionary.Values;
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

		public int Count => dictionary.Count;
		public bool IsReadOnly => false;

		public ICollection<string> Keys => dictionary.Keys;
		public ICollection<JNode> Values => dictionary.Values;

		public JNode this[string key]
		{
			get => dictionary[key];
			set
			{
				dictionary[key] = value;
				value.Parent = this;

				if (autoSubscribeToChildren)
				{
					value.Subscribe(internalLinkHandler, Trigger);
				}

				Trigger(this);
			}
		}

		public void Add<T>(string key, T value)
		{
			Add(key, value, value);
		}

		public void Add<T>(string key, T value, T defaultValue)
		{
			if (value is JNode node)
			{
				Add(key, node);
			}
			else
			{
				Add(key, (JNode)new JData<T>(value, defaultValue));
			}
		}

		public void Add(string key, JNode node)
		{
			dictionary.Add(key, node);
			node.Parent = this;

			if (autoSubscribeToChildren)
			{
				node.Subscribe(internalLinkHandler, Trigger);
			}

			Trigger(this);
		}

		public void Add(KeyValuePair<string, JNode> kvp)
		{
			Add(kvp.Key, kvp.Value);
		}

		public void Add(IEnumerable<KeyValuePair<string, JNode>> kvps)
		{
			foreach (var kvp in kvps)
			{
				var node = kvp.Value;
				dictionary.Add(kvp.Key, node);
				node.Parent = this;

				if (autoSubscribeToChildren)
				{
					node.Subscribe(internalLinkHandler, Trigger);
				}
			}

			Trigger(this);
		}

		public string GetKey(JNode node)
		{
			string result = null;

			foreach (var kvp in dictionary)
			{
				if (kvp.Value == node)
				{
					result = kvp.Key;
					break;
				}
			}

			return result;
		}

		public bool ContainsKey(string key)
		{
			return dictionary.ContainsKey(key);
		}

		public bool Contains(KeyValuePair<string, JNode> kvp)
		{
			return dictionary.Contains(kvp);
		}

		public void CopyTo(KeyValuePair<string, JNode>[] array, int arrayIndex)
		{
			(new List<KeyValuePair<string, JNode>>(dictionary)).CopyTo(array, arrayIndex);
		}

		public IJDict AsEnumerableDict()
		{
			return this;
		}

		IEnumerator<KeyValuePair<string, JNode>> IEnumerable<KeyValuePair<string, JNode>>.GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		public override IEnumerator<JNode> GetEnumerator()
		{
			using (var enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					yield return enumerator.Current.Value;
				}
			}
		}

		public bool TryGetValue(string key, out JNode node)
		{
			return dictionary.TryGetValue(key, out node);
		}

		public void Move(string oldKey, string newKey)
		{
			var node = this[oldKey];
			dictionary.Remove(oldKey);
			dictionary[newKey] = node;
			Trigger(this);
		}

		public bool Remove(string key)
		{
			if (isRemoving)
			{
				return false;
			}

			var result = false;

			if (ContainsKey(key))
			{
				isRemoving = true;

				dictionary[key].Parent = null;

				result = dictionary.Remove(key);

				isRemoving = false;

				if (result)
				{
					Trigger(this);
				}
			}

			return result;
		}

		public void Remove(JNode node)
		{
			this.RemoveIf((kvp) => kvp.Value == node);
		}

		public bool Remove(KeyValuePair<string, JNode> kvp)
		{
			return Remove(kvp.Key);
		}

		public void Clear()
		{
			Clear(true);
		}

		public void Clear(bool triggerOnlyOnce = true)
		{
			if (dictionary.Count <= 0)
			{
				return;
			}

			if (triggerOnlyOnce)
			{
				isRemoving = true;
			}

			dictionary.ForEachReverse(kvp =>
			{
				kvp.Value.Parent = null;
			});

			isRemoving = false;

			dictionary.Clear();

			if (triggerOnlyOnce)
			{
				Trigger(this);
			}
		}

		public override string ToString()
		{
			return $"{{ {string.Join(", ", dictionary.Map(kvp => kvp.Key + " = " + kvp.Value))} }}";
		}
	}
}
