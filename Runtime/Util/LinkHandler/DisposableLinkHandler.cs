// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Services;

namespace Ju.Handlers
{
	public class DisposableLinkHandler : ILinkHandler, IDisposable
	{
		private readonly bool showUndisposedWarning;
		private bool disposed;

		public bool IsActive => !disposed;
		public bool IsDestroyed => disposed;

		public DisposableLinkHandler(bool showUndisposedWarning)
		{
			this.showUndisposedWarning = showUndisposedWarning;
		}

		~DisposableLinkHandler()
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
					ServiceContainer.Get<ILogService>().Error("A linkhandler did not dispose correctly and was cleaned up by the GC.");
				}
			}
		}
	}
}
