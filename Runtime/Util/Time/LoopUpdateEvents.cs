
namespace Ju.Time
{
	public interface ILoopEvent
	{
	}

	public interface ILoopTimeEvent : ILoopEvent
	{
		float DeltaTime { get; }
	}

	public struct LoopPreUpdateEvent : ILoopTimeEvent
	{
		public float DeltaTime { get; private set; }

		public LoopPreUpdateEvent(float deltaTime)
		{
			DeltaTime = deltaTime;
		}
	}

	public struct LoopUpdateEvent : ILoopTimeEvent
	{
		public float DeltaTime { get; private set; }

		public LoopUpdateEvent(float deltaTime)
		{
			DeltaTime = deltaTime;
		}
	}

	public struct LoopPostUpdateEvent : ILoopTimeEvent
	{
		public float DeltaTime { get; private set; }

		public LoopPostUpdateEvent(float deltaTime)
		{
			DeltaTime = deltaTime;
		}
	}

	public struct LoopPreFixedUpdateEvent : ILoopTimeEvent
	{
		public float DeltaTime { get; private set; }

		public LoopPreFixedUpdateEvent(float fixedDeltaTime)
		{
			DeltaTime = fixedDeltaTime;
		}
	}

	public struct LoopFixedUpdateEvent : ILoopTimeEvent
	{
		public float DeltaTime { get; private set; }

		public LoopFixedUpdateEvent(float fixedDeltaTime)
		{
			DeltaTime = fixedDeltaTime;
		}
	}

	public struct LoopPreCollisionsUpdateEvent : ILoopTimeEvent
	{
		public float DeltaTime { get; private set; }

		public LoopPreCollisionsUpdateEvent(float fixedDeltaTime)
		{
			DeltaTime = fixedDeltaTime;
		}
	}

	public struct LoopPostCollisionsUpdateEvent : ILoopTimeEvent
	{
		public float DeltaTime { get; private set; }

		public LoopPostCollisionsUpdateEvent(float fixedDeltaTime)
		{
			DeltaTime = fixedDeltaTime;
		}
	}

	public struct LoopPostFixedUpdateEvent : ILoopTimeEvent
	{
		public float DeltaTime { get; private set; }

		public LoopPostFixedUpdateEvent(float fixedDeltaTime)
		{
			DeltaTime = fixedDeltaTime;
		}
	}
}
