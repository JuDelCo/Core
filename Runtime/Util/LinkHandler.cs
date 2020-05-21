
namespace Ju
{
	public interface ILinkHandler
	{
		bool IsActive { get; }
		bool IsDestroyed { get; }
	}

	public struct KeepLinkHandler : ILinkHandler
	{
		public bool IsActive => true;
		public bool IsDestroyed => false;
	}
}
