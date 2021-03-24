// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public class Timer<T> : ITimer where T : ITimeDeltaEvent
	{
		private readonly DisposableLinkHandler linkHandler;
		private Span elapsed;
		private readonly Span duration;
		private readonly Func<bool> updateCondition;
		private readonly Action onCompleted;

		public Timer(float seconds)
		{
			linkHandler = new DisposableLinkHandler(false);
			ServiceContainer.Get<IEventBusService>().Subscribe<T>(linkHandler, e => Update(e.DeltaTime));

			this.duration = Span.Seconds(seconds);
		}

		public Timer(float seconds, Action onCompleted) : this(seconds)
		{
			this.onCompleted = onCompleted;
		}

		public Timer(float seconds, Action onCompleted, Func<bool> updateCondition) : this(seconds, onCompleted)
		{
			this.updateCondition = updateCondition;
		}

		public void Dispose()
		{
			linkHandler.Dispose();
			GC.SuppressFinalize(this);
		}

		public void Reset()
		{
			elapsed = Span.zero;
		}

		public void Stop()
		{
			elapsed = duration;
		}

		public Span GetDuration()
		{
			return duration;
		}

		public Span GetElapsedTime()
		{
			return elapsed;
		}

		public Span GetTimeLeft()
		{
			return duration - elapsed;
		}

		private void Update(float deltaTime)
		{
			if (!(updateCondition is null))
			{
				if (!updateCondition())
				{
					return;
				}
			}

			var completed = elapsed >= duration;

			elapsed += Span.Seconds(deltaTime);

			if (!completed && elapsed >= duration)
			{
				if (!(onCompleted is null))
				{
					onCompleted();
				}
			}
		}

		public static bool operator <(Timer<T> timer, float seconds)
		{
			return timer.GetTimeLeft().seconds < seconds;
		}

		public static bool operator <=(Timer<T> timer, float seconds)
		{
			return timer.GetTimeLeft().seconds <= seconds;
		}

		public static bool operator >(Timer<T> timer, float seconds)
		{
			return timer.GetTimeLeft().seconds > seconds;
		}

		public static bool operator >=(Timer<T> timer, float seconds)
		{
			return timer.GetTimeLeft().seconds >= seconds;
		}

		public static bool operator <(Timer<T> a, Timer<T> b)
		{
			return a.GetTimeLeft() < b.GetTimeLeft();
		}

		public static bool operator <=(Timer<T> a, Timer<T> b)
		{
			return a.GetTimeLeft() <= b.GetTimeLeft();
		}

		public static bool operator >(Timer<T> a, Timer<T> b)
		{
			return a.GetTimeLeft() > b.GetTimeLeft();
		}

		public static bool operator >=(Timer<T> a, Timer<T> b)
		{
			return a.GetTimeLeft() >= b.GetTimeLeft();
		}
	}
}
