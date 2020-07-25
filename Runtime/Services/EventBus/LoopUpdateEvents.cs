
namespace Ju.Services
{
	public struct LoopUpdateEvent
	{
		public float deltaTime;

		public LoopUpdateEvent(float deltaTime)
		{
			this.deltaTime = deltaTime;
		}
	}

	public struct LoopFixedUpdateEvent
	{
		public float fixedDeltaTime;

		public LoopFixedUpdateEvent(float fixedDeltaTime)
		{
			this.fixedDeltaTime = fixedDeltaTime;
		}
	}
}
