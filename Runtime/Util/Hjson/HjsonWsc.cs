// SPDX-License-Identifier: MIT
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Based on System.Json from https://github.com/mono/mono (MIT X11)

using System.Collections.Generic;

namespace Ju.Hjson
{
	/// <summary>Implements an object value, including whitespace and comments.</summary>
	public class WscJsonObject : JsonObject
	{
		/// <summary>Initializes a new instance of this class.</summary>
		public WscJsonObject()
		{
			Order = new List<string>();
			Comments = new Dictionary<string, string>();
		}

		/// <summary>Defines if braces are shown (root object only).</summary>
		public bool RootBraces { get; set; }
		/// <summary>Defines the order of the keys.</summary>
		public List<string> Order { get; private set; }
		/// <summary>Defines a comment for each key. The "" entry is emitted before any element.</summary>
		public Dictionary<string, string> Comments { get; private set; }
	}

	/// <summary>Implements an array value, including whitespace and comments.</summary>
	public class WscJsonArray : JsonArray
	{
		/// <summary>Initializes a new instance of this class.</summary>
		public WscJsonArray()
		{
			Comments = new List<string>();
		}

		/// <summary>Defines a comment for each item. The [0] entry is emitted before any element.</summary>
		public List<string> Comments { get; private set; }
	}
}
