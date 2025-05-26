// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

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
					var jsonPrimitive = (JsonPrimitive) value;

					switch (jsonPrimitive.JsonNumericKind)
					{
						case JsonPrimitive.NumericKind.Int:
							result = new JData<int>(jsonPrimitive.AsInt(), default(int));
							break;
						case JsonPrimitive.NumericKind.Long:
							result = new JData<long>(jsonPrimitive.AsLong(), default(long));
							break;
						case JsonPrimitive.NumericKind.Float:
							result = new JData<float>(jsonPrimitive.AsFloat(), default(float));
							break;
						case JsonPrimitive.NumericKind.Double:
							result = new JData<double>(jsonPrimitive.AsDouble(), default(double));
							break;
						case JsonPrimitive.NumericKind.Decimal:
							result = new JData<decimal>(jsonPrimitive.AsDecimal(), default(decimal));
							break;
						default:
							var valueType = value.AsObject().GetType();
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
