using System;
using System.Collections.Generic;

namespace Ju.Input
{
	public class AIInputAction : IAIInputAction, IInputAction
	{
		public event InputServiceActionEvent OnPressed = delegate { };
		public event InputServiceActionEvent OnHeld = delegate { };
		public event InputServiceActionEvent OnReleased = delegate { };

		public string Id { get; }
		public IInputPlayer Player { get; private set; }
		public bool Enabled { get; set; }
		public IEnumerable<MouseButton> MouseButtonBindings => null;
		public IEnumerable<KeyboardKey> KeyboardKeyBindings => null;
		public IEnumerable<GamepadButton> GamepadButtonBindings => null;
		public IEnumerable<GamepadAxis> GamepadAxisBindings => null;

		private bool pressed;
		private float pressedDuration;
		private bool previousPressed;
		private float axisValue;
		private float axisRawValueX;
		private float axisRawValueY;
		private bool eventTriggered;

		public AIInputAction(string id)
		{
			Id = id;
			Enabled = true;
		}

		public void ResetState()
		{
			axisValue = 0f;
			axisRawValueX = 0f;
			axisRawValueY = 0f;

			previousPressed = pressed;
			pressed = false;

			eventTriggered = false;
		}

		public void CheckState(float deltaTime)
		{
			if (pressed && previousPressed)
			{
				pressedDuration += deltaTime;
			}
			else
			{
				pressedDuration = 0f;
			}
		}

		public void Set(bool value)
		{
			if (Enabled)
			{
				pressed = value;

				if (pressed && axisValue == 0f)
				{
					axisValue = 1f;
				}
			}

			TriggerEvents();
		}

		public void Set(float value)
		{
			if (Enabled)
			{
				axisValue = (value < -1f ? -1f : (value > 1f ? 1f : value));

				if (!pressed && (axisValue != 0f))
				{
					pressed = true;
				}
			}

			TriggerEvents();
		}

		public void Set(float valueX, float valueY)
		{
			if (Enabled)
			{
				axisRawValueX = valueX;
				axisRawValueY = valueY;

				if (!pressed && (axisRawValueX != 0f || axisRawValueY != 0f))
				{
					pressed = true;
				}
			}

			TriggerEvents();
		}

		private void TriggerEvents()
		{
			if (eventTriggered)
			{
				return;
			}

			if (!previousPressed && pressed)
			{
				eventTriggered = true;
				OnPressed(this);
			}

			if (previousPressed && pressed)
			{
				eventTriggered = true;
				OnHeld(this);
			}

			if (previousPressed && !pressed)
			{
				eventTriggered = true;
				OnReleased(this);
			}
		}

		public void ResetBindings()
		{
			throw new InvalidOperationException();
		}

		public IInputAction AddBinding(MouseButton button)
		{
			throw new InvalidOperationException();
		}

		public IInputAction AddBinding(MouseButton positiveButton, MouseButton negativeButton)
		{
			throw new InvalidOperationException();
		}

		public IInputAction AddBinding(KeyboardKey key)
		{
			throw new InvalidOperationException();
		}

		public IInputAction AddBinding(KeyboardKey positiveKey, KeyboardKey negativeKey)
		{
			throw new InvalidOperationException();
		}

		public IInputAction AddBinding(GamepadButton button)
		{
			throw new InvalidOperationException();
		}

		public IInputAction AddBinding(GamepadButton positiveButton, GamepadButton negativeButton)
		{
			throw new InvalidOperationException();
		}

		public void RemoveBinding(MouseButton button)
		{
			throw new InvalidOperationException();
		}

		public void RemoveBinding(KeyboardKey key)
		{
			throw new InvalidOperationException();
		}

		public void RemoveBinding(GamepadButton button)
		{
			throw new InvalidOperationException();
		}

		public bool IsPressed()
		{
			return pressed && !previousPressed;
		}

		public bool IsHeld()
		{
			return pressed;
		}

		public float HeldDuration()
		{
			return pressedDuration;
		}

		public bool IsReleased()
		{
			return !pressed && previousPressed;
		}

		public IInputAction AddBinding(GamepadAxis axis)
		{
			throw new InvalidOperationException();
		}

		public void RemoveBinding(GamepadAxis axis)
		{
			throw new InvalidOperationException();
		}

		public float GetAxisValue()
		{
			return axisValue;
		}

		public void GetAxisRawValue(out float axisX, out float axisY)
		{
			axisX = axisRawValueX;
			axisY = axisRawValueY;
		}
	}
}
