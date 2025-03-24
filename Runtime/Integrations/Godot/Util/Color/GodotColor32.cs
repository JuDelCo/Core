// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

namespace Ju.Color
{
	public partial struct Color32
	{
		public static implicit operator Godot.Color(Color32 color)
		{
			return new Godot.Color(color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
		}

		public static implicit operator Color32(Godot.Color color)
		{
			return new Color32(color.R8, color.G8, color.B8, color.A8);
		}
	}
}

#endif
