using System;
using System.Drawing;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using PrivateWiki.UWP.Utilities.ExtensionFunctions;

namespace PrivateWiki.UWP.UI.XamlConverter
{
	public class SystemDrawingColorToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			// TODO 

			if (value is Color color)
			{
				return new SolidColorBrush(color.ToWindowsUiColor());
			}
			else if (value is DataModels.Pages.Color color2)
			{
				return new SolidColorBrush(color2.ToWindowsUiColor());
			}

			throw new ArgumentException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}