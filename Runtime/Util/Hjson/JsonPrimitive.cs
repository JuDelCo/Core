// SPDX-License-Identifier: MIT
// Copyright (c) 2021-2025 Juan Delgado (@JuDelCo)

using System;
using System.Globalization;

namespace Ju.Hjson
{
	internal class JsonPrimitive : JsonValue
	{
		internal enum NumericKind
		{
			Int,
			Long,
			Float,
			Double,
			Decimal
		}

		internal readonly struct NumericValue
		{
			private readonly NumericKind kind;
			private readonly int intValue;
			private readonly long longValue;
			private readonly float floatValue;
			private readonly double doubleValue;
			private readonly decimal decimalValue;

			public NumericValue(int value)
			{
				kind = NumericKind.Int;
				intValue = value;
				longValue = default(long);
				floatValue = default(float);
				doubleValue = default(double);
				decimalValue = default(decimal);
			}

			public NumericValue(long value)
			{
				kind = NumericKind.Long;
				intValue = default(int);
				longValue = value;
				floatValue = default(float);
				doubleValue = default(double);
				decimalValue = default(decimal);
			}

			public NumericValue(float value)
			{
				kind = NumericKind.Float;
				intValue = default(int);
				longValue = default(long);
				floatValue = value;
				doubleValue = default(double);
				decimalValue = default(decimal);
			}

			public NumericValue(double value)
			{
				kind = NumericKind.Double;
				intValue = default(int);
				longValue = default(long);
				floatValue = default(float);
				doubleValue = value;
				decimalValue = default(decimal);
			}

			public NumericValue(decimal value)
			{
				kind = NumericKind.Decimal;
				intValue = default(int);
				longValue = default(long);
				floatValue = default(float);
				doubleValue = default(double);
				decimalValue = value;
			}

			public NumericKind Kind => kind;

			public int AsInt()
			{
				switch (kind)
				{
					case NumericKind.Int:
						return intValue;
					case NumericKind.Long:
						return checked((int) longValue);
					case NumericKind.Float:
						return checked((int) floatValue);
					case NumericKind.Double:
						return checked((int) doubleValue);
					case NumericKind.Decimal:
						return checked((int) decimalValue);
					default:
						throw new InvalidOperationException();
				}
			}

			public long AsLong()
			{
				switch (kind)
				{
					case NumericKind.Int:
						return intValue;
					case NumericKind.Long:
						return longValue;
					case NumericKind.Float:
						return checked((long) floatValue);
					case NumericKind.Double:
						return checked((long) doubleValue);
					case NumericKind.Decimal:
						return checked((long) decimalValue);
					default:
						throw new InvalidOperationException();
				}
			}

			public float AsFloat()
			{
				switch (kind)
				{
					case NumericKind.Int:
						return intValue;
					case NumericKind.Long:
						return longValue;
					case NumericKind.Float:
						return floatValue;
					case NumericKind.Double:
						return (float) doubleValue;
					case NumericKind.Decimal:
						return (float) decimalValue;
					default:
						throw new InvalidOperationException();
				}
			}

			public double AsDouble()
			{
				switch (kind)
				{
					case NumericKind.Int:
						return intValue;
					case NumericKind.Long:
						return longValue;
					case NumericKind.Float:
						return floatValue;
					case NumericKind.Double:
						return doubleValue;
					case NumericKind.Decimal:
						return (double) decimalValue;
					default:
						throw new InvalidOperationException();
				}
			}

			public decimal AsDecimal()
			{
				switch (kind)
				{
					case NumericKind.Int:
						return intValue;
					case NumericKind.Long:
						return longValue;
					case NumericKind.Float:
						return (decimal) floatValue;
					case NumericKind.Double:
						return (decimal) doubleValue;
					case NumericKind.Decimal:
						return decimalValue;
					default:
						throw new InvalidOperationException();
				}
			}

			public object AsObject()
			{
				switch (kind)
				{
					case NumericKind.Int:
						return intValue;
					case NumericKind.Long:
						return longValue;
					case NumericKind.Float:
						return floatValue;
					case NumericKind.Double:
						return doubleValue;
					case NumericKind.Decimal:
						return decimalValue;
					default:
						throw new InvalidOperationException();
				}
			}

			// For byte, short, etc. we rely on callers using checked casts on top of AsInt or AsLong

			public string ToRawString()
			{
#if __MonoCS__ // mono bug ca 2014
			if (kind == NumericKind.Decimal)
			{
				var res=((IFormattable)decimalValue).ToString("G", NumberFormatInfo.InvariantInfo);
				while (res.EndsWith("0")) res=res.Substring(0, res.Length-1);
				if (res.EndsWith(".") || res.EndsWith("e", StringComparison.OrdinalIgnoreCase)) res=res.Substring(0, res.Length-1);
				return res.ToLowerInvariant();
			}
#endif

				switch (kind)
				{
					case NumericKind.Int:
						return intValue.ToString(NumberFormatInfo.InvariantInfo);
					case NumericKind.Long:
						return longValue.ToString(NumberFormatInfo.InvariantInfo);
					case NumericKind.Float:
						return floatValue.ToString("G", NumberFormatInfo.InvariantInfo).ToLowerInvariant();
					case NumericKind.Double:
						return doubleValue.ToString("G", NumberFormatInfo.InvariantInfo).ToLowerInvariant();
					case NumericKind.Decimal:
						return decimalValue.ToString("G", NumberFormatInfo.InvariantInfo).ToLowerInvariant();
					default:
						throw new InvalidOperationException();
				}
			}
		}

		private readonly JsonType jsonType;
		private readonly string stringValue;
		private readonly bool boolValue;
		private readonly NumericValue numericValue;
		private readonly object objectValue;

		JsonPrimitive()
		{
			jsonType = JsonType.String;
		}

		public JsonPrimitive(string value)
		{
			jsonType = JsonType.String;
			stringValue = value;
		}

		public JsonPrimitive(char value)
		{
			jsonType = JsonType.String;
			stringValue = value.ToString();
		}

		public JsonPrimitive(bool value)
		{
			jsonType = JsonType.Boolean;
			boolValue = value;
		}

		public JsonPrimitive(int value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue(value);
		}

		public JsonPrimitive(long value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue(value);
		}

		public JsonPrimitive(float value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue(value);
		}

		public JsonPrimitive(double value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue(value);
		}

		public JsonPrimitive(decimal value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue(value);
		}

		public JsonPrimitive(byte value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue((int) value);
		}

		public JsonPrimitive(sbyte value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue((int) value);
		}

		public JsonPrimitive(short value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue((int) value);
		}

		public JsonPrimitive(ushort value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue((int) value);
		}

		public JsonPrimitive(uint value)
		{
			jsonType = JsonType.Number;
			numericValue = new NumericValue((long) value);
		}

		public JsonPrimitive(ulong value)
		{
			jsonType = JsonType.Number;

			if (value > long.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "ulong value too large to represent as JSON number");
			}

			numericValue = new NumericValue((long) value);
		}

		public JsonPrimitive(object value)
		{
			jsonType = JsonType.Unknown;
			objectValue = value;
		}

		public override JsonType JsonType => jsonType;

		public NumericKind JsonNumericKind => numericValue.Kind;

		public bool AsBool()
		{
			if (jsonType != JsonType.Boolean)
			{
				throw new InvalidOperationException("Value is not a Boolean.");
			}

			return boolValue;
		}

		public string AsString()
		{
			if (jsonType != JsonType.String)
			{
				throw new InvalidOperationException("Value is not a String.");
			}

			return stringValue;
		}

		public byte AsByte()
		{
			if (jsonType != JsonType.Number)
			{
				throw new InvalidOperationException("Value is not a Number.");
			}

			checked // overflow check
			{
				var val = numericValue.AsInt();

				if (val < byte.MinValue || val > byte.MaxValue)
				{
					throw new OverflowException("Value is out of range for byte.");
				}

				return (byte) val;
			}
		}

		public short AsShort()
		{
			if (jsonType != JsonType.Number)
			{
				throw new InvalidOperationException("Value is not a Number.");
			}

			checked // overflow check
			{
				var val = numericValue.AsInt();

				if (val < short.MinValue || val > short.MaxValue)
				{
					throw new OverflowException("Value is out of range for short.");
				}

				return (short) val;
			}
		}

		public int AsInt()
		{
			if (jsonType != JsonType.Number)
			{
				throw new InvalidOperationException("Value is not a Number.");
			}

			return numericValue.AsInt();
		}

		public long AsLong()
		{
			if (jsonType != JsonType.Number)
			{
				throw new InvalidOperationException("Value is not a Number.");
			}

			return numericValue.AsLong();
		}

		public float AsFloat()
		{
			if (jsonType != JsonType.Number)
			{
				throw new InvalidOperationException("Value is not a Number.");
			}

			return numericValue.AsFloat();
		}

		public double AsDouble()
		{
			if (jsonType != JsonType.Number)
			{
				throw new InvalidOperationException("Value is not a Number.");
			}

			return numericValue.AsDouble();
		}

		public decimal AsDecimal()
		{
			if (jsonType != JsonType.Number)
			{
				throw new InvalidOperationException("Value is not a Number.");
			}

			return numericValue.AsDecimal();
		}

		public new object AsObject()
		{
			switch (jsonType)
			{
				case JsonType.String:
					return stringValue;
				case JsonType.Boolean:
					return boolValue;
				case JsonType.Number:
					return numericValue.AsObject();
				case JsonType.Unknown:
					return objectValue;
				default:
					throw new InvalidOperationException("Unknown type");
			}
		}

		public string GetRawString()
		{
			switch (jsonType)
			{
				case JsonType.String:
					return (stringValue ?? "");
				case JsonType.Number:
					return numericValue.ToRawString();
				case JsonType.Boolean:
					return (boolValue ? "true" : "false");
				default:
					throw new InvalidOperationException("Invalid type for GetRawString");
			}
		}
	}
}
