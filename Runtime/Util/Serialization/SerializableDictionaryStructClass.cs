// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Extensions;

namespace Ju.Util
{
	[Serializable]
	public class SerializableDictionaryStructClass<TKey, TValue> : Dictionary<TKey, TValue>
#if UNITY_EDITOR
#if UNITY_2019_3_OR_NEWER
	, UnityEngine.ISerializationCallbackReceiver
#endif
	where TKey : struct
	where TValue : class
#endif
	{
#if UNITY_EDITOR
#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeField]
		[UnityEngine.HideInInspector]
#endif
		private List<TKey> keys = new List<TKey>();

#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeField]
		[UnityEngine.SerializeReference]
		[UnityEngine.HideInInspector]
#endif
		private List<TValue> values = new List<TValue>();

		public void OnBeforeSerialize()
		{
			keys.Clear();
			values.Clear();

			foreach (KeyValuePair<TKey, TValue> pair in this)
			{
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			this.Clear();

			if (keys.Count < values.Count)
			{
				throw new Exception($"Error deserializing dictionary keys of type {typeof(TKey).GetFriendlyName()}");
			}
			else if (keys.Count > values.Count)
			{
				throw new Exception($"Error deserializing dictionary values of type {typeof(TValue).GetFriendlyName()}");
			}

			for (int i = 0; i < keys.Count; i++)
			{
				this.Add(keys[i], values[i]);
			}
		}
#endif
	}
}
