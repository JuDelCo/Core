// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Data;
using Ju.Data.Conversion;
using Ju.Extensions;
using Ju.Hjson;
using Ju.Log;

public static class JNodeHjsonExtensions
{
	public static string ToHjson(this JNode node, int maxDepth = -1)
	{
		return Process(node, maxDepth).ToString(Stringify.Hjson);
	}

	public static string ToJson(this JNode node, int maxDepth)
	{
		return node.ToJson(false, maxDepth);
	}

	public static string ToJson(this JNode node, bool compact = false, int maxDepth = -1)
	{
		return Process(node, maxDepth).ToString(compact ? Stringify.Plain : Stringify.Formatted);
	}

	private static JsonValue Process(JNode node, int maxDepth)
	{
		JsonValue result;

		if (node == null)
		{
			result = null;
		}
		else if (node.IsRef())
		{
			if (node.AsRef().Reference != null)
			{
				result = new JsonPrimitive($"Ref{{{node.AsRef().Reference.Path}}}");
			}
			else
			{
				result = new JsonPrimitive(null);
			}
		}
		else
		{
			if (node.IsDict())
			{
				if (maxDepth == 0)
				{
					result = new JsonObject();
				}
				else
				{
					var o = new JsonObject();
					result = o;

					foreach (var kvp in node.AsDict().AsEnumerableDict())
					{
						o.Add(kvp.Key, Process(kvp.Value, maxDepth - 1));
					}
				}
			}
			else if (node.IsList())
			{
				if (maxDepth == 0)
				{
					result = new JsonArray();
				}
				else
				{
					var a = new JsonArray();
					result = a;

					foreach (var item in node.AsList())
					{
						a.Add(Process(item, maxDepth - 1));
					}
				}
			}
			else // JData
			{
				result = ProcessData(node);
			}
		}

		return result;
	}

	private static JsonPrimitive ProcessData(JNode node)
	{
		JsonPrimitive result = null;

		if (!node.IsData())
		{
			throw new Exception("Hjson serializer error: Unknown JNode type.");
		}

		var dataType = node.GetDataType();

		if (dataType == typeof(string))
		{
			result = new JsonPrimitive(node.AsData<string>().Value);
		}
		else if (dataType == typeof(bool))
		{
			result = new JsonPrimitive(node.AsData<bool>().Value);
		}
		else if (dataType == typeof(byte))
		{
			result = new JsonPrimitive(node.AsData<byte>().Value);
		}
		else if (dataType == typeof(sbyte))
		{
			result = new JsonPrimitive(node.AsData<sbyte>().Value);
		}
		else if (dataType == typeof(char))
		{
			result = new JsonPrimitive(node.AsData<char>().Value);
		}
		else if (dataType == typeof(float))
		{
			result = new JsonPrimitive(node.AsData<float>().Value);
		}
		else if (dataType == typeof(double))
		{
			result = new JsonPrimitive(node.AsData<double>().Value);
		}
		else if (dataType == typeof(decimal))
		{
			result = new JsonPrimitive(node.AsData<decimal>().Value);
		}
		else if (dataType == typeof(short))
		{
			result = new JsonPrimitive(node.AsData<short>().Value);
		}
		else if (dataType == typeof(int))
		{
			result = new JsonPrimitive(node.AsData<int>().Value);
		}
		else if (dataType == typeof(long))
		{
			result = new JsonPrimitive(node.AsData<long>().Value);
		}
		else if (dataType == typeof(ushort))
		{
			result = new JsonPrimitive(node.AsData<ushort>().Value);
		}
		else if (dataType == typeof(uint))
		{
			result = new JsonPrimitive(node.AsData<uint>().Value);
		}
		else if (dataType == typeof(ulong))
		{
			result = new JsonPrimitive(Cast.This(node.AsData<ulong>().Value).AsString());
		}
		else if (dataType.IsEnum)
		{
			result = new JsonPrimitive(node.GetRawData().ToString());
		}
		else if (DataTypeConverter.HasConverter(new ConversionType(dataType, typeof(string))))
		{
			result = new JsonPrimitive((string) DataTypeConverter.GetConverter(new ConversionType(dataType, typeof(string)))(node.GetRawData()));
		}
		else
		{
			Log.Warning($"No string converter found for the type {dataType.GetFriendlyName()}, falling back to default ToString() method");

			result = new JsonPrimitive(node.GetRawData().ToString());
		}

		return result;
	}

	public static void UpdateFromHjson(this JNode node, string hjson, bool clearLists = true)
	{
		node.Merge(JNodeHjson.Parse(hjson), clearLists);
	}

	public static void UpdateFromJson(this JNode node, string json, bool clearLists = true)
	{
		node.UpdateFromHjson(json, clearLists);
	}
}
