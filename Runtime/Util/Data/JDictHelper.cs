// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.Data
{
	using Ju.Color;

	public partial class JDict : JNode, IDictionary<string, JNode>
	{
		protected T GetDict<T>(string key) where T : JDict, new()
		{
			if (!initialized)
			{
				this[key] = new T();
			}

			return this[key].AsDict() as T;
		}

		protected JDict GetDict(string key, bool reseteable = false, int capacity = 0, bool subscribeToChildren = true)
		{
			if (!initialized)
			{
				this[key] = new JDict(reseteable, capacity, subscribeToChildren);
			}

			return this[key].AsDict();
		}

		protected JDict GetDict(string key, int capacity = 0)
		{
			return GetDict(key, false, capacity);
		}

		protected JList GetList(string key, bool reseteable = true, int capacity = 0, bool subscribeToChildren = true)
		{
			if (!initialized)
			{
				this[key] = new JList(reseteable, capacity, subscribeToChildren);
			}

			return this[key].AsList();
		}

		protected JList GetList(string key, int capacity = 0)
		{
			return GetList(key, true, capacity);
		}

		protected JList<T> GetList<T>(string key, bool reseteable = true, int capacity = 0, bool subscribeToChildren = true) where T : JNode
		{
			if (!initialized)
			{
				this[key] = new JList<T>(reseteable, capacity, subscribeToChildren);
			}

			return this[key] as JList<T>;
		}

		protected JList<T> GetList<T>(string key, int capacity = 0) where T : JNode
		{
			return GetList<T>(key, true, capacity);
		}

		protected JData<T> GetData<T>(string key, T defaultValue = default(T)) where T : struct
		{
			if (!initialized)
			{
				this[key] = new JData<T>(defaultValue, defaultValue);
			}

			return this[key].AsData<T>();
		}

		protected JData<T> GetData<T>(string key, T defaultValue, T value = default(T)) where T : struct
		{
			if (!initialized)
			{
				this[key] = new JData<T>(value, defaultValue);
			}

			return this[key].AsData<T>();
		}

		protected JData<T> GetDataClass<T>(string key, Func<T> defaultValueFactory = null) where T : class
		{
			if (!initialized)
			{
				T value = (defaultValueFactory != null ? defaultValueFactory() : null);
				T defaultValue = (defaultValueFactory != null ? defaultValueFactory() : null);

				this[key] = new JData<T>(value, defaultValue);
			}

			return this[key].AsData<T>();
		}

		protected JData<T> GetDataClass<T>(string key, Func<T> defaultValueFactory, Func<T> valueFactory = null) where T : class
		{
			if (!initialized)
			{
				T value = (valueFactory != null ? valueFactory() : null);
				T defaultValue = (defaultValueFactory != null ? defaultValueFactory() : null);

				this[key] = new JData<T>(value, defaultValue);
			}

			return this[key].AsData<T>();
		}

		protected JData<string> GetDataString(string key)
		{
			return GetData<string>(key);
		}

		protected JData<bool> GetDataBool(string key)
		{
			return GetData<bool>(key);
		}

		protected JData<byte> GetDataByte(string key)
		{
			return GetData<byte>(key);
		}

		protected JData<sbyte> GetDataSByte(string key)
		{
			return GetData<sbyte>(key);
		}

		protected JData<char> GetDataChar(string key)
		{
			return GetData<char>(key);
		}

		protected JData<float> GetDataSingle(string key)
		{
			return GetData<float>(key);
		}

		protected JData<float> GetDataFloat(string key)
		{
			return GetData<float>(key);
		}

		protected JData<double> GetDataDouble(string key)
		{
			return GetData<double>(key);
		}

		protected JData<double> GetDataFloat64(string key)
		{
			return GetData<double>(key);
		}

		protected JData<decimal> GetDataDecimal(string key)
		{
			return GetData<decimal>(key);
		}

		protected JData<decimal> GetDataFloat128(string key)
		{
			return GetData<decimal>(key);
		}

		protected JData<short> GetDataShort(string key)
		{
			return GetData<short>(key);
		}

		protected JData<short> GetDataInt16(string key)
		{
			return GetData<short>(key);
		}

		protected JData<int> GetDataInt(string key)
		{
			return GetData<int>(key);
		}

		protected JData<int> GetDataInt32(string key)
		{
			return GetData<int>(key);
		}

		protected JData<long> GetDataLong(string key)
		{
			return GetData<long>(key);
		}

		protected JData<long> GetDataInt64(string key)
		{
			return GetData<long>(key);
		}

		protected JData<ushort> GetDataUShort(string key)
		{
			return GetData<ushort>(key);
		}

		protected JData<ushort> GetDataUInt16(string key)
		{
			return GetData<ushort>(key);
		}

		protected JData<uint> GetDataUInt(string key)
		{
			return GetData<uint>(key);
		}

		protected JData<uint> GetDataUInt32(string key)
		{
			return GetData<uint>(key);
		}

		protected JData<ulong> GetDataULong(string key)
		{
			return GetData<ulong>(key);
		}

		protected JData<ulong> GetDataUInt64(string key)
		{
			return GetData<ulong>(key);
		}

		protected JData<DateTime> GetDataDateTime(string key)
		{
			return GetData<DateTime>(key);
		}

		protected JData<Guid> GetDataGuid(string key)
		{
			return GetData<Guid>(key);
		}

		protected JData<Color> GetDataColor(string key)
		{
			return GetData<Color>(key);
		}

		protected JData<Color32> GetDataColor32(string key)
		{
			return GetData<Color32>(key);
		}

		private JData<T> GetData<T>(string key)
		{
			if (!initialized)
			{
				this[key] = new JData<T>();
			}

			return this[key].AsData<T>();
		}
	}
}
