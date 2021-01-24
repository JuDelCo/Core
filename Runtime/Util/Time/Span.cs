// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using Microseconds = System.Int64;

namespace Ju.Time
{
	public struct Span
	{
		private readonly Microseconds value;

		public static Span zero = new Span(0);

		public static Span Now()
		{
			return Span.Microseconds(System.DateTime.UtcNow.Ticks / (System.TimeSpan.TicksPerMillisecond / 1000));
		}

		public static Span Seconds(float amount)
		{
			return new Span((long)((double)amount * 1000000f));
		}

		public static Span Milliseconds(int amount)
		{
			return new Span((long)amount * 1000);
		}

		public static Span Microseconds(long amount)
		{
			return new Span(amount);
		}

		private Span(long microseconds)
		{
			value = microseconds;
		}

#pragma warning disable IDE1006

		public float seconds
		{
			get { return value / 1000000f; }
		}

		public int milliseconds
		{
			get { return (int)(value / 1000); }
		}

		public long microseconds
		{
			get { return value; }
		}

#pragma warning restore IDE1006

		public static Span operator +(Span a, Span b)
		{
			return Span.Microseconds(a.microseconds + b.microseconds);
		}

		public static Span operator -(Span a, Span b)
		{
			return Span.Microseconds(a.microseconds - b.microseconds);
		}

		public static Span operator *(Span t, float v)
		{
			return Span.Seconds(t.seconds * v);
		}

		public static Span operator *(float v, Span t)
		{
			return t * v;
		}

		public static Span operator *(Span t, long v)
		{
			return Span.Microseconds(t.microseconds * v);
		}

		public static Span operator *(long v, Span t)
		{
			return t * v;
		}

		public static Span operator /(Span t, float v)
		{
			return Span.Seconds(t.seconds / v);
		}

		public static Span operator /(Span t, long v)
		{
			return Span.Microseconds(t.microseconds / v);
		}

		public static Span operator %(Span a, Span b)
		{
			return Span.Microseconds(a.microseconds % b.microseconds);
		}

		public static bool operator <(Span a, Span b)
		{
			return a.value < b.value;
		}

		public static bool operator >(Span a, Span b)
		{
			return a.value > b.value;
		}

		public static bool operator <=(Span a, Span b)
		{
			return a.value <= b.value;
		}

		public static bool operator >=(Span a, Span b)
		{
			return a.value >= b.value;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + value.GetHashCode();
				return hash;
			}
		}

		public override bool Equals(object obj)
		{
			return (obj is Span span && (this == span));
		}

		public static bool operator ==(Span a, Span b)
		{
			return (a.value == b.value);
		}

		public static bool operator !=(Span a, Span b)
		{
			return !(a == b);
		}
	}
}
