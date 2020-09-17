﻿using Windows.UI;
using Windows.UI.Xaml.Media;

namespace PrivateWiki.UWP.Utilities.ExtensionFunctions
{
	static class ColorExtension
	{
		public static Windows.UI.Color ToWindowsUiColor(this System.Drawing.Color color) => Color.FromArgb(color.A, color.R, color.G, color.B);

		public static SolidColorBrush ToBrush(this System.Drawing.Color color) => new SolidColorBrush(color.ToWindowsUiColor());

		public static System.Drawing.Color ToSystemDrawingColor(this Color color) => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
	}
}