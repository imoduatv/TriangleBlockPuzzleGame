using UnityEngine;

namespace Archon.SwissArmyLib.Utils
{
	public static class ColorUtils
	{
		public static string ToHex(this Color color)
		{
			Color32 color2 = color;
			return $"{color2.r:X2}{color2.g:X2}{color2.b:X2}{color2.a:X2}";
		}

		public static string RichTextColor(string text, Color color)
		{
			return $"<color=#{color.ToHex()}>{text}</color>";
		}
	}
}
