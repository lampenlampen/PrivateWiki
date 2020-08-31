using System.Globalization;

namespace PrivateWiki.Utilities
{
	public static class ColorExtensions
	{
		public static System.Drawing.Color HexToColor(this string hexString)
		{
			//replace # occurences
			if (hexString.IndexOf('#') != -1)
				hexString = hexString.Replace("#", "");

			int r, g, b = 0;

			r = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
			g = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
			b = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

			return System.Drawing.Color.FromArgb(r, g, b);
		}
	}
}