
namespace Ju.Extensions
{
	public static class BoolExtensions
	{
		public static uint ToInt(this bool boolean)
		{
			return (uint)(boolean ? 1 : 0);
		}
	}
}
