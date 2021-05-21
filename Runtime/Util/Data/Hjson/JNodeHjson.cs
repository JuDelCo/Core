// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Extensions;
using Ju.Hjson;

namespace Ju.Data
{
	public static class JNodeHjson
	{
		public static JNode Parse(string hjson)
		{
			return Parse(HjsonValue.Parse(hjson));
		}

		public static JNode Parse(JsonValue data)
		{
			return Process(data);
		}

		private static JNode Process(JsonValue value)
		{
			JNode result;

			switch (value.JsonType)
			{
				case JsonType.Object:
					var jsonObject = value.Qo();
					var dict = new JDict(false, jsonObject.Count, true);
					result = dict;

					foreach (var kvp in jsonObject)
					{
						dict.Add(kvp.Key, Process(kvp.Value));
					}

					break;
				case JsonType.Array:
					var jsonArray = value.Qa();
					var list = new JList(true, jsonArray.Count, true);
					result = list;

					foreach (var item in jsonArray)
					{
						list.Add(Process(item));
					}

					break;
				case JsonType.Boolean:
					result = new JData<bool>(value.Qb(), default(bool));

					break;
				case JsonType.Number:
					var valueType = value.ToValue().GetType();

					if (valueType == typeof(int))
					{
						result = new JData<int>(value.Qi(), default(int));
					}
					else if (valueType == typeof(long))
					{
						result = new JData<long>(value.Ql(), default(long));
					}
					else if (valueType == typeof(float))
					{
						result = new JData<float>((float)value.Qd(), default(float));
					}
					else if (valueType == typeof(double))
					{
						result = new JData<double>(value.Qd(), default(double));
					}
					else
					{
						throw new Exception($"JsonValue of type Number contains an invalid type of: {valueType.GetFriendlyName()}.");
					}

					break;
				case JsonType.String:
					// TODO: Try to detect types and convert
					result = new JData<string>(value.Qs(), default(string));

					break;
				case JsonType.Unknown:
				default:
					result = new JData<string>(value.Qstr(), default(string));

					break;
			}

			return result;
		}
	}

	public static class JNodeJson
	{
		public static JNode Parse(string json)
		{
			return JNodeHjson.Parse(json);
		}

		public static JNode Parse(JsonValue data)
		{
			return JNodeHjson.Parse(data);
		}
	}
}
