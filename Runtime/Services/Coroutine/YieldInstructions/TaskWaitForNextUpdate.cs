// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using Ju.Time;

namespace Ju.Services
{
	public class TaskWaitForNextUpdate : TaskWaitForTicks<TimeUpdateEvent>
	{
		public TaskWaitForNextUpdate() : base(1)
		{
		}
	}
}
