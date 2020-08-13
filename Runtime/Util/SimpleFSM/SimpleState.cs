using System;

namespace Ju.FSM
{
	internal class SimpleState
	{
		public Func<bool> condition;
		public Action action;

		public SimpleState(Func<bool> condition, Action action)
		{
			this.condition = condition;
			this.action = action;
		}
	}
}