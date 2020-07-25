
#if UNITY_2018_3_OR_NEWER

using Ju.Services;
using UnityEngine;

namespace Ju.Unity
{
	public abstract class MonoBehaviourActor : MonoBehaviour
	{
		protected virtual void OnEnable()
		{
			ServiceContainer.Get<IDataService>().ListAdd(this);
		}

		protected virtual void OnDisable()
		{
			ServiceContainer.Get<IDataService>().ListRemove(this);
		}
	}
}

#endif
