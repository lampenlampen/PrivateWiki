using System.Globalization;

namespace PrivateWiki.Core.ApplicationLanguage
{
	public class CurrentAppUICulture
	{
		public CultureInfo CurrentCulture { get; }

		public CurrentAppUICulture(CultureInfo currentCulture)
		{
			CurrentCulture = currentCulture;
		}
	}
}