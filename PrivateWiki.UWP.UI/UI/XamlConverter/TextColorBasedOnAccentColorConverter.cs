using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PrivateWiki.UWP.UI.UI.XamlConverter
{
	public class TextColorBasedOnAccentColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is bool a)
			{
				return a ? ElementTheme.Dark : ElementTheme.Light;
			}

			throw new ArgumentException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}