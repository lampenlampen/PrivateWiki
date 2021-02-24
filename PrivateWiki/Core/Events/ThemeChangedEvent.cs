namespace PrivateWiki.Core.Events
{
	public class ThemeChangedEvent : Event<ThemeChangedEventArgs>
	{
		
	}

	public class ThemeChangedEventArgs
	{
		public AppTheme NewTheme { get; }

		public ThemeChangedEventArgs(AppTheme newTheme)
		{
			NewTheme = newTheme;
		}
	}

	public enum AppTheme
	{
		Light,
		Dark,
		System
	}
}