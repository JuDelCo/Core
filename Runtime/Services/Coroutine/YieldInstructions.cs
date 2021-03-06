// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections;
using Ju.Time;

namespace Ju.Services
{
	public abstract class YieldInstruction : IEnumerator
	{
		public abstract bool KeepWaiting { get; }
		public abstract object Current { get; }

		public abstract bool MoveNext();
		public abstract void Reset();
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

	public abstract class YieldTaskBase : YieldInstruction
	{
		public override object Current { get { return null; } }

		public override bool MoveNext()
		{
			return KeepWaiting;
		}

		public override void Reset()
		{
		}
	}

	public class TaskWaitUntil : YieldTaskBase
	{
		public override bool KeepWaiting { get { return !condition(); } }

		private readonly Func<bool> condition;

		public TaskWaitUntil(Func<bool> condition)
		{
			this.condition = condition;
		}
	}

	public class TaskWaitWhile : YieldTaskBase
	{
		public override bool KeepWaiting { get { return condition(); } }

		private readonly Func<bool> condition;

		public TaskWaitWhile(Func<bool> condition)
		{
			this.condition = condition;
		}
	}

	public class TaskWaitForSeconds<T> : YieldTaskBase where T : ILoopTimeEvent
	{
		public override bool KeepWaiting { get { return timer.GetTimeLeft().seconds > 0f; } }

		private readonly ITimer timer;

		public TaskWaitForSeconds(float seconds)
		{
			timer = new Timer<T>(seconds);
		}
	}

	public class TaskWaitForTicks<T> : YieldTaskBase where T : ILoopEvent
	{
		public override bool KeepWaiting { get { return timer.GetFramesLeft() > 0; } }

		private readonly IFrameTimer timer;

		public TaskWaitForTicks(int ticks)
		{
			timer = new FrameTimer<T>(ticks);
		}
	}

	public class TaskWaitForNextUpdate : TaskWaitForTicks<LoopUpdateEvent>
	{
		public TaskWaitForNextUpdate() : base(1)
		{
		}
	}

	public class TaskWaitForNextFixedUpdate : TaskWaitForTicks<LoopFixedUpdateEvent>
	{
		public TaskWaitForNextFixedUpdate() : base(1)
		{
		}
	}
}
