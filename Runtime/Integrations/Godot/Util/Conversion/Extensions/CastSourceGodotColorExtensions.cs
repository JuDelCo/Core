// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

namespace Ju.Data.Conversion
{
	public static class CastSourceGodotColorExtensions
	{
		public static string AsString(this CastSource<Godot.Color> source, bool prependHashChar = true, bool toUpper = true)
		{
			return $"{(prependHashChar ? "#" : "")}{ConditionalCasing(source.value.ToHtml(true).TrimStart('#'), toUpper)}";
		}

		private static string ConditionalCasing(string value, bool toUpper)
		{
			return (toUpper ? value.ToUpper() : value.ToLower());
		}
	}
}

#endif
