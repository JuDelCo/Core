
#if UNITY_2018_3_OR_NEWER

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ju
{
	public abstract class MonoBehaviourActor : MonoBehaviour
	{
		protected bool enableAutoDataService;
		protected bool enableAutoObservables;
		protected List<IDisposable> observableUnsubscribers;

		protected virtual void Awake()
		{
			enableAutoDataService = false;
			enableAutoObservables = false;
			observableUnsubscribers = null;
		}

		protected virtual void OnEnable()
		{
			if (enableAutoDataService)
			{
				Services.Get<IDataService>().ListAdd(this);
			}

			if (enableAutoObservables)
			{
				if (observableUnsubscribers == null)
				{
					observableUnsubscribers = new List<IDisposable>();
				}
			}
		}

		protected virtual void OnDisable()
		{
			if (enableAutoDataService)
			{
				Services.Get<IDataService>().ListRemove(this);
			}

			if (enableAutoObservables)
			{
				foreach (var observableUnsubscriber in observableUnsubscribers)
				{
					observableUnsubscriber.Dispose();
				}

				observableUnsubscribers.Clear();
			}
		}

		public void AddObservableUnsubscriber(IDisposable disposable)
		{
			observableUnsubscribers.Add(disposable);
		}
	}
}

#endif
