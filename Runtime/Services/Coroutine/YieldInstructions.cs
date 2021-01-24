// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using Ju.Time;

namespace Ju.Services
{
	public abstract class YieldInstruction : IEnumerator
	{
		public virtual bool KeepWaiting { get { return false; } }

		public virtual object Current { get { return null; } }

		public virtual bool MoveNext()
		{
			return KeepWaiting;
		}

		public virtual void Reset()
		{
		}
	}

	public class Coroutine : YieldInstruction
	{
		public override bool KeepWaiting { get { return !finished; } }

		public override object Current { get { return routine.Current; } }

		private readonly IEnumerator routine;
		private bool finished;

		public Coroutine(IEnumerator routine)
		{
			this.routine = routine;
		}

		public override bool MoveNext()
		{
			finished = !routine.MoveNext();
			return !finished;
		}

		public override void Reset()
		{
			finished = false;
			routine.Reset();
		}
	}

	public class TaskWaitUntil : YieldInstruction
	{
		public override bool KeepWaiting { get { return !condition(); } }

		private readonly Func<bool> condition;

		public TaskWaitUntil(Func<bool> condition)
		{
			this.condition = condition;
		}
	}

	public class TaskWaitWhile : YieldInstruction
	{
		public override bool KeepWaiting { get { return condition(); } }

		private readonly Func<bool> condition;

		public TaskWaitWhile(Func<bool> condition)
		{
			this.condition = condition;
		}
	}

	public class TaskWaitForSeconds<T> : YieldInstruction where T : ILoopTimeEvent
	{
		public override bool KeepWaiting { get { return timer.GetTimeLeft().seconds > 0f; } }

		private readonly ITimer timer;

		public TaskWaitForSeconds(float seconds)
		{
			timer = new Timer<T>(seconds);
		}
	}

	public class TaskWaitForFrames<T> : YieldInstruction where T : ILoopEvent
	{
		public override bool KeepWaiting { get { return timer.GetFramesLeft() > 0; } }

		private readonly IFrameTimer timer;

		public TaskWaitForFrames(int frameCount)
		{
			timer = new FrameTimer<T>(frameCount);
		}
	}
}
