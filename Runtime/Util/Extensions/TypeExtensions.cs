// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Extensions
{
	public static class TypeExtensions
	{
		public static string GetFriendlyName(this Type type, bool aliasNullable = true, bool includeSpaceAfterComma = true)
		{
			TryGetInnerElementType(ref type, out string arrayBrackets);

			if (!TryGetNameAliasNonArray(type, out string friendlyName))
			{
				if (!type.IsGenericType)
				{
					friendlyName = type.Name;
				}
				else
				{
					if (aliasNullable && type.GetGenericTypeDefinition() == typeof(System.Nullable<>))
					{
						string generics = GetFriendlyName(type.GetGenericArguments()[0]);
						friendlyName = generics + "?";
					}
					else
					{
						string generics = GetFriendlyGenericArguments(type, includeSpaceAfterComma);
						int iBacktick = type.Name.IndexOf('`');
						friendlyName = (iBacktick > 0 ? type.Name.Remove(iBacktick) : type.Name) + $"<{generics}>";
					}
				}
			}

			return friendlyName + arrayBrackets;
		}

		private static string GetFriendlyGenericArguments(Type type, bool includeSpaceAfterComma)
		{
			return string.Join(includeSpaceAfterComma ? ", " : ",", type.GetGenericArguments().Map(t => t.GetFriendlyName()));
		}

		private static bool TryGetNameAliasNonArray(Type type, out string alias)
		{
			return ((alias = typeAliases[(int)Type.GetTypeCode(type)]) != null) && !type.IsEnum;
		}

		private static bool TryGetInnerElementType(ref Type type, out string arrayBrackets)
		{
			arrayBrackets = null;

			if (!type.IsArray)
			{
				return false;
			}

			do
			{
				arrayBrackets += "[" + new string(',', type.GetArrayRank() - 1) + "]";
				type = type.GetElementType();
			}
			while (type.IsArray);

			return true;
		}

		private static readonly string[] typeAliases =
		{
			"void",     // 0
			null,       // 1 (any other type)
			"DBNull",   // 2
			"bool",     // 3
			"char",     // 4
			"sbyte",    // 5
			"byte",     // 6
			"short",    // 7
			"ushort",   // 8
			"int",      // 9
			"uint",     // 10
			"long",     // 11
			"ulong",    // 12
			"float",    // 13
			"double",   // 14
			"decimal",  // 15
			null,       // 16 (DateTime)
			null,       // 17 (-undefined-)
			"string",   // 18
		};
	}
}
