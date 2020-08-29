
#if UNITY_2019_3_OR_NEWER

namespace Ju.ColorUtil
{
	public partial struct Color32
	{
		public static implicit operator UnityEngine.Color32(Color32 color)
		{
			return new UnityEngine.Color32(color.r, color.g, color.b, color.a);
		}

		public static explicit operator Color32(UnityEngine.Color32 color)
		{
			return new Color32(color.r, color.g, color.b, color.a);
		}
	}
}

#endif
