// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Data;
using Ju.Extensions;
using Ju.Log;

public static class JNodeExtensions
{
	public static bool IsDict(this JNode node)
	{
		if (node is JRef jRef)
		{
			return jRef.Reference.IsDict();
		}

		return (node is JDict);
	}

	public static bool IsList(this JNode node)
	{
		if (node is JRef jRef)
		{
			return jRef.Reference.IsList();
		}

		return (node is JList);
	}

	public static bool IsData(this JNode node)
	{
		if (node is JRef jRef)
		{
			return jRef.Reference.IsData();
		}

		return (node is IJData);
	}

	public static bool IsRef(this JNode node)
	{
		return (node is JRef);
	}

	public static JDict AsDict(this JNode node)
	{
		if (node is JRef jRef)
		{
			return jRef.Reference.AsDict();
		}
		else if (node is JDict jDict)
		{
			return jDict;
		}

		return null;
	}

	public static JList AsList(this JNode node)
	{
		if (node is JRef jRef)
		{
			return jRef.Reference.AsList();
		}
		else if (node is JList jList)
		{
			return jList;
		}

		return null;
	}

	public static JData<T> AsData<T>(this JNode node)
	{
		if (node is JRef jRef)
		{
			return jRef.Reference.AsData<T>();
		}

		var jDataType = node.GetDataType();

		if (jDataType != null && jDataType != typeof(T))
		{
			throw new Exception($"JData type {jDataType.GetFriendlyName()} differs to the requested type {typeof(T).GetFriendlyName()}.");
		}

		if (node is JData<T> jData)
		{
			return jData;
		}

		return null;
	}

	public static JRef AsRef(this JNode node)
	{
		if (node is JRef jRef)
		{
			return jRef;
		}

		return null;
	}

	public static Type GetDataType(this JNode node)
	{
		if (node is JRef jRef)
		{
			return jRef.Reference.GetDataType();
		}

		if (node is IJData jData)
		{
			return jData.GetDataType();
		}

		return null;
	}

	public static object GetRawData(this JNode node)
	{
		if (node is JRef jRef)
		{
			return jRef.Reference.GetRawData();
		}

		if (node is IJData jData)
		{
			return jData.GetRawData();
		}

		return null;
	}

	public static void SetRawData(this JNode node, object rawData)
	{
		if (node is JRef jRef)
		{
			jRef.Reference.SetRawData(rawData);
		}

		if (node is IJData jData)
		{
			jData.SetRawData(rawData);
		}
	}

	public static IEnumerable<JNode> Select(this JNode node, Func<JNode, bool> predicate, int maxDepth = -1)
	{
		if (predicate(node))
		{
			yield return node;
		}

		if (maxDepth != 0)
		{
			foreach (var child in node.Children)
			{
				using (var enumerator = child.Select(predicate, (maxDepth - 1)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						yield return enumerator.Current;
					}
				}
			}
		}
	}

	public static void Merge(this JNode node, JNode data, bool clearLists = true)
	{
		if (node.IsDict())
		{
			if (!data.IsDict())
			{
				Log.Warning($"Tried to merge JNode of type {data.GetType().GetFriendlyName()} into a JDict node in {node.Path}.");
				return;
			}

			var nodeDict = node.AsDict();
			var dataDict = data.AsDict();

			var kvps = dataDict.AsEnumerableDict().Reverse();

			kvps.ForEachReverse(kvp =>
			{
				if (nodeDict.ContainsKey(kvp.Key))
				{
					nodeDict[kvp.Key].Merge(kvp.Value, clearLists);
				}
				else
				{
					nodeDict.Add(kvp.Key, kvp.Value);
				}
			});
		}
		else if (node.IsList())
		{
			if (!data.IsList())
			{
				Log.Warning($"Tried to merge JNode of type {data.GetType().GetFriendlyName()} into a JList node in {node.Path}.");
				return;
			}

			var nodeList = node.AsList();
			var dataList = data.AsList();

			if (clearLists)
			{
				nodeList.Clear();
			}

			var nodes = dataList.Reverse();

			nodes.ForEachReverse(item =>
			{
				nodeList.Add(item);
			});
		}
		else if (node.IsData())
		{
			if (!data.IsData())
			{
				Log.Warning($"Tried to merge JNode of type {data.GetType().GetFriendlyName()} into a JData node in {node.Path}.");
				return;
			}

			var nodeData = node as IJData;
			var dataData = data as IJData;

			// This method throws exceptions too
			nodeData.SetRawData(dataData.GetRawData());
		}
	}
}
