namespace PrivateWiki.DataModels
{
	public class AppTheme
	{
		public static AppTheme DefaultLightModeTheme = new("Dark Theme", true);
		public static AppTheme DefaultDarkModeTheme = new("Light Theme");

		public bool DarkMode { get; }

		public string Name { get; }

		public AppTheme(string name, bool darkMode = false)
		{
			Name = name;
			DarkMode = darkMode;
		}
	}
}