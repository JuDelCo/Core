// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Extensions;

namespace Ju.Util
{
	[Serializable]
	public struct SerializableType : IEquatable<SerializableType>
	{
		private static List<Type> typeCache = new List<Type>();
		private static List<string> nameCache = new List<string>();
		private static List<string> friendlyNameCache = new List<string>();
		private static List<SerializableType> serializableCache = new List<SerializableType>();

#if UNITY_2019_3_OR_NEWER
		[UnityEngine.SerializeReference]
#endif
		private string AssemblyQualifiedName;
		private string TypeFriendlyName;

		public Type Type => GetTypeFromFullName(AssemblyQualifiedName);
		public string Name => AssemblyQualifiedName;
		public string FriendlyName => TypeFriendlyName;

		public static string GetTypeFriendlyName(Type type)
		{
			if (!typeCache.Contains(type))
			{
				var name = type.AssemblyQualifiedName;
				var friendlyName = type.GetFriendlyName(false, false);
				typeCache.Add(type);
				nameCache.Add(name);
				friendlyNameCache.Add(friendlyName);
				serializableCache.Add(new SerializableType() { AssemblyQualifiedName = name, TypeFriendlyName = friendlyName });
			}

			return friendlyNameCache[typeCache.IndexOf(type)];
		}

		public static string GetTypeFullName(Type type)
		{
			if (!typeCache.Contains(type))
			{
				var name = type.AssemblyQualifiedName;
				var friendlyName = type.GetFriendlyName(false, false);
				typeCache.Add(type);
				nameCache.Add(name);
				friendlyNameCache.Add(friendlyName);
				serializableCache.Add(new SerializableType() { AssemblyQualifiedName = name, TypeFriendlyName = friendlyName });
			}

			return nameCache[typeCache.IndexOf(type)];
		}

		private static Type GetTypeFromFullName(string typeName)
		{
			if (!nameCache.Contains(typeName))
			{
				var type = Type.GetType(typeName);
				var friendlyName = type.GetFriendlyName(false, false);
				typeCache.Add(type);
				nameCache.Add(typeName);
				friendlyNameCache.Add(friendlyName);
				serializableCache.Add(new SerializableType() { AssemblyQualifiedName = typeName, TypeFriendlyName = friendlyName });
			}

			return typeCache[nameCache.IndexOf(typeName)];
		}

		public static implicit operator Type(SerializableType serializableType)
		{
			return serializableType.Type;
		}

		public static implicit operator SerializableType(Type type)
		{
			GetTypeFullName(type);
			return serializableCache[typeCache.IndexOf(type)];
		}

		public override int GetHashCode()
		{
			return AssemblyQualifiedName.GetHashCode();
		}

		public bool Equals(SerializableType other)
		{
			return (AssemblyQualifiedName == other.AssemblyQualifiedName);
		}

		public override bool Equals(object obj)
		{
			return (obj is SerializableType serializableType && (this == serializableType));
		}

		public static bool operator ==(SerializableType a, SerializableType b)
		{
			return (a.AssemblyQualifiedName == b.AssemblyQualifiedName);
		}

		public static bool operator !=(SerializableType a, SerializableType b)
		{
			return !(a == b);
		}
	}
}
