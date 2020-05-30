using System;

namespace Ju
{
	public class DisposableLinkHandler : ILinkHandler, IDisposable
	{
		private bool showUndisposedWarning;
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
					Services.Get<ILogService>().Error("A linkhandler did not dispose correctly and was cleaned up by the GC.");
				}
			}
		}
	}
}
