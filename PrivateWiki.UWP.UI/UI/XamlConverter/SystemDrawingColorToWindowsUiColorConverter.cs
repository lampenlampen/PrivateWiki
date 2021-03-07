using System;
using System.Drawing;
using Windows.UI.Xaml.Data;
using PrivateWiki.UWP.UI.Utilities.ExtensionFunctions;

namespace PrivateWiki.UWP.UI.UI.XamlConverter
{
	class SystemDrawingColorToWindowsUiColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			// TODO 

			if (value is Color color)
			{
				return color.ToWindowsUiColor();
			}
			else if (value is DataModels.Pages.Color color2)
			{
				return color2.ToWindowsUiColor();
			}

			throw new ArgumentException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}