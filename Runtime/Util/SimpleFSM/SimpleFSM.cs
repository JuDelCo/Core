using System;
using System.Collections.Generic;

namespace Ju.FSM
{
	public class SimpleFSM
	{
		private List<SimpleState> states = new List<SimpleState>();
		private SimpleState currentState = null;

		public void AddState(Func<bool> condition, Action action)
		{
			states.Add(new SimpleState(condition, action));
		}

		public void Tick()
		{
			if (currentState != null)
			{
				currentState.action();
			}

			for (int i = 0, count = states.Count; i < count; ++i)
			{
				var state = states[i];

				if (state.condition == null || state.condition())
				{
					if (currentState != state)
					{
						currentState = state;
					}

					break;
				}
			}
		}
	}
}
