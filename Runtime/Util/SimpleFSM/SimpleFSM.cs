using System;
using System.Collections.Generic;

namespace Ju
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

			foreach (var state in states)
			{
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
