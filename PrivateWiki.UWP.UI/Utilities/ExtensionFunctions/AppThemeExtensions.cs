using Windows.UI.Xaml;
using PrivateWiki.DataModels;

namespace PrivateWiki.UWP.UI.Utilities.ExtensionFunctions
{
	public static class AppThemeExtensions
	{
		public static ElementTheme ToPlatformTheme(this AppTheme theme) =>
			theme.DarkMode switch
			{
				false => ElementTheme.Light,
				true => ElementTheme.Dark
			};
	}
}