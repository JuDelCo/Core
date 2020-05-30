using System;

namespace Ju
{
	public struct ObjectLinkHandler<T> : ILinkHandler
	{
		private WeakReference objectRef;

		public ObjectLinkHandler(T obj)
		{
			this.objectRef = new WeakReference(obj);
		}

		public bool IsActive => !IsDestroyed;
		public bool IsDestroyed => !objectRef.IsAlive || ((T)objectRef.Target) == null;
	}
}
