namespace PrivateWiki.Core.AppTheme
{
	public class CurrentAppThemeQueryHandler : IQueryHandler<GetCurrentAppTheme, DataModels.AppTheme>
	{
		private readonly DataModels.AppTheme _defaultAppTheme = DataModels.AppTheme.DefaultLightModeTheme;

		public DataModels.AppTheme Handle(GetCurrentAppTheme query) => _defaultAppTheme;
	}
}