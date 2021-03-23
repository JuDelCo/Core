// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using Ju.Time;

namespace Ju.Services
{
	public class TaskWaitForSeconds<T> : YieldTaskBase where T : ITimeDeltaEvent
	{
		public override bool KeepWaiting { get { return timer.GetTimeLeft().seconds > 0f; } }

		private readonly ITimer timer;

		public TaskWaitForSeconds(float seconds)
		{
			timer = new Timer<T>(seconds);
		}
	}
}
