// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

using System;

namespace Ju.Handlers
{
	using Ju.Data;
	using Ju.Log;

	public class JNodeLinkHandler : ILinkHandler, IDisposable
	{
		private readonly JNode node;
		private readonly bool showUndisposedWarning;
		private bool disposed;

		public JNode Node => node;
		public bool IsActive => (!disposed && !node.IsDisposed());
		public bool IsDestroyed => (disposed || node.IsDisposed());

		public JNodeLinkHandler(JNode node, bool showUndisposedWarning)
		{
			this.node = node;
			this.showUndisposedWarning = showUndisposedWarning;
		}

		~JNodeLinkHandler()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				disposed = true;

				if (!disposing && showUndisposedWarning)
				{
					Log.Error("A linkhandler did not dispose correctly and was cleaned up by the GC.");
				}
			}
		}
	}
}
