// SPDX-License-Identifier: MIT
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Copyright (c) 2021 Juan Delgado (@JuDelCo)
// Based on System.Json from https://github.com/mono/mono (MIT X11)

using System;
using System.Globalization;

namespace Ju.Hjson
{
	/// <summary>Implements a primitive value.</summary>
	internal class JsonPrimitive : JsonValue
	{
		object value;

		/// <summary>Initializes a new string.</summary>
		public JsonPrimitive(string value) { this.value = value; }
		/// <summary>Initializes a new char.</summary>
		public JsonPrimitive(char value) { this.value = value.ToString(); }
		/// <summary>Initializes a new bool.</summary>
		public JsonPrimitive(bool value) { this.value = value; }
		/// <summary>Initializes a new decimal.</summary>
		public JsonPrimitive(decimal value) { this.value = value; }
		/// <summary>Initializes a new double.</summary>
		public JsonPrimitive(double value) { this.value = value; }
		/// <summary>Initializes a new float.</summary>
		public JsonPrimitive(float value) { this.value = value; }
		/// <summary>Initializes a new long.</summary>
		public JsonPrimitive(long value) { this.value = value; }
		/// <summary>Initializes a new int.</summary>
		public JsonPrimitive(int value) { this.value = value; }
		/// <summary>Initializes a new byte.</summary>
		public JsonPrimitive(byte value) { this.value = value; }
		/// <summary>Initializes a new short.</summary>
		public JsonPrimitive(short value) { this.value = value; }

		JsonPrimitive() { }
		public static new JsonPrimitive FromObject(object value) { return new JsonPrimitive { value = value }; }

		internal object Value
		{
			get { return value; }
		}

		/// <summary>The type of this value.</summary>
		public override JsonType JsonType
		{
			get
			{
				if (value == null) return JsonType.String;

				var type = value.GetType();
				if (type == typeof(Boolean)) return JsonType.Boolean;
				if (type == typeof(String)) return JsonType.String;
				if (type == typeof(Byte) ||
				  type == typeof(SByte) ||
				  type == typeof(Int16) ||
				  type == typeof(UInt16) ||
				  type == typeof(Int32) ||
				  type == typeof(UInt32) ||
				  type == typeof(Int64) ||
				  type == typeof(UInt64) ||
				  type == typeof(Single) ||
				  type == typeof(Double) ||
				  type == typeof(Decimal)) return JsonType.Number;
				return JsonType.Unknown;
			}
		}

		internal string GetRawString()
		{
			switch (JsonType)
			{
				case JsonType.String:
					return ((string)value) ?? "";
				case JsonType.Number:
#if __MonoCS__ // mono bug ca 2014
          if (value is decimal)
          {
            var res=((IFormattable)value).ToString("G", NumberFormatInfo.InvariantInfo);
            while (res.EndsWith("0")) res=res.Substring(0, res.Length-1);
            if (res.EndsWith(".") || res.EndsWith("e", StringComparison.OrdinalIgnoreCase)) res=res.Substring(0, res.Length-1);
            return res.ToLowerInvariant();
          }
#endif
					// use ToLowerInvariant() to convert E to e
					return ((IFormattable)value).ToString("G", NumberFormatInfo.InvariantInfo).ToLowerInvariant();
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
