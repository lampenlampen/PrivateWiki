using System;
using System.Drawing;
using Windows.UI.Xaml.Data;
using PrivateWiki.UWP.Utilities.ExtensionFunctions;

namespace PrivateWiki.UWP.UI.XamlConverter
{
	class SystemDrawingColorToWindowsUiColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is Color color)
			{
				return color.ToWindowsUiColor();
			}

			throw new ArgumentException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}