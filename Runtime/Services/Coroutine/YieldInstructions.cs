using System;
using System.Collections;

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
}
