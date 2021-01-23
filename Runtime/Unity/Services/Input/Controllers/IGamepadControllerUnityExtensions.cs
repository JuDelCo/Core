
#if UNITY_2019_3_OR_NEWER

using Ju.Input;
using UnityEngine;

public static class IGamepadControllerUnityExtensions
{
	public static Vector2 GetAxisRaw(this IGamepadController gamepadController, GamepadAxis axis)
	{
		gamepadController.GetAxisRaw(axis, out float axisX, out float axisY);

		return new Vector2(axisX, axisY);
	}
}

#endif
