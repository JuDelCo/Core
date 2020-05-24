using System;

namespace Ju
{
	public struct StateLinkHandler : ILinkHandler
	{
		private WeakReference stateRef;

		public StateLinkHandler(State state)
		{
			this.stateRef = new WeakReference(state);
		}

		public bool IsActive => !IsDestroyed && ((State)stateRef.Target).IsCurrent();
		public bool IsDestroyed => !stateRef.IsAlive;
	}
}
