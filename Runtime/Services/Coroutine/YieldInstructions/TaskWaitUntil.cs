// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Services
{
	public class TaskWaitUntil : YieldTaskBase
	{
		public override bool KeepWaiting { get { return !condition(); } }

		private readonly Func<bool> condition;

		public TaskWaitUntil(Func<bool> condition)
		{
			this.condition = condition;
		}
	}
}
