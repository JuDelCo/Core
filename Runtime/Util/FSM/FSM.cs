using System.Collections.Generic;

namespace Ju.FSM
{
	public interface IFiniteStateMachine
	{
		State CurrentState { get; }
		int StateFrameCounter { get; }
		float StateTimer { get; }

		void SetState(string stateId);
	}

	public class FSM : IFiniteStateMachine
	{
		public State CurrentState { get; private set; }
		public int StateFrameCounter { get; private set; }
		public float StateTimer { get; private set; }

		private readonly List<State> states = new List<State>();
		private readonly Dictionary<int, State> idStates = new Dictionary<int, State>();
		private readonly Dictionary<string, int> ids = new Dictionary<string, int>();

		protected virtual void SetupState(State state)
		{
		}

		public void AddState(State state)
		{
			states.Add(state);
			state.InternalSetFSM(this);
			SetupState(state);
		}

		public void AddState(int stateId, State state)
		{
			AddState(state);
			idStates.Add(stateId, state);
		}

		public void AddState(string stateName, State state)
		{
			var stateId = GetStateId(stateName);

			if (stateId == -1)
			{
				stateId = idStates.Count;
			}

			AddState(stateId, state);
		}

		public void SetState(int stateId)
		{
			if (idStates.ContainsKey(stateId))
			{
				SetState(idStates[stateId]);
			}
		}

		public void SetState(string stateName)
		{
			SetState(GetStateId(stateName));
		}

		private void SetState(State state)
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

		public int GetStateId(string stateName)
		{
			if (ids.ContainsKey(stateName))
			{
				return ids[stateName];
			}

			return -1;
		}

		public void Tick(float deltaTime)
		{
			++StateFrameCounter;
			StateTimer += deltaTime;

			if (CurrentState != null)
			{
				CurrentState.OnTick();
			}

			for (int i = 0, count = states.Count; i < count; ++i)
			{
				var state = states[i];

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
