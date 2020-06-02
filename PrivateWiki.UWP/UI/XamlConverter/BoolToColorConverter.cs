using System;
using System.Drawing;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using PrivateWiki.UWP.Utilities.ExtensionFunctions;

#nullable enable

namespace PrivateWiki.UWP.UI.XamlConverter
{
	class EnableStateToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (!string.IsNullOrEmpty(value.ToString()))
			{
				var model = (bool) value;
				if (model)
				{
					return new SolidColorBrush(Color.LimeGreen.ToWindowsUiColor());
				}
				else
				{
					return new SolidColorBrush(Color.Red.ToWindowsUiColor());
				}
			}

			return new SolidColorBrush(Color.Red.ToWindowsUiColor());
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}