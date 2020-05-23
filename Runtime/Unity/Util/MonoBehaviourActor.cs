
#if UNITY_2018_3_OR_NEWER

using UnityEngine;

namespace Ju
{
	public abstract class MonoBehaviourActor : MonoBehaviour
	{
		protected virtual void OnEnable()
		{
			Services.Get<IDataService>().ListAdd(this);
		}

		protected virtual void OnDisable()
		{
			Services.Get<IDataService>().ListRemove(this);
		}
	}
}

#endif
