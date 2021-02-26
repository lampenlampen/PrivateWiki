using System.Collections.Generic;
using System.Globalization;

namespace PrivateWiki.Core.ApplicationLanguage
{
	public class SupportedCultures
	{
		public IEnumerable<CultureInfo> SupportedAppCultures { get; }

		public SupportedCultures(IEnumerable<CultureInfo> supportedAppCultures)
		{
			SupportedAppCultures = supportedAppCultures;
		}
	}
}