// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using UnityEngine;

namespace Ju.Data.Conversion
{
	public static class CastSourceUnityStringExtensions
	{
		public static UnityEngine.Color AsColor(this CastSource<string> source, UnityEngine.Color defaultValue = default(UnityEngine.Color))
		{
			var value = source.value;

			if (source.value.Length <= 8 && !source.value.StartsWith("#"))
			{
				value = "#" + source.value;
			}

			return (ColorUtility.TryParseHtmlString(value, out UnityEngine.Color result)) ? result : defaultValue;
		}

		public static Color32 AsColor32(this CastSource<string> source, Color32 defaultValue = default(Color32))
		{
			return source.AsColor(defaultValue);
		}

		private static readonly char[] vectorStringDelimiters = new char[] { '(', ',', ')' };

		public static Vector2 AsVector2(this CastSource<string> source, Vector2 defaultValue = default(Vector2))
		{
			var result = defaultValue;
			var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

			if (splittedValue.Length == 2 && float.TryParse(splittedValue[0], out float x) && float.TryParse(splittedValue[1], out float y))
			{
				result = new Vector2(x, y);
			}

			return result;
		}

		public static Vector2Int AsVector2Int(this CastSource<string> source, Vector2Int defaultValue = default(Vector2Int))
		{
			var result = defaultValue;
			var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

			if (splittedValue.Length == 2 && int.TryParse(splittedValue[0], out int x) && int.TryParse(splittedValue[1], out int y))
			{
				result = new Vector2Int(x, y);
			}

			return result;
		}

		public static Vector3 AsVector3(this CastSource<string> source, Vector3 defaultValue = default(Vector3))
		{
			var result = defaultValue;
			var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

			if (splittedValue.Length == 3 && float.TryParse(splittedValue[0], out float x) && float.TryParse(splittedValue[1], out float y) && float.TryParse(splittedValue[2], out float z))
			{
				result = new Vector3(x, y, z);
			}

			return result;
		}

		public static Vector3Int AsVector3Int(this CastSource<string> source, Vector3Int defaultValue = default(Vector3Int))
		{
			var result = defaultValue;
			var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

			if (splittedValue.Length == 3 && int.TryParse(splittedValue[0], out int x) && int.TryParse(splittedValue[1], out int y) && int.TryParse(splittedValue[2], out int z))
			{
				result = new Vector3Int(x, y, z);
			}

			return result;
		}

		public static Vector4 AsVector4(this CastSource<string> source, Vector4 defaultValue = default(Vector4))
		{
			var result = defaultValue;
			var splittedValue = source.value.Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

			if (splittedValue.Length == 4 && float.TryParse(splittedValue[0], out float x) && float.TryParse(splittedValue[1], out float y) && float.TryParse(splittedValue[2], out float z) && float.TryParse(splittedValue[3], out float w))
			{
				result = new Vector4(x, y, z, w);
			}

			return result;
		}
	}
}

#endif
