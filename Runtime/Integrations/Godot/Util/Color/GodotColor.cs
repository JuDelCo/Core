// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

namespace Ju.Color
{
	public partial struct Color
	{
		public static implicit operator Godot.Color(Color color)
		{
			return new Godot.Color(color.r, color.g, color.b, color.a);
		}

		public static implicit operator Color(Godot.Color color)
		{
			return new Color(color.R, color.G, color.B, color.A);
		}
	}
}

#endif
