// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;

namespace Ju.FSM
{
	public abstract class State
	{
		protected IFiniteStateMachine fsm = null;
		protected Func<bool> extraCondition = null;

		public virtual bool Condition()
		{
			return false;
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

		internal void InternalSetFSM(IFiniteStateMachine fsm)
		{
			this.fsm = fsm;
		}

		public bool IsAllow()
		{
			return Condition() && (extraCondition == null || extraCondition());
		}

		public bool IsCurrent()
		{
			return (fsm != null && fsm.CurrentState == this);
		}
	}
}
