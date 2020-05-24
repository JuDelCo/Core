using System;

namespace Ju
{
	public abstract class State
	{
		protected FSM fsm = null;
		protected Func<bool> extraCondition = null;

		public virtual bool Condition()
		{
			return true;
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnTick()
		{
		}

		public virtual void OnFixedTick()
		{
		}

		public virtual void OnExit()
		{
		}

		internal void InternalSetFSM(FSM fsm)
		{
			this.fsm = fsm;
		}

		public bool IsAllow()
		{
			return Condition() && (extraCondition == null ? true : extraCondition());
		}

		public bool IsCurrent()
		{
			return (fsm == null ? false : fsm.CurrentState == this);
		}
	}
}
