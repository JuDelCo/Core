// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Data;

public static class JNodeDataExtensions
{
	public static JData<T> AsDataEnum<T>(this JNode node) where T : Enum
	{
		return node.AsData<T>();
	}

	public static JData<bool> AsDataBool(this JNode node)
	{
		return node.AsData<bool>();
	}

	public static JData<byte> AsDataByte(this JNode node)
	{
		return node.AsData<byte>();
	}

	public static JData<sbyte> AsDataSByte(this JNode node)
	{
		return node.AsData<sbyte>();
	}

	public static JData<char> AsDataChar(this JNode node)
	{
		return node.AsData<char>();
	}

	public static JData<float> AsDataSingle(this JNode node)
	{
		return node.AsData<float>();
	}

	public static JData<float> AsDataFloat(this JNode node)
	{
		return node.AsData<float>();
	}

	public static JData<double> AsDataDouble(this JNode node)
	{
		return node.AsData<double>();
	}

	public static JData<double> AsDataFloat64(this JNode node)
	{
		return node.AsData<double>();
	}

	public static JData<decimal> AsDataDecimal(this JNode node)
	{
		return node.AsData<decimal>();
	}

	public static JData<decimal> AsDataFloat128(this JNode node)
	{
		return node.AsData<decimal>();
	}

	public static JData<short> AsDataShort(this JNode node)
	{
		return node.AsData<short>();
	}

	public static JData<short> AsDataInt16(this JNode node)
	{
		return node.AsData<short>();
	}

	public static JData<int> AsDataInt(this JNode node)
	{
		return node.AsData<int>();
	}

	public static JData<int> AsDataInt32(this JNode node)
	{
		return node.AsData<int>();
	}

	public static JData<long> AsDataLong(this JNode node)
	{
		return node.AsData<long>();
	}

	public static JData<long> AsDataInt64(this JNode node)
	{
		return node.AsData<long>();
	}

	public static JData<ushort> AsDataUShort(this JNode node)
	{
		return node.AsData<ushort>();
	}

	public static JData<ushort> AsDataUInt16(this JNode node)
	{
		return node.AsData<ushort>();
	}

	public static JData<uint> AsDataUInt(this JNode node)
	{
		return node.AsData<uint>();
	}

	public static JData<uint> AsDataUInt32(this JNode node)
	{
		return node.AsData<uint>();
	}

	public static JData<ulong> AsDataULong(this JNode node)
	{
		return node.AsData<ulong>();
	}

	public static JData<ulong> AsDataUInt64(this JNode node)
	{
		return node.AsData<ulong>();
	}

	public static JData<string> AsDataString(this JNode node)
	{
		return node.AsData<string>();
	}

	public static JData<DateTime> AsDataDateTime(this JNode node)
	{
		return node.AsData<DateTime>();
	}
}
