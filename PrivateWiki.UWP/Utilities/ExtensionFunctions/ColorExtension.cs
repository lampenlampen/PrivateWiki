using Windows.UI;
using Windows.UI.Xaml.Media;

namespace PrivateWiki.Utilities.ExtensionFunctions
{
	static class ColorExtension
	{
		public static Windows.UI.Color ToWindowsUiColor(this System.Drawing.Color color)
		{
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		public static SolidColorBrush ToBrush(this System.Drawing.Color color)
		{
			return new SolidColorBrush(color.ToWindowsUiColor());
		}
	}
}