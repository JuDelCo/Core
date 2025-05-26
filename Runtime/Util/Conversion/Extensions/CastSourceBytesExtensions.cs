// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using Ju.Data.Conversion;

public static class CastSourceBytesExtensions
{
	public static string AsFormattedBytesString<T>(this CastSource<T> source, bool round = false) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
	{
		var input = (ulong) Convert.ChangeType(source.value, typeof(ulong));
		double result;
		string suffix;

		if (input >= 0x10000000000)
		{
			suffix = "TB";
			result = (input >> 30);
		}
		else if (input >= 0x40000000)
		{
			suffix = "GB";
			result = (input >> 20);
		}
		else if (input >= 0x100000)
		{
			suffix = "MB";
			result = (input >> 10);
		}
		else if (input >= 0x400)
		{
			suffix = "KB";
			result = input;
		}
		else
		{
			suffix = "B";
			result = input;
			result *= 1024;
		}

		result /= 1024;

		return result.ToString(round ? "0" : "0.00") + suffix;
	}

}
