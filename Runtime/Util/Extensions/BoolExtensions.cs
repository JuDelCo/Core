
namespace Ju.Extensions
{
	public static class BoolExtensions
	{
		public static uint ToUInt(this bool boolean)
		{
			return (uint)(boolean ? 1 : 0);
		}

		public static int ToInt(this bool boolean)
		{
			return (boolean ? 1 : 0);
		}
	}
}
