using System.Collections.Generic;

namespace PrivateWiki.Core.AppTheme
{
	public class AppThemes
	{
		public IEnumerable<DataModels.AppTheme> SupportedAppThemes { get; }

		public AppThemes(IEnumerable<DataModels.AppTheme> supportedAppThemes)
		{
			SupportedAppThemes = supportedAppThemes;
		}
	}
}