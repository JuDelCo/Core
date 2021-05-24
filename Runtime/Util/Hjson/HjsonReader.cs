// SPDX-License-Identifier: MIT
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Copyright (c) 2021 Juan Delgado (@JuDelCo)
// Based on System.Json from https://github.com/mono/mono (MIT X11)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ju.Hjson
{
	using JsonPair = KeyValuePair<string, JsonValue>;

	internal class HjsonReader : BaseReader
	{
		readonly StringBuilder sb = new StringBuilder();
		readonly IEnumerable<IHjsonDsfProvider> dsfProviders = Enumerable.Empty<IHjsonDsfProvider>();

		public HjsonReader(TextReader reader, IJsonReader jsonReader, HjsonOptions options)
		  : base(reader, jsonReader)
		{
			if (options != null)
			{
				ReadWsc = options.KeepWsc;
				dsfProviders = options.DsfProviders;
			}
		}

		public JsonValue Read()
		{
			// Braces for the root object are optional

			int c = SkipPeekChar();
			switch (c)
			{
				case '[':
				case '{':
					return CheckTrailing(ReadCore());
				default:
					try
					{
						// assume we have a root object without braces
						return CheckTrailing(ReadCore(true));
					}
					catch (Exception)
					{
						// test if we are dealing with a single JSON value instead (true/false/null/num/"")
						Reset();
						try { return CheckTrailing(ReadCore()); }
						catch (Exception) { }
						throw; // throw original error
					}
			}
		}

		JsonValue CheckTrailing(JsonValue v)
		{
			SkipWhite2();
			if (ReadChar() >= 0) throw ParseError("Extra characters in input");
			return v;
		}

		void SkipWhite2()
		{
			while (PeekChar() >= 0)
			{
				while (IsWhite((char)PeekChar())) ReadChar();
				int p = PeekChar();
				if (p == '#' || p == '/' && PeekChar(1) == '/')
				{
					for (; ; )
					{
						var ch = PeekChar();
						if (ch < 0 || ch == '\n') break;
						ReadChar();
					}
				}
				else if (p == '/' && PeekChar(1) == '*')
				{
					ReadChar(); ReadChar();
					for (; ; )
					{
						var ch = PeekChar();
						if (ch < 0 || ch == '*' && PeekChar(1) == '/') break;
						ReadChar();
					}
					if (PeekChar() >= 0) { ReadChar(); ReadChar(); }
				}
				else break;
			}
		}

		protected override string GetWhite()
		{
			var res = base.GetWhite();
			int to = res.Length - 1;
			if (to >= 0)
			{
				// remove trailing whitespace
				for (; to > 0 && res[to] <= ' ' && res[to] != '\n'; to--) ;
				// but only up to EOL
				if (res[to] == '\n') to--;
				if (to >= 0 && res[to] == '\r') to--;
				res = res.Substring(0, to + 1);
				foreach (char c in res) if (c > ' ') return res;
			}
			return "";
		}

		public override int SkipPeekChar()
		{
			SkipWhite2();
			return PeekChar();
		}

		JsonValue ReadCore(bool objectWithoutBraces = false)
		{
			int c = objectWithoutBraces ? '{' : SkipPeekChar();
			if (c < 0) throw ParseError("Incomplete input");
			switch (c)
			{
				case '[':
					JsonArray list;
					WscJsonArray wscL = null;
					ReadChar();
					ResetWhite();
					if (ReadWsc) list = wscL = new WscJsonArray();
					else list = new JsonArray();
					SkipPeekChar();
					if (ReadWsc) wscL.Comments.Add(GetWhite());
					for (int i = 0; ; i++)
					{
						if (SkipPeekChar() == ']') { ReadChar(); break; }
						if (HasReader) Reader.Index(i);
						var value = ReadCore();
						if (HasReader) Reader.Value(value);
						list.Add(value);
						ResetWhite();
						if (SkipPeekChar() == ',') { ReadChar(); ResetWhite(); SkipPeekChar(); }
						if (ReadWsc) wscL.Comments.Add(GetWhite());
					}
					return list;
				case '{':
					JsonObject obj;
					WscJsonObject wsc = null;
					if (!objectWithoutBraces)
					{
						ReadChar();
						ResetWhite();
					}
					if (ReadWsc) obj = wsc = new WscJsonObject() { RootBraces = !objectWithoutBraces };
					else obj = new JsonObject();
					SkipPeekChar();
					if (ReadWsc) wsc.Comments[""] = GetWhite();
					for (; ; )
					{
						if (objectWithoutBraces) { if (SkipPeekChar() < 0) break; }
						else if (SkipPeekChar() == '}') { ReadChar(); break; }
						string name = ReadKeyName();
						SkipWhite2();
						Expect(':');
						SkipWhite2();
						if (HasReader) Reader.Key(name);
						var value = ReadCore();
						if (HasReader) Reader.Value(value);
						obj.Add(new JsonPair(name, value));
						ResetWhite();
						if (SkipPeekChar() == ',') { ReadChar(); ResetWhite(); SkipPeekChar(); }
						if (ReadWsc) { wsc.Comments[name] = GetWhite(); wsc.Order.Add(name); }
					}
					return obj;
				case '\'':
				case '"': return ReadStringLiteral(ReadMlString);
				default: return ReadTfnns(c);
			}
		}

		string ReadKeyName()
		{
			// quotes for keys are optional in Hjson
			// unless they include {}[],: or whitespace.

			int c = PeekChar();
			if (c == '"' || c == '\'') return ReadStringLiteral(null);

			sb.Length = 0;
			int space = -1;
			for (; ; )
			{
				c = PeekChar();
				if (c < 0) throw ParseError("Name is not closed");
				char ch = (char)c;
				if (ch == ':')
				{
					if (sb.Length == 0) throw ParseError("Found ':' but no key name (for an empty key name use quotes)");
					else if (space >= 0 && space != sb.Length) throw ParseError("Found whitespace in your key name (use quotes to include)");
					return sb.ToString();
				}
				else if (IsWhite(ch))
				{
					if (space < 0) space = sb.Length;
					ReadChar();
				}
				else if (HjsonValue.IsPunctuatorChar(ch))
					throw ParseError("Found '" + ch + "' where a key name was expected (check your syntax or use quotes if the key name includes {}[],: or whitespace)");
				else
				{
					ReadChar();
					sb.Append(ch);
				}
			}
		}

		void SkipIndent(int indent)
		{
			while (indent-- > 0)
			{
				char c = (char)PeekChar();
				if (IsWhite(c) && c != '\n') ReadChar();
				else break;
			}
		}

		string ReadMlString()
		{
			// Parse a multiline string value.
			int triple = 0;
			sb.Length = 0;

			// we are at '''
			var indent = Column - 3;

			// skip white/to (newline)
			for (; ; )
			{
				char c = (char)PeekChar();
				if (IsWhite(c) && c != '\n') ReadChar();
				else break;
			}
			if (PeekChar() == '\n') { ReadChar(); SkipIndent(indent); }

			// When parsing for string values, we must look for " and \ characters.
			while (true)
			{
				int ch = PeekChar();
				if (ch < 0) throw ParseError("Bad multiline string");
				else if (ch == '\'')
				{
					triple++;
					ReadChar();
					if (triple == 3)
					{
						if (sb[sb.Length - 1] == '\n') sb.Length--;
						return sb.ToString();
					}
					else continue;
				}
				else
				{
					while (triple > 0)
					{
						sb.Append('\'');
						triple--;
					}
				}
				if (ch == '\n')
				{
					sb.Append('\n');
					ReadChar();
					SkipIndent(indent);
				}
				else
				{
					if (ch != '\r') sb.Append((char)ch);
					ReadChar();
				}
			}
		}

		internal static bool TryParseNumericLiteral(string text, bool stopAtNext, out JsonValue value)
		{
			int c, leadingZeros = 0, p = 0;
			double val = 0;
			bool negative = false, testLeading = true;
			text += '\0';
			value = null;

			if (text[p] == '-')
			{
				negative = true;
				p++;
				if (text[p] == 0) return false;
			}

			for (int x = 0; ; x++)
			{
				c = text[p];
				if (c < '0' || c > '9') break;
				if (testLeading)
				{
					if (c == '0') leadingZeros++;
					else testLeading = false;
				}
				val = val * 10 + (c - '0');
				p++;
			}
			if (testLeading) leadingZeros--; // single 0 is allowed
			if (leadingZeros > 0) return false;

			// fraction
			if (text[p] == '.')
			{
				if (leadingZeros < 0) return false;
				int fdigits = 0;
				double frac = 0;
				p++;
				if (text[p] == 0) return false;
				double d = 10;
				for (; ; )
				{
					c = text[p];
					if (c < '0' || '9' < c) break;
					p++;
					frac += (c - '0') / d;
					d *= 10;
					fdigits++;
				}
				if (fdigits == 0) return false;
				val += frac;
			}

			c = text[p];
			if (c == 'e' || c == 'E')
			{
				// exponent
				int exp = 0, expSign = 1;

				p++;
				if (text[p] == 0) return false;

				c = text[p];
				if (c == '-')
				{
					p++;
					expSign = -1;
				}
				else if (c == '+') p++;

				if (text[p] == 0) return false;

				for (; ; )
				{
					c = text[p];
					if (c < '0' || c > '9') break;
					exp = exp * 10 + (c - '0');
					p++;
				}

				if (exp != 0)
					val *= Math.Pow(10, exp * expSign);
			}

			while (p < text.Length && IsWhite(text[p])) p++;

			bool foundStop = false;
			if (p < text.Length && stopAtNext)
			{
				// end scan if we find a control character like ,}] or a comment
				char ch = text[p];
				if (ch == ',' || ch == '}' || ch == ']' || ch == '#' || ch == '/' && (text.Length > p + 1 && (text[p + 1] == '/' || text[p + 1] == '*')))
					foundStop = true;
			}

			if (p + 1 != text.Length && !foundStop) return false;

			if (negative)
			{
				if (val == 0.0) { value = -0.0; return true; }
				val *= -1;
			}

			long lval = (long)val;
			if (lval == val)
			{
				if (lval > int.MaxValue) value = lval;
				else value = (int)lval;
			}
			else
			{
				if (val > float.MaxValue) value = val;
				else value = (float)val;
			}

			return true;
		}

		JsonValue ReadTfnns(int c)
		{
			if (HjsonValue.IsPunctuatorChar((char)c))
				throw ParseError("Found a punctuator character '" + c + "' when expecting a quoteless string (check your syntax)");

			sb.Length = 0;
			for (; ; )
			{
				bool isEol = c < 0 || c == '\n';
				if (isEol || c == ',' ||
				  c == '}' || c == ']' ||
				  c == '#' ||
				  c == '/' && (PeekChar(1) == '/' || PeekChar(1) == '*'))
				{
					if (sb.Length > 0)
					{
						char ch = sb[0];
						switch (ch)
						{
							case 'f': if (sb.ToString().Trim() == "false") return false; break;
							case 'n': if (sb.ToString().Trim() == "null") return null; break;
							case 't': if (sb.ToString().Trim() == "true") return true; break;
							default:
								if (ch == '-' || ch >= '0' && ch <= '9')
								{
									if (TryParseNumericLiteral(sb.ToString(), false, out JsonValue res)) return res;
								}
								break;
						}
					}
					if (isEol)
					{
						// remove any whitespace at the end (ignored in quoteless strings)
						return HjsonDsf.Parse(dsfProviders, sb.ToString().Trim());
					}
				}
				ReadChar();
				if (c != '\r') sb.Append((char)c);
				c = PeekChar();
			}
		}
	}
}
