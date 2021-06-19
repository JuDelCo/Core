// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Handlers;
using Ju.Services.Internal;

namespace Ju.Time
{
	public partial class FrameTimer<T> : IFrameTimer where T : ITimeEvent
	{
		private readonly DisposableLinkHandler linkHandler;
		private int elapsed;
		private readonly int duration;
		private readonly Func<bool> updateCondition;
		private readonly Action onCompleted;

		public FrameTimer(int frames)
		{
			linkHandler = new DisposableLinkHandler(false);

			ServiceCache.EventBus.Subscribe<T>(linkHandler, () =>
			{
				if (updateCondition != null)
				{
					if (!updateCondition())
					{
						return;
					}
				}

				var completed = elapsed > duration;

				elapsed += 1;

				if (!completed && elapsed >= duration)
				{
					if (onCompleted != null)
					{
						onCompleted();
					}
				}
			});

			this.duration = frames;
		}

		public FrameTimer(int frames, Action onCompleted) : this(frames)
		{
			this.onCompleted = onCompleted;
		}

		public FrameTimer(int frames, Action onCompleted, Func<bool> updateCondition) : this(frames, onCompleted)
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
			elapsed = 0;
		}

		public void Stop()
		{
			elapsed = duration + 1;
		}

		public int GetDuration()
		{
			return duration;
		}

		public int GetElapsedFrames()
		{
			return elapsed;
		}

		public int GetFramesLeft()
		{
			return duration - elapsed;
		}

		public static bool operator <(FrameTimer<T> timer, int frames)
		{
			return timer.GetFramesLeft() < frames;
		}

		public static bool operator <=(FrameTimer<T> timer, int frames)
		{
			return timer.GetFramesLeft() <= frames;
		}

		public static bool operator >(FrameTimer<T> timer, int frames)
		{
			return timer.GetFramesLeft() > frames;
		}

		public static bool operator >=(FrameTimer<T> timer, int frames)
		{
			return timer.GetFramesLeft() >= frames;
		}

		public static bool operator <(FrameTimer<T> a, FrameTimer<T> b)
		{
			return a.GetFramesLeft() < b.GetFramesLeft();
		}

		public static bool operator <=(FrameTimer<T> a, FrameTimer<T> b)
		{
			return a.GetFramesLeft() <= b.GetFramesLeft();
		}

		public static bool operator >(FrameTimer<T> a, FrameTimer<T> b)
		{
			return a.GetFramesLeft() > b.GetFramesLeft();
		}

		public static bool operator >=(FrameTimer<T> a, FrameTimer<T> b)
		{
			return a.GetFramesLeft() >= b.GetFramesLeft();
		}
	}
}
