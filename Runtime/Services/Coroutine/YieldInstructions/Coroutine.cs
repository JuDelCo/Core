// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System.Collections;

namespace Ju.Services
{
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
}
