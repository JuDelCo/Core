using System;
using System.Collections.Generic;

namespace Ju
{
	public class ObservableUnsubscriber<T> : IDisposable
	{
		private List<Action<T>> actions;
		private Action<T> action;
		private bool disposed = false;

		public ObservableUnsubscriber(List<Action<T>> actions, Action<T> action)
		{
			this.actions = actions;
			this.action = action;
		}

		~ObservableUnsubscriber()
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
				if (action != null && actions.Contains(action))
				{
					actions.Remove(action);
				}

				disposed = true;

				if (!disposing)
				{
					Services.Get<ILogService>().Error("An Observable handle did not dispose correctly and was cleaned up by the GC.");
				}
			}
		}
	}
}
