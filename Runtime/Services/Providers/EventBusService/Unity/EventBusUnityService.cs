
#if UNITY_2018_3_OR_NEWER

using UnityEngine;
using Handle = System.Object;

namespace Ju
{
	public class EventBusUnityService : EventBusService
	{
		public override void Setup()
		{
			base.Setup();

			SetEnabledHandleTest((Handle handle) =>
			{
				var result = true;

				if (handle.GetType().IsSubclassOf(typeof(Behaviour)))
				{
					result = ((Behaviour)handle).isActiveAndEnabled;
				}

				return result;
			});
		}
	}
}

#endif
