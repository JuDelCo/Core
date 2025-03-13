// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

namespace Ju.Services
{
	public abstract class YieldTaskBase : YieldInstruction
	{
		public override object Current { get { return null; } }

		public override bool MoveNext()
		{
			return KeepWaiting;
		}

		public override void Reset()
		{
		}
	}
}
