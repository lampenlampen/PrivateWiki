using System.Collections.Generic;

namespace PrivateWiki.Core.AppTheme
{
	public class AppThemesQueryHandler : IQueryHandler<GetAppThemes, AppThemes>
	{
		private readonly AppThemes _appThemes = new(new List<DataModels.AppTheme>
		{
			DataModels.AppTheme.DefaultLightModeTheme,
			DataModels.AppTheme.DefaultDarkModeTheme
		});

		public AppThemes Handle(GetAppThemes query) => _appThemes;
	}
}