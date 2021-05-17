// SPDX-License-Identifier: MIT
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Based on System.Json from https://github.com/mono/mono (MIT X11)

namespace Ju.Hjson
{
	/// <summary>Defines the known json types.</summary>
	/// <remarks>There is no null type as the primitive will be null instead of the JsonPrimitive containing null.</remarks>
	public enum JsonType
	{
		/// <summary>Json value of type string.</summary>
		String,
		/// <summary>Json value of type number.</summary>
		Number,
		/// <summary>Json value of type object.</summary>
		Object,
		/// <summary>Json value of type array.</summary>
		Array,
		/// <summary>Json value of type boolean.</summary>
		Boolean,
		/// <summary>Json value of an unknown type.</summary>
		Unknown,
	}
}
