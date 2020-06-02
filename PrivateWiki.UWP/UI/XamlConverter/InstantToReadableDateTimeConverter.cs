using System;
using Windows.UI.Xaml.Data;
using NodaTime;

namespace PrivateWiki.UWP.UI.XamlConverter
{
	class InstantToReadableDateTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var time = (Instant) value;
			var timeZone = DateTimeZoneProviders.Tzdb["Europe/Berlin"];
			var zonedDateTime = time.InZone(timeZone);

			return $"{zonedDateTime.Day}.{zonedDateTime.Month}.{zonedDateTime.Year} {zonedDateTime.Hour}:{zonedDateTime.Minute}";

			return zonedDateTime.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}