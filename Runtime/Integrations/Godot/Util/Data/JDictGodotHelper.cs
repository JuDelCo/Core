// SPDX-License-Identifier: MIT
// Copyright (c) 2025-2025 Juan Delgado (@JuDelCo)

#if GODOT4_3_OR_GREATER

using System.Collections.Generic;
using Godot;

namespace Ju.Data
{
	public partial class JDict : JNode, IDictionary<string, JNode>
	{
		protected JData<Godot.Color> GetDataGodotColor(string key)
		{
			return GetData<Godot.Color>(key);
		}

		protected JData<Vector2> GetDataVector2(string key)
		{
			return GetData<Vector2>(key);
		}

		protected JData<Vector2I> GetDataVector2Int(string key)
		{
			return GetData<Vector2I>(key);
		}

		protected JData<Vector3> GetDataVector3(string key)
		{
			return GetData<Vector3>(key);
		}

		protected JData<Vector3I> GetDataVector3Int(string key)
		{
			return GetData<Vector3I>(key);
		}

		protected JData<Vector4> GetDataVector4(string key)
		{
			return GetData<Vector4>(key);
		}

		protected JData<Vector4I> GetDataVector4Int(string key)
		{
			return GetData<Vector4I>(key);
		}
	}
}

#endif
