
namespace Ju
{
	public static class FSMExtensions
	{
		public static void AddState(this FSM fsm, int stateId, State state)
		{
			fsm.AddState(stateId.ToString(), state);
		}

		public static void SetState(this FSM fsm, int stateId)
		{
			fsm.SetState(stateId.ToString());
		}

		public static void SetState(this IFiniteStateMachine fsm, int stateId)
		{
			fsm.SetState(stateId.ToString());
		}
	}
}
