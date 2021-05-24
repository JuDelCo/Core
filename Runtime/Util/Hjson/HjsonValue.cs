// SPDX-License-Identifier: MIT
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Based on System.Json from https://github.com/mono/mono (MIT X11)

using System;
using System.IO;

namespace Ju.Hjson
{
	/// <summary>Contains functions to load and save in the Hjson format.</summary>
	public static class HjsonValue
	{
		/// <summary>Loads Hjson/JSON from a file.</summary>
		public static JsonValue Load(string path)
		{
			return Load(path, null, null);
		}

		/// <summary>Loads Hjson/JSON from a file, optionally preserving whitespace and comments.</summary>
		public static JsonValue Load(string path, HjsonOptions options)
		{
			return Load(path, null, options);
		}

		/// <summary>Loads Hjson/JSON from a stream.</summary>
		public static JsonValue Load(Stream stream)
		{
			return Load(stream, null, null);
		}

		/// <summary>Loads Hjson/JSON from a stream, optionally preserving whitespace and comments.</summary>
		public static JsonValue Load(Stream stream, HjsonOptions options)
		{
			return Load(stream, null, options);
		}

		/// <summary>Loads Hjson/JSON from a TextReader.</summary>
		public static JsonValue Load(TextReader textReader, IJsonReader jsonReader = null)
		{
			return Load(textReader, jsonReader, null);
		}

		/// <summary>Loads Hjson/JSON from a TextReader, optionally preserving whitespace and comments.</summary>
		public static JsonValue Load(TextReader textReader, HjsonOptions options, IJsonReader jsonReader = null)
		{
			return Load(textReader, jsonReader, options);
		}

		/// <summary>Loads Hjson/JSON from a TextReader, preserving whitespace and comments.</summary>
		[Obsolete("Use Load", true)]
		public static JsonValue LoadWsc(TextReader textReader)
		{
			return Load(textReader, null, new HjsonOptions { KeepWsc = true });
		}

		static JsonValue Load(string path, IJsonReader jsonReader, HjsonOptions options)
		{
			if (Path.GetExtension(path).ToLower() == ".json") return JsonValue.Load(path);
			try
			{
				using (var s = File.OpenRead(path))
					return Load(s, jsonReader, options);
			}
			catch (Exception e) { throw new Exception(e.Message + " (in " + path + ")", e); }
		}

		static JsonValue Load(Stream stream, IJsonReader jsonReader, HjsonOptions options)
		{
			if (stream == null) throw new ArgumentNullException("stream");
			return Load(new StreamReader(stream, true), jsonReader, options);
		}

		static JsonValue Load(TextReader textReader, IJsonReader jsonReader, HjsonOptions options)
		{
			if (textReader == null) throw new ArgumentNullException("textReader");
			return new HjsonReader(textReader, jsonReader, options).Read();
		}

		/// <summary>Parses the specified Hjson/JSON string.</summary>
		public static JsonValue Parse(string hjsonString)
		{
			if (hjsonString == null) throw new ArgumentNullException("hjsonString");
			return Load(new StringReader(hjsonString));
		}

		/// <summary>Parses the specified Hjson/JSON string, optionally preserving whitespace and comments.</summary>
		public static JsonValue Parse(string hjsonString, HjsonOptions options)
		{
			if (hjsonString == null) throw new ArgumentNullException("hjsonString");
			return Load(new StringReader(hjsonString), options);
		}

		/// <summary>Saves Hjson to a file.</summary>
		public static void Save(JsonValue json, string path, HjsonOptions options = null)
		{
			if (Path.GetExtension(path).ToLower() == ".json") { json.Save(path, Stringify.Formatted); return; }
			using (var s = File.CreateText(path))
				Save(json, s, options);
		}

		/// <summary>Saves Hjson to a stream.</summary>
		public static void Save(JsonValue json, Stream stream, HjsonOptions options = null)
		{
			if (stream == null) throw new ArgumentNullException("stream");
			Save(json, new StreamWriter(stream), options);
		}

		/// <summary>Saves Hjson to a TextWriter.</summary>
		public static void Save(JsonValue json, TextWriter textWriter, HjsonOptions options = null)
		{
			if (textWriter == null) throw new ArgumentNullException("textWriter");
			new HjsonWriter(options).Save(json, textWriter, 0, false, "", true, true);
			textWriter.Flush();
		}

		internal static bool IsPunctuatorChar(char ch)
		{
			return ch == '{' || ch == '}' || ch == '[' || ch == ']' || ch == ',' || ch == ':';
		}

		#region obsolete

		/// <summary>Loads Hjson/JSON from a file, optionally preserving whitespace and comments.</summary>
		[Obsolete("Use HjsonOptions for preserveComments")]
		public static JsonValue Load(string path, bool preserveComments)
		{
			return Load(path, null, new HjsonOptions { KeepWsc = preserveComments });
		}

		/// <summary>Loads Hjson/JSON from a stream, optionally preserving whitespace and comments.</summary>
		[Obsolete("Use HjsonOptions for preserveComments")]
		public static JsonValue Load(Stream stream, bool preserveComments)
		{
			return Load(stream, null, new HjsonOptions { KeepWsc = preserveComments });
		}

		/// <summary>Loads Hjson/JSON from a TextReader, optionally preserving whitespace and comments.</summary>
		[Obsolete("Use HjsonOptions for preserveComments")]
		public static JsonValue Load(TextReader textReader, bool preserveComments, IJsonReader jsonReader = null)
		{
			return Load(textReader, jsonReader, new HjsonOptions { KeepWsc = preserveComments });
		}

		/// <summary>Parses the specified Hjson/JSON string, optionally preserving whitespace and comments.</summary>
		[Obsolete("Use HjsonOptions for preserveComments")]
		public static JsonValue Parse(string hjsonString, bool preserveComments)
		{
			if (hjsonString == null) throw new ArgumentNullException("hjsonString");
			return Load(new StringReader(hjsonString), new HjsonOptions { KeepWsc = preserveComments });
		}

		#endregion
	}
}
