// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System.Collections;

namespace Ju.Services
{
	public abstract class YieldInstruction : IEnumerator
	{
		public abstract bool KeepWaiting { get; }
		public abstract object Current { get; }

		public abstract bool MoveNext();
		public abstract void Reset();
	}
}
