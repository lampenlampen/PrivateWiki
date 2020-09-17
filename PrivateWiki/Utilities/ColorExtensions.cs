using System.Drawing;
using System.Globalization;

namespace PrivateWiki.Utilities
{
	public static class ColorExtensions
	{
		public static Color HexToColor(this string hexString)
		{
			//replace # occurences
			if (hexString.IndexOf('#') != -1)
				hexString = hexString.Replace("#", "");

			var r = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
			var g = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
			var b = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

			return Color.FromArgb(r, g, b);
		}

		public static string ToHexColor(this Color color) => $"#{color.R:X2}{color.G:X2}{color.B:X2}";
	}
}