using System.Globalization;

namespace PrivateWiki.Core.ApplicationLanguage
{
	public class CurrentAppUICultureQueryHandler : IQueryHandler<GetCurrentAppUICulture, CurrentAppUICulture>
	{
		private readonly CurrentAppUICulture _currentAppUiCulture = new(CultureInfo.CurrentUICulture);

		public CurrentAppUICulture Handle(GetCurrentAppUICulture query) => _currentAppUiCulture;
	}
}