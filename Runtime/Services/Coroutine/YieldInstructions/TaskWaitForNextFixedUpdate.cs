// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using Ju.Time;

namespace Ju.Services
{
	public class TaskWaitForNextFixedUpdate : TaskWaitForTicks<TimeFixedUpdateEvent>
	{
		public TaskWaitForNextFixedUpdate() : base(1)
		{
		}
	}
}
