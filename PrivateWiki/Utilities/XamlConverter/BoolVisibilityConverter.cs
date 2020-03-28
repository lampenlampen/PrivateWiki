using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PrivateWiki.Utilities.XamlConverter
{
	class BoolVisibilityConverter : IValueConverter
	{
		public BoolVisibilityConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is bool b && b)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return (value is Visibility visibility && visibility == Visibility.Visible);
		}
    }
}
