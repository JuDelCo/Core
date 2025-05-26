// SPDX-License-Identifier: MIT
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Based on System.Json from https://github.com/mono/mono (MIT X11)

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Ju.Hjson
{
	using JsonPair = KeyValuePair<string, JsonValue>;

	internal class HjsonWriter
	{
		readonly bool writeWsc;
		readonly bool emitRootBraces;
		readonly IEnumerable<IHjsonDsfProvider> dsfProviders = System.Linq.Enumerable.Empty<IHjsonDsfProvider>();
		static readonly Regex needsEscapeName = new Regex(@"[,\{\[\}\]\s:#""']|\/\/|\/\*|'''");

		public HjsonWriter(HjsonOptions options)
		{
			if (options != null)
			{
				writeWsc = options.KeepWsc;
				emitRootBraces = options.EmitRootBraces;
				dsfProviders = options.DsfProviders;
			}
			else emitRootBraces = true;
		}

		void Nl(TextWriter tw, int level)
		{
			tw.Write(JsonValue.eol);
			tw.Write(new string(' ', level * 2));
		}

		string GetWsc(string str)
		{
			if (string.IsNullOrEmpty(str)) return "";
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == '\n' ||
				  c == '#' ||
				  c == '/' && i + 1 < str.Length && (str[i + 1] == '/' || str[i + 1] == '*')) break;
				if (c > ' ') return " # " + str;
			}
			return str;
		}

		string GetWsc(Dictionary<string, string> white, string key) { return white.ContainsKey(key) ? GetWsc(white[key]) : ""; }
		string GetWsc(List<string> white, int index) { return white.Count > index ? GetWsc(white[index]) : ""; }
		bool TestWsc(string str) { return str.Length > 0 && str[str[0] == '\r' && str.Length > 1 ? 1 : 0] != '\n'; }

		public void Save(JsonValue value, TextWriter tw, int level, bool hasComment, string separator, bool noIndent = false, bool isRootObject = false)
		{
			if (value == null)
			{
				tw.Write(separator);
				tw.Write("null");
				return;
			}

			// check for DSF
			string dsfValue = HjsonDsf.Stringify(dsfProviders, value);
			if (dsfValue != null)
			{
				tw.Write(separator);
				tw.Write(dsfValue);
				return;
			}

			switch (value.JsonType)
			{
				case JsonType.Object:
					var obj = value.Qo();
					WscJsonObject kw = writeWsc ? obj as WscJsonObject : null;
					bool showBraces = !isRootObject || (kw != null ? kw.RootBraces : emitRootBraces);
					if (!noIndent) { if (obj.Count > 0) Nl(tw, level); else tw.Write(separator); }
					if (showBraces) tw.Write('{');
					else level--; // reduce level for root
					if (kw != null)
					{
						var kwl = GetWsc(kw.Comments, "");
						foreach (string key in System.Linq.Enumerable.Distinct(System.Linq.Enumerable.Concat(kw.Order, kw.Keys)))
						{
							if (!obj.ContainsKey(key)) continue;
							var val = obj[key];
							tw.Write(kwl);
							Nl(tw, level + 1);
							kwl = GetWsc(kw.Comments, key);

							tw.Write(EscapeName(key));
							tw.Write(":");
							Save(val, tw, level + 1, TestWsc(kwl), " ");
						}
						tw.Write(kwl);
						if (showBraces) Nl(tw, level);
					}
					else
					{
						bool skipFirst = !showBraces;
						foreach (JsonPair pair in obj)
						{
							if (!skipFirst) Nl(tw, level + 1); else skipFirst = false;
							tw.Write(EscapeName(pair.Key));
							tw.Write(":");
							Save(pair.Value, tw, level + 1, false, " ");
						}
						if (showBraces && obj.Count > 0) Nl(tw, level);
					}
					if (showBraces) tw.Write('}');
					break;
				case JsonType.Array:
					int i = 0, n = value.Count;
					if (!noIndent) { if (n > 0) Nl(tw, level); else tw.Write(separator); }
					tw.Write('[');
					WscJsonArray whiteL = null;
					string wsl = null;
					if (writeWsc)
					{
						whiteL = value as WscJsonArray;
						if (whiteL != null) wsl = GetWsc(whiteL.Comments, 0);
					}
					for (; i < n; i++)
					{
						var v = value[i];
						if (whiteL != null)
						{
							tw.Write(wsl);
							wsl = GetWsc(whiteL.Comments, i + 1);
						}
						Nl(tw, level + 1);
						Save(v, tw, level + 1, wsl != null && TestWsc(wsl), "", true);
					}
					if (whiteL != null) tw.Write(wsl);
					if (n > 0) Nl(tw, level);
					tw.Write(']');
					break;
				case JsonType.Boolean:
					tw.Write(separator);
					tw.Write((bool) value ? "true" : "false");
					break;
				case JsonType.String:
					WriteString(((JsonPrimitive) value).GetRawString(), tw, level, hasComment, separator);
					break;
				default:
					tw.Write(separator);
					tw.Write(((JsonPrimitive) value).GetRawString());
					break;
			}
		}

		static string EscapeName(string name)
		{
			if (name.Length == 0 || needsEscapeName.IsMatch(name))
				return "\"" + JsonWriter.EscapeString(name) + "\"";
			else
				return name;
		}

		void WriteString(string value, TextWriter tw, int level, bool hasComment, string separator)
		{
			if (value == "") { tw.Write(separator + "\"\""); return; }

			char left = value[0], right = value[value.Length - 1];
			char left1 = value.Length > 1 ? value[1] : '\0', left2 = value.Length > 2 ? value[2] : '\0';
			bool doEscape = hasComment || System.Linq.Enumerable.Any(value, c => NeedsQuotes(c));

			if (doEscape ||
			  BaseReader.IsWhite(left) || BaseReader.IsWhite(right) ||
			  left == '"' ||
			  left == '\'' ||
			  left == '#' ||
			  left == '/' && (left1 == '*' || left1 == '/') ||
			  HjsonValue.IsPunctuatorChar(left) ||
			  HjsonReader.TryParseNumericLiteral(value, true, out JsonValue dummy) ||
			  StartsWithKeyword(value))
			{
				// If the string contains no control characters, no quote characters, and no
				// backslash characters, then we can safely slap some quotes around it.
				// Otherwise we first check if the string can be expressed in multiline
				// format or we must replace the offending characters with safe escape
				// sequences.

				if (!System.Linq.Enumerable.Any(value, c => NeedsEscape(c))) tw.Write(separator + "\"" + value + "\"");
				else if (!System.Linq.Enumerable.Any(value, c => NeedsEscapeML(c)) && !value.Contains("'''") && !System.Linq.Enumerable.All(value, c => BaseReader.IsWhite(c))) WriteMLString(value, tw, level, separator);
				else tw.Write(separator + "\"" + JsonWriter.EscapeString(value) + "\"");
			}
			else tw.Write(separator + value);
		}

		void WriteMLString(string value, TextWriter tw, int level, string separator)
		{
			var lines = value.Replace("\r", "").Split('\n');

			if (lines.Length == 1)
			{
				tw.Write(separator + "'''");
				tw.Write(lines[0]);
				tw.Write("'''");
			}
			else
			{
				level++;
				Nl(tw, level);
				tw.Write("'''");

				foreach (var line in lines)
				{
					Nl(tw, !string.IsNullOrEmpty(line) ? level : 0);
					tw.Write(line);
				}
				Nl(tw, level);
				tw.Write("'''");
			}
		}

		static bool StartsWithKeyword(string text)
		{
			int p;
			if (text.StartsWith("true") || text.StartsWith("null")) p = 4;
			else if (text.StartsWith("false")) p = 5;
			else return false;
			while (p < text.Length && BaseReader.IsWhite(text[p])) p++;
			if (p == text.Length) return true;
			char ch = text[p];
			return ch == ',' || ch == '}' || ch == ']' || ch == '#' || ch == '/' && (text.Length > p + 1 && (text[p + 1] == '/' || text[p + 1] == '*'));
		}

		static bool NeedsQuotes(char c)
		{
			switch (c)
			{
				case '\t':
				case '\f':
				case '\b':
				case '\n':
				case '\r':
					return true;
				default:
					return false;
			}
		}

		static bool NeedsEscape(char c)
		{
			switch (c)
			{
				case '\"':
				case '\\':
					return true;
				default:
					return NeedsQuotes(c);
			}
		}

		static bool NeedsEscapeML(char c)
		{
			switch (c)
			{
				case '\n':
				case '\r':
				case '\t':
					return false;
				default:
					return NeedsQuotes(c);
			}
		}
	}
}
