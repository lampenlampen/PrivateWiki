namespace PrivateWiki.Core.Events
{
	public class ThemeChangedEvent : Event<ThemeChangedEventArgs> { }

	public class ThemeChangedEventArgs
	{
		public DataModels.AppTheme NewTheme { get; }

		public ThemeChangedEventArgs(DataModels.AppTheme newTheme)
		{
			NewTheme = newTheme;
		}
	}
}