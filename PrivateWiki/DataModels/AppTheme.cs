namespace PrivateWiki.DataModels
{
	public class AppTheme
	{
		public static AppTheme DefaultLightModeTheme = new(false);
		public static AppTheme DefaultDarkModeTheme = new(true);

		public bool DarkMode { get; }

		public AppTheme(bool darkMode = false)
		{
			DarkMode = darkMode;
		}
	}
}