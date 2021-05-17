// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;

namespace Ju.Data.Conversion
{
	public static partial class DataTypeConverter
	{
		private static bool initialized = false;
		private static readonly char[] vectorStringDelimiters = new char[] { '(', ',', ')' };

		private static string GetExceptionMsgBetween<TSource, TResult>()
		{
			return $"Error converting from type {typeof(TSource).Name} to type {typeof(TResult).Name}";
		}

		private static void Initialize()
		{
			if (initialized)
			{
				return;
			}

			converters = new Dictionary<ConversionType, Func<object, object>>();
			initialized = true;

			AddConverter(ConversionType.Between<bool, byte>(), o => (byte)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, sbyte>(), o => (sbyte)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, float>(), o => (float)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, double>(), o => (double)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, decimal>(), o => (decimal)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, short>(), o => (short)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, int>(), o => (int)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, long>(), o => (long)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, ushort>(), o => (ushort)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, uint>(), o => (uint)((bool)o ? 1 : 0));
			AddConverter(ConversionType.Between<bool, ulong>(), o => (ulong)((bool)o ? 1 : 0));

			AddConverter(ConversionType.Between<byte, bool>(), o => ((byte)o > 0));
			AddConverter(ConversionType.Between<byte, sbyte>(), o => (byte)o <= sbyte.MaxValue ? (sbyte)(byte)o : throw new Exception(GetExceptionMsgBetween<byte, sbyte>()));
			AddConverter(ConversionType.Between<byte, float>(), o => (float)(byte)o);
			AddConverter(ConversionType.Between<byte, double>(), o => (double)(byte)o);
			AddConverter(ConversionType.Between<byte, decimal>(), o => (decimal)(byte)o);
			AddConverter(ConversionType.Between<byte, short>(), o => (short)(byte)o);
			AddConverter(ConversionType.Between<byte, int>(), o => (int)(byte)o);
			AddConverter(ConversionType.Between<byte, long>(), o => (long)(byte)o);
			AddConverter(ConversionType.Between<byte, ushort>(), o => (ushort)(byte)o);
			AddConverter(ConversionType.Between<byte, uint>(), o => (uint)(byte)o);
			AddConverter(ConversionType.Between<byte, ulong>(), o => (ulong)(byte)o);

			AddConverter(ConversionType.Between<sbyte, bool>(), o => ((sbyte)o > 0));
			AddConverter(ConversionType.Between<sbyte, byte>(), o => (sbyte)o >= 0 ? (sbyte)o : throw new Exception(GetExceptionMsgBetween<sbyte, byte>()));
			AddConverter(ConversionType.Between<sbyte, float>(), o => (float)(sbyte)o);
			AddConverter(ConversionType.Between<sbyte, double>(), o => (double)(sbyte)o);
			AddConverter(ConversionType.Between<sbyte, decimal>(), o => (decimal)(sbyte)o);
			AddConverter(ConversionType.Between<sbyte, short>(), o => (short)(sbyte)o);
			AddConverter(ConversionType.Between<sbyte, int>(), o => (int)(sbyte)o);
			AddConverter(ConversionType.Between<sbyte, long>(), o => (long)(sbyte)o);
			AddConverter(ConversionType.Between<sbyte, ushort>(), o => (sbyte)o >= 0 ? (ushort)(sbyte)o : throw new Exception(GetExceptionMsgBetween<sbyte, ushort>()));
			AddConverter(ConversionType.Between<sbyte, uint>(), o => (sbyte)o >= 0 ? (uint)(sbyte)o : throw new Exception(GetExceptionMsgBetween<sbyte, uint>()));
			AddConverter(ConversionType.Between<sbyte, ulong>(), o => (sbyte)o >= 0 ? (ulong)(sbyte)o : throw new Exception(GetExceptionMsgBetween<sbyte, ulong>()));

			AddConverter(ConversionType.Between<float, bool>(), o => ((float)o > 0));
			AddConverter(ConversionType.Between<float, byte>(), o => (float)o >= byte.MinValue && (float)o <= byte.MaxValue && (float)o == (byte)(float)o ? (byte)(float)o : throw new Exception(GetExceptionMsgBetween<float, byte>()));
			AddConverter(ConversionType.Between<float, sbyte>(), o => (float)o == (sbyte)(float)o ? (sbyte)(float)o : throw new Exception(GetExceptionMsgBetween<float, sbyte>()));
			AddConverter(ConversionType.Between<float, double>(), o => (double)(float)o);
			AddConverter(ConversionType.Between<float, decimal>(), o => (decimal)(float)o);
			AddConverter(ConversionType.Between<float, short>(), o => (float)o == (short)(float)o ? (short)(float)o : throw new Exception(GetExceptionMsgBetween<float, short>()));
			AddConverter(ConversionType.Between<float, int>(), o => (float)o == (int)(float)o ? (int)(float)o : throw new Exception(GetExceptionMsgBetween<float, int>()));
			AddConverter(ConversionType.Between<float, long>(), o => (float)o == (long)(float)o ? (long)(float)o : throw new Exception(GetExceptionMsgBetween<float, long>()));
			AddConverter(ConversionType.Between<float, ushort>(), o => (float)o >= ushort.MinValue && (float)o <= ushort.MaxValue && (float)o == (ushort)(float)o ? (ushort)(float)o : throw new Exception(GetExceptionMsgBetween<float, ushort>()));
			AddConverter(ConversionType.Between<float, uint>(), o => (float)o >= uint.MinValue && (float)o <= uint.MaxValue && (float)o == (uint)(float)o ? (uint)(float)o : throw new Exception(GetExceptionMsgBetween<float, uint>()));
			AddConverter(ConversionType.Between<float, ulong>(), o => (float)o >= ulong.MinValue && (float)o <= ulong.MaxValue && (float)o == (ulong)(float)o ? (ulong)(float)o : throw new Exception(GetExceptionMsgBetween<float, ulong>()));

			AddConverter(ConversionType.Between<double, bool>(), o => ((double)o > 0));
			AddConverter(ConversionType.Between<double, byte>(), o => (double)o >= byte.MinValue && (double)o <= byte.MaxValue && (double)o == (byte)(double)o ? (byte)(double)o : throw new Exception(GetExceptionMsgBetween<double, byte>()));
			AddConverter(ConversionType.Between<double, sbyte>(), o => (double)o == (sbyte)(double)o ? (sbyte)(double)o : throw new Exception(GetExceptionMsgBetween<double, sbyte>()));
			AddConverter(ConversionType.Between<double, float>(), o => ((double)o <= float.MaxValue && (double)o == (float)(double)o) ? (float)(double)o : throw new Exception(GetExceptionMsgBetween<double, float>()));
			AddConverter(ConversionType.Between<double, decimal>(), o => (decimal)(double)o);
			AddConverter(ConversionType.Between<double, short>(), o => (double)o == (short)(double)o ? (short)(double)o : throw new Exception(GetExceptionMsgBetween<double, short>()));
			AddConverter(ConversionType.Between<double, int>(), o => (double)o == (int)(double)o ? (int)(double)o : throw new Exception(GetExceptionMsgBetween<double, int>()));
			AddConverter(ConversionType.Between<double, long>(), o => (double)o == (long)(double)o ? (long)(double)o : throw new Exception(GetExceptionMsgBetween<double, long>()));
			AddConverter(ConversionType.Between<double, ushort>(), o => (double)o >= ushort.MinValue && (double)o <= ushort.MaxValue && (double)o == (ushort)(double)o ? (ushort)(double)o : throw new Exception(GetExceptionMsgBetween<double, ushort>()));
			AddConverter(ConversionType.Between<double, uint>(), o => (double)o >= uint.MinValue && (double)o <= uint.MaxValue && (double)o == (uint)(double)o ? (uint)(double)o : throw new Exception(GetExceptionMsgBetween<double, uint>()));
			AddConverter(ConversionType.Between<double, ulong>(), o => (double)o >= ulong.MinValue && (double)o <= ulong.MaxValue && (double)o == (ulong)(double)o ? (ulong)(double)o : throw new Exception(GetExceptionMsgBetween<double, ulong>()));

			AddConverter(ConversionType.Between<decimal, bool>(), o => ((decimal)o > 0));
			AddConverter(ConversionType.Between<decimal, byte>(), o => (decimal)o >= byte.MinValue && (decimal)o <= byte.MaxValue && (decimal)o == (byte)(decimal)o ? (byte)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, byte>()));
			AddConverter(ConversionType.Between<decimal, sbyte>(), o => (decimal)o == (sbyte)(decimal)o ? (sbyte)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, sbyte>()));
			AddConverter(ConversionType.Between<decimal, float>(), o => ((decimal)o <= Convert.ToDecimal(float.MaxValue) && (decimal)o == Convert.ToDecimal((float)(decimal)o)) ? (float)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, float>()));
			AddConverter(ConversionType.Between<decimal, double>(), o => ((decimal)o <= Convert.ToDecimal(double.MaxValue) && (decimal)o == Convert.ToDecimal((double)(decimal)o)) ? (double)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, double>()));
			AddConverter(ConversionType.Between<decimal, short>(), o => (decimal)o == (short)(decimal)o ? (short)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, short>()));
			AddConverter(ConversionType.Between<decimal, int>(), o => (decimal)o == (int)(decimal)o ? (int)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, int>()));
			AddConverter(ConversionType.Between<decimal, long>(), o => (decimal)o == (long)(decimal)o ? (long)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, long>()));
			AddConverter(ConversionType.Between<decimal, ushort>(), o => (decimal)o >= ushort.MinValue && (decimal)o <= ushort.MaxValue && (decimal)o == (ushort)(decimal)o ? (ushort)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, ushort>()));
			AddConverter(ConversionType.Between<decimal, uint>(), o => (decimal)o >= uint.MinValue && (decimal)o <= uint.MaxValue && (decimal)o == (uint)(decimal)o ? (uint)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, uint>()));
			AddConverter(ConversionType.Between<decimal, ulong>(), o => (decimal)o >= ulong.MinValue && (decimal)o <= ulong.MaxValue && (decimal)o == (ulong)(decimal)o ? (ulong)(decimal)o : throw new Exception(GetExceptionMsgBetween<decimal, ulong>()));

			AddConverter(ConversionType.Between<short, bool>(), o => ((short)o > 0));
			AddConverter(ConversionType.Between<short, byte>(), o => ((short)o >= byte.MinValue && (short)o <= byte.MaxValue) ? (byte)(short)o : throw new Exception(GetExceptionMsgBetween<short, byte>()));
			AddConverter(ConversionType.Between<short, sbyte>(), o => ((short)o >= sbyte.MinValue && (short)o <= sbyte.MaxValue) ? (sbyte)(short)o : throw new Exception(GetExceptionMsgBetween<short, sbyte>()));
			AddConverter(ConversionType.Between<short, float>(), o => (float)(short)o);
			AddConverter(ConversionType.Between<short, double>(), o => (double)(short)o);
			AddConverter(ConversionType.Between<short, decimal>(), o => (decimal)(short)o);
			AddConverter(ConversionType.Between<short, int>(), o => (int)(short)o);
			AddConverter(ConversionType.Between<short, long>(), o => (long)(short)o);
			AddConverter(ConversionType.Between<short, ushort>(), o => (short)o >= ushort.MinValue ? (ushort)(short)o : throw new Exception(GetExceptionMsgBetween<short, ushort>()));
			AddConverter(ConversionType.Between<short, uint>(), o => (short)o >= uint.MinValue ? (uint)(short)o : throw new Exception(GetExceptionMsgBetween<short, uint>()));
			AddConverter(ConversionType.Between<short, ulong>(), o => (short)o >= (short)ulong.MinValue ? (ulong)(short)o : throw new Exception(GetExceptionMsgBetween<short, ulong>()));

			AddConverter(ConversionType.Between<int, bool>(), o => ((int)o > 0));
			AddConverter(ConversionType.Between<int, byte>(), o => ((int)o >= byte.MinValue && (int)o <= byte.MaxValue) ? (byte)(int)o : throw new Exception(GetExceptionMsgBetween<int, byte>()));
			AddConverter(ConversionType.Between<int, sbyte>(), o => ((int)o >= sbyte.MinValue && (int)o <= sbyte.MaxValue) ? (sbyte)(int)o : throw new Exception(GetExceptionMsgBetween<int, sbyte>()));
			AddConverter(ConversionType.Between<int, float>(), o => (float)(int)o);
			AddConverter(ConversionType.Between<int, double>(), o => (double)(int)o);
			AddConverter(ConversionType.Between<int, decimal>(), o => (decimal)(int)o);
			AddConverter(ConversionType.Between<int, short>(), o => ((int)o >= short.MinValue && (int)o <= short.MaxValue) ? (short)(int)o : throw new Exception(GetExceptionMsgBetween<int, short>()));
			AddConverter(ConversionType.Between<int, long>(), o => (long)(int)o);
			AddConverter(ConversionType.Between<int, ushort>(), o => (int)o >= ushort.MinValue && (int)o <= ushort.MaxValue ? (ushort)(int)o : throw new Exception(GetExceptionMsgBetween<int, ushort>()));
			AddConverter(ConversionType.Between<int, uint>(), o => (int)o >= uint.MinValue ? (uint)(int)o : throw new Exception(GetExceptionMsgBetween<int, uint>()));
			AddConverter(ConversionType.Between<int, ulong>(), o => (int)o >= (int)ulong.MinValue ? (ulong)(int)o : throw new Exception(GetExceptionMsgBetween<int, ulong>()));

			AddConverter(ConversionType.Between<long, bool>(), o => ((long)o > 0));
			AddConverter(ConversionType.Between<long, byte>(), o => ((long)o >= byte.MinValue && (long)o <= byte.MaxValue) ? (byte)(long)o : throw new Exception(GetExceptionMsgBetween<long, byte>()));
			AddConverter(ConversionType.Between<long, sbyte>(), o => ((long)o >= sbyte.MinValue && (long)o <= sbyte.MaxValue) ? (sbyte)(long)o : throw new Exception(GetExceptionMsgBetween<long, sbyte>()));
			AddConverter(ConversionType.Between<long, float>(), o => (long)o >= float.MinValue && (long)o <= float.MaxValue && (long)o == (float)(long)o ? (float)(long)o : throw new Exception(GetExceptionMsgBetween<long, float>()));
			AddConverter(ConversionType.Between<long, double>(), o => (double)(long)o);
			AddConverter(ConversionType.Between<long, decimal>(), o => (decimal)(long)o);
			AddConverter(ConversionType.Between<long, short>(), o => ((long)o >= short.MinValue && (long)o <= short.MaxValue) ? (short)(long)o : throw new Exception(GetExceptionMsgBetween<long, short>()));
			AddConverter(ConversionType.Between<long, int>(), o => ((long)o >= int.MinValue && (long)o <= int.MaxValue) ? (int)(long)o : throw new Exception(GetExceptionMsgBetween<long, int>()));
			AddConverter(ConversionType.Between<long, ushort>(), o => (long)o >= ushort.MinValue && (long)o <= ushort.MaxValue ? (ushort)(long)o : throw new Exception(GetExceptionMsgBetween<long, ushort>()));
			AddConverter(ConversionType.Between<long, uint>(), o => (long)o >= uint.MinValue && (long)o <= uint.MaxValue ? (uint)(long)o : throw new Exception(GetExceptionMsgBetween<long, uint>()));
			AddConverter(ConversionType.Between<long, ulong>(), o => (long)o >= (long)ulong.MinValue ? (ulong)(long)o : throw new Exception(GetExceptionMsgBetween<long, ulong>()));

			AddConverter(ConversionType.Between<ushort, bool>(), o => ((ushort)o > 0));
			AddConverter(ConversionType.Between<ushort, byte>(), o => (ushort)o <= byte.MaxValue ? (byte)(ushort)o : throw new Exception(GetExceptionMsgBetween<ushort, byte>()));
			AddConverter(ConversionType.Between<ushort, sbyte>(), o => (ushort)o <= sbyte.MaxValue ? (sbyte)(ushort)o : throw new Exception(GetExceptionMsgBetween<ushort, sbyte>()));
			AddConverter(ConversionType.Between<ushort, float>(), o => (float)(ushort)o);
			AddConverter(ConversionType.Between<ushort, double>(), o => (double)(ushort)o);
			AddConverter(ConversionType.Between<ushort, decimal>(), o => (decimal)(ushort)o);
			AddConverter(ConversionType.Between<ushort, short>(), o => (ushort)o <= short.MaxValue ? (short)(ushort)o : throw new Exception(GetExceptionMsgBetween<ushort, short>()));
			AddConverter(ConversionType.Between<ushort, int>(), o => (int)(ushort)o);
			AddConverter(ConversionType.Between<ushort, long>(), o => (long)(ushort)o);
			AddConverter(ConversionType.Between<ushort, uint>(), o => (uint)(ushort)o);
			AddConverter(ConversionType.Between<ushort, ulong>(), o => (ulong)(ushort)o);

			AddConverter(ConversionType.Between<uint, bool>(), o => ((uint)o > 0));
			AddConverter(ConversionType.Between<uint, byte>(), o => (uint)o <= byte.MaxValue ? (byte)(uint)o : throw new Exception(GetExceptionMsgBetween<uint, byte>()));
			AddConverter(ConversionType.Between<uint, sbyte>(), o => (uint)o <= sbyte.MaxValue ? (sbyte)(uint)o : throw new Exception(GetExceptionMsgBetween<uint, sbyte>()));
			AddConverter(ConversionType.Between<uint, float>(), o => (uint)o <= float.MaxValue ? (float)(uint)o : throw new Exception(GetExceptionMsgBetween<uint, float>()));
			AddConverter(ConversionType.Between<uint, double>(), o => (double)(uint)o);
			AddConverter(ConversionType.Between<uint, decimal>(), o => (decimal)(uint)o);
			AddConverter(ConversionType.Between<uint, short>(), o => (uint)o <= short.MaxValue ? (short)(uint)o : throw new Exception(GetExceptionMsgBetween<uint, short>()));
			AddConverter(ConversionType.Between<uint, int>(), o => (uint)o <= int.MaxValue ? (int)(uint)o : throw new Exception(GetExceptionMsgBetween<uint, int>()));
			AddConverter(ConversionType.Between<uint, long>(), o => (long)(uint)o);
			AddConverter(ConversionType.Between<uint, ushort>(), o => (uint)o <= ushort.MaxValue ? (ushort)(uint)o : throw new Exception(GetExceptionMsgBetween<uint, ushort>()));
			AddConverter(ConversionType.Between<uint, ulong>(), o => (ulong)(uint)o);

			AddConverter(ConversionType.Between<ulong, bool>(), o => ((ulong)o > 0));
			AddConverter(ConversionType.Between<ulong, byte>(), o => (ulong)o <= byte.MaxValue ? (byte)(ulong)o : throw new Exception(GetExceptionMsgBetween<ulong, byte>()));
			AddConverter(ConversionType.Between<ulong, sbyte>(), o => (ulong)o <= (ulong)sbyte.MaxValue ? (sbyte)(ulong)o : throw new Exception(GetExceptionMsgBetween<ulong, sbyte>()));
			AddConverter(ConversionType.Between<ulong, float>(), o => (ulong)o <= float.MaxValue ? (float)(ulong)o : throw new Exception(GetExceptionMsgBetween<ulong, float>()));
			AddConverter(ConversionType.Between<ulong, double>(), o => (ulong)o <= double.MaxValue ? (double)(ulong)o : throw new Exception(GetExceptionMsgBetween<ulong, double>()));
			AddConverter(ConversionType.Between<ulong, decimal>(), o => (decimal)(ulong)o);
			AddConverter(ConversionType.Between<ulong, short>(), o => (ulong)o <= (ulong)short.MaxValue ? (short)(ulong)o : throw new Exception(GetExceptionMsgBetween<ulong, short>()));
			AddConverter(ConversionType.Between<ulong, int>(), o => (ulong)o <= int.MaxValue ? (int)(ulong)o : throw new Exception(GetExceptionMsgBetween<ulong, int>()));
			AddConverter(ConversionType.Between<ulong, long>(), o => (ulong)o <= long.MaxValue ? (long)(ulong)o : throw new Exception(GetExceptionMsgBetween<ulong, long>()));
			AddConverter(ConversionType.Between<ulong, ushort>(), o => (ulong)o <= ushort.MaxValue ? (ushort)(ulong)o : throw new Exception(GetExceptionMsgBetween<ulong, ushort>()));
			AddConverter(ConversionType.Between<ulong, uint>(), o => (ulong)o <= uint.MaxValue ? (uint)(ulong)o : throw new Exception(GetExceptionMsgBetween<ulong, uint>()));

			AddConverter(ConversionType.Between<bool, string>(), o => Cast.This((bool)o).AsString());
			AddConverter(ConversionType.Between<byte, string>(), o => Cast.This((byte)o).AsString());
			AddConverter(ConversionType.Between<sbyte, string>(), o => Cast.This((sbyte)o).AsString());
			AddConverter(ConversionType.Between<float, string>(), o => Cast.This((float)o).AsString());
			AddConverter(ConversionType.Between<double, string>(), o => Cast.This((double)o).AsString());
			AddConverter(ConversionType.Between<decimal, string>(), o => Cast.This((decimal)o).AsString());
			AddConverter(ConversionType.Between<short, string>(), o => Cast.This((short)o).AsString());
			AddConverter(ConversionType.Between<int, string>(), o => Cast.This((int)o).AsString());
			AddConverter(ConversionType.Between<long, string>(), o => Cast.This((long)o).AsString());
			AddConverter(ConversionType.Between<ushort, string>(), o => Cast.This((ushort)o).AsString());
			AddConverter(ConversionType.Between<uint, string>(), o => Cast.This((uint)o).AsString());
			AddConverter(ConversionType.Between<ulong, string>(), o => Cast.This((ulong)o).AsString());
			AddConverter(ConversionType.Between<char, string>(), o => Cast.This((char)o).AsString());

			AddConverter(ConversionType.Between<string, bool>(), o => bool.TryParse((string)o, out bool result) ? result : throw new Exception(GetExceptionMsgBetween<string, bool>()));
			AddConverter(ConversionType.Between<string, byte>(), o => byte.TryParse((string)o, out byte result) ? result : throw new Exception(GetExceptionMsgBetween<string, byte>()));
			AddConverter(ConversionType.Between<string, sbyte>(), o => sbyte.TryParse((string)o, out sbyte result) ? result : throw new Exception(GetExceptionMsgBetween<string, sbyte>()));
			AddConverter(ConversionType.Between<string, float>(), o => float.TryParse((string)o, out float result) ? result : throw new Exception(GetExceptionMsgBetween<string, float>()));
			AddConverter(ConversionType.Between<string, double>(), o => double.TryParse((string)o, out double result) ? result : throw new Exception(GetExceptionMsgBetween<string, double>()));
			AddConverter(ConversionType.Between<string, decimal>(), o => decimal.TryParse((string)o, out decimal result) ? result : throw new Exception(GetExceptionMsgBetween<string, decimal>()));
			AddConverter(ConversionType.Between<string, short>(), o => short.TryParse((string)o, out short result) ? result : throw new Exception(GetExceptionMsgBetween<string, short>()));
			AddConverter(ConversionType.Between<string, int>(), o => int.TryParse((string)o, out int result) ? result : throw new Exception(GetExceptionMsgBetween<string, int>()));
			AddConverter(ConversionType.Between<string, long>(), o => long.TryParse((string)o, out long result) ? result : throw new Exception(GetExceptionMsgBetween<string, long>()));
			AddConverter(ConversionType.Between<string, ushort>(), o => ushort.TryParse((string)o, out ushort result) ? result : throw new Exception(GetExceptionMsgBetween<string, ushort>()));
			AddConverter(ConversionType.Between<string, uint>(), o => uint.TryParse((string)o, out uint result) ? result : throw new Exception(GetExceptionMsgBetween<string, uint>()));
			AddConverter(ConversionType.Between<string, ulong>(), o => ulong.TryParse((string)o, out ulong result) ? result : throw new Exception(GetExceptionMsgBetween<string, ulong>()));
			AddConverter(ConversionType.Between<string, char>(), o => char.TryParse((string)o, out char result) ? result : throw new Exception(GetExceptionMsgBetween<string, char>()));

#if UNITY_2019_3_OR_NEWER

			AddConverter(ConversionType.Between<UnityEngine.Color, UnityEngine.Color32>(), o => (UnityEngine.Color32)(UnityEngine.Color)o);
			AddConverter(ConversionType.Between<UnityEngine.Color32, UnityEngine.Color>(), o => (UnityEngine.Color)(UnityEngine.Color32)o);
			AddConverter(ConversionType.Between<UnityEngine.Vector2Int, UnityEngine.Vector2>(), o => (UnityEngine.Vector2)(UnityEngine.Vector2Int)o);
			AddConverter(ConversionType.Between<UnityEngine.Vector3Int, UnityEngine.Vector3>(), o => (UnityEngine.Vector3)(UnityEngine.Vector3Int)o);
			AddConverter(ConversionType.Between<UnityEngine.Vector2, UnityEngine.Vector2Int>(), o => (UnityEngine.Vector2)o == UnityEngine.Vector2Int.RoundToInt((UnityEngine.Vector2)o) ? UnityEngine.Vector2Int.RoundToInt((UnityEngine.Vector2)o) : throw new Exception(GetExceptionMsgBetween<UnityEngine.Vector2, UnityEngine.Vector2Int>()));
			AddConverter(ConversionType.Between<UnityEngine.Vector3, UnityEngine.Vector3Int>(), o => (UnityEngine.Vector3)o == UnityEngine.Vector3Int.RoundToInt((UnityEngine.Vector3)o) ? UnityEngine.Vector3Int.RoundToInt((UnityEngine.Vector3)o) : throw new Exception(GetExceptionMsgBetween<UnityEngine.Vector3, UnityEngine.Vector3Int>()));

			AddConverter(ConversionType.Between<UnityEngine.Color, string>(), o => Cast.This((UnityEngine.Color)o).AsString());
			AddConverter(ConversionType.Between<UnityEngine.Color32, string>(), o => Cast.This((UnityEngine.Color32)o).AsString());
			AddConverter(ConversionType.Between<UnityEngine.Vector2, string>(), o => Cast.This((UnityEngine.Vector2)o).AsString());
			AddConverter(ConversionType.Between<UnityEngine.Vector2Int, string>(), o => Cast.This((UnityEngine.Vector2Int)o).AsString());
			AddConverter(ConversionType.Between<UnityEngine.Vector3, string>(), o => Cast.This((UnityEngine.Vector3)o).AsString());
			AddConverter(ConversionType.Between<UnityEngine.Vector3Int, string>(), o => Cast.This((UnityEngine.Vector3Int)o).AsString());
			AddConverter(ConversionType.Between<UnityEngine.Vector4, string>(), o => Cast.This((UnityEngine.Vector4)o).AsString());

			AddConverter(ConversionType.Between<string, UnityEngine.Color>(), o =>
			{
				var value = (string)o;
				if (value.Length <= 8 && !value.StartsWith("#")) value = "#" + value;
				return (UnityEngine.ColorUtility.TryParseHtmlString(value, out UnityEngine.Color result)) ? result : throw new Exception(GetExceptionMsgBetween<string, UnityEngine.Color>());
			});
			AddConverter(ConversionType.Between<string, UnityEngine.Color32>(), o =>
			{
				var value = (string)o;
				if (value.Length <= 8 && !value.StartsWith("#")) value = "#" + value;
				return (UnityEngine.ColorUtility.TryParseHtmlString(value, out UnityEngine.Color result)) ? (UnityEngine.Color32)result : throw new Exception(GetExceptionMsgBetween<string, UnityEngine.Color32>());
			});
			AddConverter(ConversionType.Between<string, UnityEngine.Vector2>(), o =>
			{
				var splittedValue = ((string)o).Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);
				if (splittedValue.Length == 2 && float.TryParse(splittedValue[0], out float x) && float.TryParse(splittedValue[1], out float y)) return new UnityEngine.Vector2(x, y);
				else throw new Exception(GetExceptionMsgBetween<string, UnityEngine.Vector2>());
			});
			AddConverter(ConversionType.Between<string, UnityEngine.Vector2Int>(), o =>
			{
				var splittedValue = ((string)o).Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);
				if (splittedValue.Length == 2 && int.TryParse(splittedValue[0], out int x) && int.TryParse(splittedValue[1], out int y)) return new UnityEngine.Vector2Int(x, y);
				else throw new Exception(GetExceptionMsgBetween<string, UnityEngine.Vector2Int>());
			});
			AddConverter(ConversionType.Between<string, UnityEngine.Vector3>(), o =>
			{
				var splittedValue = ((string)o).Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);
				if (splittedValue.Length == 3 && float.TryParse(splittedValue[0], out float x) && float.TryParse(splittedValue[1], out float y) && float.TryParse(splittedValue[2], out float z)) return new UnityEngine.Vector3(x, y, z);
				else throw new Exception(GetExceptionMsgBetween<string, UnityEngine.Vector3>());
			});
			AddConverter(ConversionType.Between<string, UnityEngine.Vector3Int>(), o =>
			{
				var splittedValue = ((string)o).Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);
				if (splittedValue.Length == 3 && int.TryParse(splittedValue[0], out int x) && int.TryParse(splittedValue[1], out int y) && int.TryParse(splittedValue[2], out int z)) return new UnityEngine.Vector3Int(x, y, z);
				else throw new Exception(GetExceptionMsgBetween<string, UnityEngine.Vector3Int>());
			});
			AddConverter(ConversionType.Between<string, UnityEngine.Vector4>(), o =>
			{
				var splittedValue = ((string)o).Split(vectorStringDelimiters, StringSplitOptions.RemoveEmptyEntries);
				if (splittedValue.Length == 4 && float.TryParse(splittedValue[0], out float x) && float.TryParse(splittedValue[1], out float y) && float.TryParse(splittedValue[2], out float z) && float.TryParse(splittedValue[3], out float w)) return new UnityEngine.Vector4(x, y, z, w);
				else throw new Exception(GetExceptionMsgBetween<string, UnityEngine.Vector4>());
			});

#endif
		}
	}
}
