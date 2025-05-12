// SPDX-License-Identifier: MIT
// Copyright (c) 2021-2025 Juan Delgado (@JuDelCo)
// Copyright (c) 2014-2016 Christian Zangl
// Copyright (c) 2001-2003 Ximian, Inc
// Based on System.Json from https://github.com/mono/mono (MIT X11)

namespace Ju.Hjson
{
	/// <summary>The ToString format.</summary>
	public enum Stringify
	{
		/// <summary>JSON (no whitespace).</summary>
		Plain,
		/// <summary>Formatted JSON.</summary>
		Formatted,
		/// <summary>Hjson.</summary>
		Hjson,
	}
}
