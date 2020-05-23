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

		public bool HasSubscribers()
		{
			return (actions.Count > 0);
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
}
