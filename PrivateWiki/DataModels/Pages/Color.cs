using System.Drawing;

namespace PrivateWiki.DataModels.Pages
{
	public readonly struct Color
	{
		public byte R { get; }
		public byte G { get; }
		public byte B { get; }

		public Color(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}
	}
	
	public static class Color2Extensions
	{
		public static Color ToColor(this System.Drawing.Color color) => new Color(color.R, color.G, color.B);

		public static System.Drawing.Color ToColor(this Color color) => System.Drawing.Color.FromArgb(color.R, color.G, color.B);
	}
}