using System.Collections.Generic;
using System.Globalization;

namespace PrivateWiki.Core.ApplicationLanguage
{
	public class SupportedCulturesQueryHandler : IQueryHandler<GetSupportedCultures, SupportedCultures>
	{
		private readonly SupportedCultures _supportedAppCultures = new(new List<CultureInfo>
		{
			new("en"),
			new("de-DE"),
			new("fr-FR")
		});

		public SupportedCultures Handle(GetSupportedCultures query) => _supportedAppCultures;
	}
}