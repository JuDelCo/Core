using DateTime = System.DateTime;
using TimeSpan = System.TimeSpan;
using Microseconds = System.Int64;

namespace Ju.TimeUnit
{
	public struct Time
	{
		private Microseconds value;

		public static Time zero = new Time(0);

		public static Time Now()
		{
			return Time.Microseconds(DateTime.UtcNow.Ticks / (TimeSpan.TicksPerMillisecond / 1000));
		}

		public static Time Seconds(float amount)
		{
			return new Time((long)((double)amount * 1000000f));
		}

		public static Time Milliseconds(int amount)
		{
			return new Time((long)amount * 1000);
		}

		public static Time Microseconds(long amount)
		{
			return new Time(amount);
		}

		private Time(long microseconds)
		{
			value = microseconds;
		}

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

		public static Time operator +(Time a, Time b)
		{
			return Time.Microseconds(a.microseconds + b.microseconds);
		}

		public static Time operator -(Time a, Time b)
		{
			return Time.Microseconds(a.microseconds - b.microseconds);
		}

		public static Time operator *(Time t, float v)
		{
			return Time.Seconds(t.seconds * v);
		}

		public static Time operator *(float v, Time t)
		{
			return t * v;
		}

		public static Time operator *(Time t, long v)
		{
			return Time.Microseconds(t.microseconds * v);
		}

		public static Time operator *(long v, Time t)
		{
			return t * v;
		}

		public static Time operator /(Time t, float v)
		{
			return Time.Seconds(t.seconds / v);
		}

		public static Time operator /(Time t, long v)
		{
			return Time.Microseconds(t.microseconds / v);
		}

		public static Time operator %(Time a, Time b)
		{
			return Time.Microseconds(a.microseconds % b.microseconds);
		}

		public static bool operator <(Time a, Time b)
		{
			return a.value < b.value;
		}

		public static bool operator >(Time a, Time b)
		{
			return a.value > b.value;
		}

		public static bool operator <=(Time a, Time b)
		{
			return a.value <= b.value;
		}

		public static bool operator >=(Time a, Time b)
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
			return (obj is Time && (this == (Time)obj));
		}

		public static bool operator ==(Time a, Time b)
		{
			return (a.value == b.value);
		}

		public static bool operator !=(Time a, Time b)
		{
			return !(a == b);
		}
	}
}
