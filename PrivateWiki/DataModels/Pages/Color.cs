namespace PrivateWiki.DataModels.Pages
{
	public class Color
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

		public override string ToString() => $"Color[R={R}, G={G}, B={B}]";

		protected bool Equals(Color other)
		{
			return R == other.R && G == other.G && B == other.B;
		}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Color) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = R.GetHashCode();
				hashCode = (hashCode * 397) ^ G.GetHashCode();
				hashCode = (hashCode * 397) ^ B.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(Color? left, Color? right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Color? left, Color? right)
		{
			return !Equals(left, right);
		}
	}

	public static class Color2Extensions
	{
		public static Color ToColor(this System.Drawing.Color color) => new Color(color.R, color.G, color.B);

		public static System.Drawing.Color ToSystemDrawingColor(this Color color) => System.Drawing.Color.FromArgb(color.R, color.G, color.B);
	}
}