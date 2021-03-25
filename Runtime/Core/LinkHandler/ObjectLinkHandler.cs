// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Handlers
{
	public struct ObjectLinkHandler<T> : ILinkHandler
	{
		private readonly WeakReference objectRef;

		public ObjectLinkHandler(T obj)
		{
			this.objectRef = new WeakReference(obj);
		}

		public bool IsActive => !IsDestroyed;
		public bool IsDestroyed => !objectRef.IsAlive || ((T)objectRef.Target) == null;
	}
}
