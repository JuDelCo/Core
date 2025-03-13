// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Time;

namespace Ju.FSM
{
	public class SimpleFSM
	{
		public int StateTickCounter { get; private set; }
		public float StateTimer { get; private set; }

		private Action currentState;
		private TimeSince timeSinceStateChange;
		private readonly Dictionary<string, int> ids = new Dictionary<string, int>();
		private readonly Dictionary<int, Action> idStates = new Dictionary<int, Action>();
		private readonly Dictionary<int, Func<bool>> conditionStates = new Dictionary<int, Func<bool>>();

		public void AddState(Action state)
		{
			AddState(idStates.Count, state);
		}

		public void AddState(int stateId, Action state)
		{
			if (idStates.ContainsKey(stateId))
			{
				idStates.Remove(stateId);
			}

			idStates.Add(stateId, state);
		}

		public void AddState(string stateName, Action state)
		{
			var stateId = GetStateId(stateName);

			if (stateId == -1)
			{
				stateId = idStates.Count;
				ids.Add(stateName, stateId);
			}

			AddState(stateId, state);
		}

		public void AddState(Func<bool> condition, Action state)
		{
			AddState(idStates.Count, condition, state);
		}

		public void AddState(int stateId, Func<bool> condition, Action state)
		{
			AddState(stateId, state);
			conditionStates.Add(stateId, condition);
		}

		public void AddState(string stateName, Func<bool> condition, Action state)
		{
			var stateId = GetStateId(stateName);

			if (stateId == -1)
			{
				stateId = idStates.Count;
				ids.Add(stateName, stateId);
			}

			AddState(stateId, condition, state);
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

		private void SetState(Action state)
		{
			if (state != null && currentState != state)
			{
				StateTickCounter = 0;
				timeSinceStateChange = 0f;
				StateTimer = 0f;

				currentState = state;
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

		public void Tick()
		{
			++StateTickCounter;
			StateTimer = timeSinceStateChange;

			if (currentState != null)
			{
				currentState();
			}

			if (conditionStates.Count > 0)
			{
				foreach (var kvp in conditionStates)
				{
					if (kvp.Value != null && kvp.Value())
					{
						SetState(kvp.Key);
						break;
					}
				}
			}
		}
	}
}
