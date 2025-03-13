// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using Ju.Time;

namespace Ju.Services
{
	public class TaskWaitForTicks<T> : YieldTaskBase where T : ITimeEvent
	{
		public override bool KeepWaiting { get { return timer.GetFramesLeft() > 0; } }

		private readonly IFrameTimer timer;

		public TaskWaitForTicks(int ticks)
		{
			timer = new FrameTimer<T>(ticks);
		}
	}
}
