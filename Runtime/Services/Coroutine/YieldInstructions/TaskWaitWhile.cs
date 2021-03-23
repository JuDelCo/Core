// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Services
{
	public class TaskWaitWhile : YieldTaskBase
	{
		public override bool KeepWaiting { get { return condition(); } }

		private readonly Func<bool> condition;

		public TaskWaitWhile(Func<bool> condition)
		{
			this.condition = condition;
		}
	}
}
