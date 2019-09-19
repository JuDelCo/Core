using System;
using System.Collections.Generic;

namespace Ju
{
	public class Observable<T>
	{
		public T Value
		{
			get => value;
			set => SetValue(value);
		}

		private List<Action<T>> actions;
		private T value;
		private uint callStackCounter = 0;

		public Observable(T initialValue)
		{
			actions = new List<Action<T>>();
			value = initialValue;
		}

		public IDisposable Subscribe(Action<T> action)
		{
			if (!actions.Contains(action))
			{
				actions.Add(action);
			}

			return new ObservableUnsubscriber<T>(actions, action);
		}

		public void Trigger()
		{
			for (int i = actions.Count - 1; i >= 0; --i)
			{
				if (callStackCounter > 0)
				{
					throw new Exception("An Observable callback has modified the same value that triggered it.");
				}

				callStackCounter++;
				actions[i](value);
				callStackCounter--;
			}
		}

		private void SetValue(T newValue)
		{
			if (!EqualityComparer<T>.Default.Equals(value, newValue))
			{
				value = newValue;
				Trigger();
			}
		}
	}

	public class ObservableUnsubscriber<T> : IDisposable
	{
		private List<Action<T>> actions;
		private Action<T> action;

		public ObservableUnsubscriber(List<Action<T>> actions, Action<T> action)
		{
			this.actions = actions;
			this.action = action;
		}

		public void Dispose()
		{
			if (action != null && actions.Contains(action))
			{
				actions.Remove(action);
			}
		}
	}
}
