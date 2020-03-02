
#if UNITY_2018_3_OR_NEWER

using UnityEngine;

namespace Ju
{
	public abstract class MonoBehaviourSubscriber : MonoBehaviour
	{
		protected virtual void OnEnable()
		{
			Services.Get<IEventBusService>().Enable(this);
		}

		protected virtual void OnDisable()
		{
			Services.Get<IEventBusService>().Disable(this);
		}

		protected virtual void OnDestroy()
		{
			Services.Get<IEventBusService>().UnSuscribe(this);
		}
	}
}

#endif
