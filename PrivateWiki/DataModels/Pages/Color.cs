namespace PrivateWiki.DataModels.Pages
{
	public record Color
	{
		public byte R { get; init; }
		public byte G { get; init; }
		public byte B { get; init; }

		public Color() { }

		public Color(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}
	}

	public static class ColorExtensions
	{
		public static Color ToColor(this System.Drawing.Color color) => new Color(color.R, color.G, color.B);

		public static System.Drawing.Color ToSystemDrawingColor(this Color color) => System.Drawing.Color.FromArgb(color.R, color.G, color.B);
	}
}