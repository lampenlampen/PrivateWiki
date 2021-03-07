using System;
using Windows.UI.Xaml.Data;
using PrivateWiki.DataModels;

namespace PrivateWiki.UWP.UI.UI.XamlConverter
{
	class SyncFrequencyToRadioButtonConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var button = Enum.Parse<SyncFrequency>((string) parameter);

			var result = button.Equals(value);

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}