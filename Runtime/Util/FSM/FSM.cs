using System.Collections.Generic;

namespace Ju
{
	public class FSM
	{
		public State CurrentState { get; private set; }
		public int StateFrameCounter { get; private set; }
		public float StateTimer { get; private set; }

		private List<State> states = new List<State>();

		protected virtual void SetupState(State state)
		{
		}

		public void AddState(State state)
		{
			states.Add(state);
			state.InternalSetFSM(this);
			SetupState(state);
		}

		public void SetState(State state)
		{
			if (state != null && CurrentState != state && states.Contains(state))
			{
				if (CurrentState != null)
				{
					CurrentState.OnExit();
				}

				StateFrameCounter = 0;
				StateTimer = 0f;

				CurrentState = state;
				CurrentState.OnEnter();
			}
		}

		public void Tick(float deltaTime)
		{
			++StateFrameCounter;
			StateTimer += deltaTime;

			if (CurrentState != null)
			{
				CurrentState.OnTick();
			}

			foreach (var state in states)
			{
				if (state.IsAllow())
				{
					SetState(state);
					break;
				}
			}
		}

		public void FixedTick()
		{
			if (CurrentState != null)
			{
				CurrentState.OnFixedTick();
			}
		}
	}
}
