// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Handlers;

namespace Ju.Observables
{
	internal class ObservableHandleActionPair<T>
	{
		public ILinkHandler handle;
		public Action<T> action;

		public ObservableHandleActionPair(ILinkHandler handle, Action<T> action)
		{
			this.handle = handle;
			this.action = action;
		}
	}

	public class Observable<T>
	{
		public T Value
		{
			get => value;
			set => SetValue(value);
		}

		private readonly List<ObservableHandleActionPair<T>> actions = null;
		private T value;
		private uint callStackCounter = 0;

		public Observable(T initialValue)
		{
			actions = new List<ObservableHandleActionPair<T>>();
			value = initialValue;
		}

		public void Subscribe(ILinkHandler handle, Action<T> action)
		{
			actions.Add(new ObservableHandleActionPair<T>(handle, action));
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

				var handle = actions[i].handle;

				if (handle.IsDestroyed)
				{
					actions.RemoveAt(i);

					continue;
				}

				if (!handle.IsActive)
				{
					continue;
				}

				callStackCounter++;

				actions[i].action(value);

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
