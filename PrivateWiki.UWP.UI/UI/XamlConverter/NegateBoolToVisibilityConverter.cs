using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PrivateWiki.UWP.UI.UI.XamlConverter
{
	public class NegateBoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is bool a && a) return Visibility.Collapsed;
			else return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is Visibility a)
			{
				switch (a)
				{
					case Visibility.Visible:
						return false;
					case Visibility.Collapsed:
						return true;
					default:
						throw new ArgumentOutOfRangeException(nameof(a));
				}
			}

			throw new ArgumentException(nameof(value));
		}
	}
}