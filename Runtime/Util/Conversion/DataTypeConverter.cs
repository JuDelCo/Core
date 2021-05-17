// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.Data.Conversion
{
	using Ju.Log;

	public struct ConversionType
	{
		public Type fromType;
		public Type toType;

		public ConversionType(Type fromType, Type toType)
		{
			this.fromType = fromType;
			this.toType = toType;
		}

		public static ConversionType Between<TSource, TResult>()
		{
			return new ConversionType(typeof(TSource), typeof(TResult));
		}
	}

	public static partial class DataTypeConverter
	{
		private static Dictionary<ConversionType, Func<object, object>> converters;

		public static void AddConverter(ConversionType type, Func<object, object> converter)
		{
			if (HasConverter(type))
			{
				Log.Warning($"Overwriting existing converter between the types {type.fromType.Name} and {type.toType.Name}");

				converters[type] = converter;
			}
			else
			{
				converters.Add(type, converter);
			}
		}

		public static bool HasConverter(ConversionType type)
		{
			Initialize();

			return converters.ContainsKey(type);
		}

		public static Func<object, object> GetConverter(ConversionType type)
		{
			if (HasConverter(type))
			{
				return converters[type];
			}

			return null;
		}

		public static void RemoveConverter(ConversionType type)
		{
			if (HasConverter(type))
			{
				converters.Remove(type);
			}
		}
	}
}
