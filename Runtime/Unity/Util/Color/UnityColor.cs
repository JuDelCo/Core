
#if UNITY_2019_3_OR_NEWER

namespace Ju.ColorUtil
{
	public partial struct Color
	{
		public static implicit operator UnityEngine.Color(Color color)
		{
			return new UnityEngine.Color(color.r, color.g, color.b, color.a);
		}

		public static implicit operator Color(UnityEngine.Color color)
		{
			return new Color(color.r, color.g, color.b, color.a);
		}

		public static implicit operator UnityEngine.Color32(Color color)
		{
			return new UnityEngine.Color32((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
		}

		public static implicit operator Color(UnityEngine.Color32 color)
		{
			return new Color(color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
		}
	}
}

#endif
