using System;
using Windows.UI.Xaml.Data;

namespace PrivateWiki.UWP.UI.XamlConverter
{
	public class NegateBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return !(value is bool);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return !(value is bool);
		}
	}
}