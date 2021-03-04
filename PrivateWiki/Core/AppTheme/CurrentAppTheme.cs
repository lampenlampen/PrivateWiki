namespace PrivateWiki.Core.AppTheme
{
	public class CurrentAppTheme
	{
		public DataModels.AppTheme AppTheme { get; }

		public CurrentAppTheme(DataModels.AppTheme appTheme)
		{
			AppTheme = appTheme;
		}
	}
}