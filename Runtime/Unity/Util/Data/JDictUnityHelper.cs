// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System.Collections.Generic;
using UnityEngine;

namespace Ju.Data
{
	public partial class JDict : JNode, IDictionary<string, JNode>
	{
		protected JData<UnityEngine.Color> GetDataUnityColor(string key)
		{
			return GetData<UnityEngine.Color>(key);
		}

		protected JData<Color32> GetDataUnityColor32(string key)
		{
			return GetData<Color32>(key);
		}

		protected JData<Vector2> GetDataVector2(string key)
		{
			return GetData<Vector2>(key);
		}

		protected JData<Vector2Int> GetDataVector2Int(string key)
		{
			return GetData<Vector2Int>(key);
		}

		protected JData<Vector3> GetDataVector3(string key)
		{
			return GetData<Vector3>(key);
		}

		protected JData<Vector3Int> GetDataVector3Int(string key)
		{
			return GetData<Vector3Int>(key);
		}

		protected JData<Vector4> GetDataVector4(string key)
		{
			return GetData<Vector4>(key);
		}
	}
}

#endif
