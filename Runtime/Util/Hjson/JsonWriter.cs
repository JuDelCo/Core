// SPDX-License-Identifier: MIT
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Based on System.Json from https://github.com/mono/mono (MIT X11)

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ju.Hjson
{
	using JsonPair = KeyValuePair<string, JsonValue>;

	internal class JsonWriter
	{
		readonly bool format;

		public JsonWriter(bool format)
		{
			this.format = format;
		}

		void Nl(TextWriter tw, int level)
		{
			if (format)
			{
				tw.Write(JsonValue.eol);
				tw.Write(new string(' ', level * 2));
			}
		}

		public void Save(JsonValue value, TextWriter tw, int level)
		{
			bool following = false;
			switch (value.JsonType)
			{
				case JsonType.Object:
					if (level > 0) Nl(tw, level);
					tw.Write('{');
					foreach (JsonPair pair in ((JsonObject)value))
					{
						if (following) tw.Write(",");
						Nl(tw, level + 1);
						tw.Write('\"');
						tw.Write(EscapeString(pair.Key));
						tw.Write("\":");
						var nextType = pair.Value != null ? (JsonType?)pair.Value.JsonType : null;
						if (format && nextType != JsonType.Array && nextType != JsonType.Object) tw.Write(" ");
						if (pair.Value == null) tw.Write("null");
						else Save(pair.Value, tw, level + 1);
						following = true;
					}
					if (following) Nl(tw, level);
					tw.Write('}');
					break;
				case JsonType.Array:
					if (level > 0) Nl(tw, level);
					tw.Write('[');
					foreach (JsonValue v in ((JsonArray)value))
					{
						if (following) tw.Write(",");
						if (v != null)
						{
							if (v.JsonType != JsonType.Array && v.JsonType != JsonType.Object) Nl(tw, level + 1);
							Save(v, tw, level + 1);
						}
						else
						{
							Nl(tw, level + 1);
							tw.Write("null");
						}
						following = true;
					}
					if (following) Nl(tw, level);
					tw.Write(']');
					break;
				case JsonType.Boolean:
					tw.Write((bool)value ? "true" : "false");
					break;
				case JsonType.String:
					tw.Write('"');
					tw.Write(EscapeString(((JsonPrimitive)value).GetRawString()));
					tw.Write('"');
					break;
				default:
					tw.Write(((JsonPrimitive)value).GetRawString());
					break;
			}
		}

		internal static string EscapeString(string src)
		{
			if (src == null) return null;

			for (int i = 0; i < src.Length; i++)
			{
				if (GetEscapedChar(src[i]) != null)
				{
					var sb = new StringBuilder();
					if (i > 0) sb.Append(src, 0, i);
					return DoEscapeString(sb, src, i);
				}
			}
			return src;
		}

		static string DoEscapeString(StringBuilder sb, string src, int cur)
		{
			int start = cur;
			for (int i = cur; i < src.Length; i++)
			{
				string escaped = GetEscapedChar(src[i]);
				if (escaped != null)
				{
					sb.Append(src, start, i - start);
					sb.Append(escaped);
					start = i + 1;
				}
			}
			sb.Append(src, start, src.Length - start);
			return sb.ToString();
		}

		static string GetEscapedChar(char c)
		{
			switch (c)
			{
				case '\"': return "\\\"";
				case '\t': return "\\t";
				case '\n': return "\\n";
				case '\r': return "\\r";
				case '\f': return "\\f";
				case '\b': return "\\b";
				case '\\': return "\\\\";
				default: return null;
			}
		}
	}
}
