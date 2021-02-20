using Windows.UI.Xaml;
using PrivateWiki.Core.Events;

namespace PrivateWiki.UWP.Utilities.ExtensionFunctions
{
	public static class AppThemeExtensions
	{
		public static ElementTheme ToPlatformTheme(this AppTheme theme) =>
			theme switch
			{
				AppTheme.Light => ElementTheme.Light,
				AppTheme.Dark => ElementTheme.Dark,
				_ => ElementTheme.Default
			};
	}
}