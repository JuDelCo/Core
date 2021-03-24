// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.Services;

namespace Ju.Time
{
	public class FrameClock<T> : IFrameClock where T : ITimeEvent
	{
		private readonly DisposableLinkHandler linkHandler;
		private int elapsed;
		private readonly Func<bool> updateCondition;

		public FrameClock()
		{
			linkHandler = new DisposableLinkHandler(false);
			ServiceContainer.Get<IEventBusService>().Subscribe<T>(linkHandler, Tick);
		}

		public FrameClock(int elapsedFrames) : this()
		{
			elapsed = elapsedFrames;
		}

		public FrameClock(Func<bool> updateCondition) : this()
		{
			this.updateCondition = updateCondition;
		}

		public FrameClock(int elapsedFrames, Func<bool> updateCondition) : this(elapsedFrames)
		{
			this.updateCondition = updateCondition;
		}

		public void Dispose()
		{
			linkHandler.Dispose();
			GC.SuppressFinalize(this);
		}

		public int Reset(int elapsedFrames = 0)
		{
			var elapsedTime = elapsed;

			elapsed = Math.Max(elapsedFrames, 0);

			return elapsedTime;
		}

		public int GetElapsedFrames()
		{
			return elapsed;
		}

		private void Tick()
		{
			if (!(updateCondition is null))
			{
				if (!updateCondition())
				{
					return;
				}
			}

			elapsed += 1;
		}

		public static bool operator <(FrameClock<T> clock, int frames)
		{
			return clock.GetElapsedFrames() < frames;
		}

		public static bool operator <=(FrameClock<T> clock, int frames)
		{
			return clock.GetElapsedFrames() <= frames;
		}

		public static bool operator >(FrameClock<T> clock, int frames)
		{
			return clock.GetElapsedFrames() > frames;
		}

		public static bool operator >=(FrameClock<T> clock, int frames)
		{
			return clock.GetElapsedFrames() >= frames;
		}

		public static bool operator <(FrameClock<T> a, FrameClock<T> b)
		{
			return a.GetElapsedFrames() < b.GetElapsedFrames();
		}

		public static bool operator <=(FrameClock<T> a, FrameClock<T> b)
		{
			return a.GetElapsedFrames() <= b.GetElapsedFrames();
		}

		public static bool operator >(FrameClock<T> a, FrameClock<T> b)
		{
			return a.GetElapsedFrames() > b.GetElapsedFrames();
		}

		public static bool operator >=(FrameClock<T> a, FrameClock<T> b)
		{
			return a.GetElapsedFrames() >= b.GetElapsedFrames();
		}
	}
}
