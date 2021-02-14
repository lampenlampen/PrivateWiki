using System.Collections.Generic;
using System.Globalization;

namespace PrivateWiki.Services.TranslationService.InCodeTranslations
{
	public class GermanTranslation : IInCodeTranslationResources
	{
		public void LoadResources(IDictionary<CultureInfo, Dictionary<string, string>> res)
		{
			var culture = new CultureInfo("de-de");
			var dict = new Dictionary<string, string>
			{
				["test"] = "German Lorem Ipsum",
				["table_of_contents"] = "Inhaltsverzeichnis",
				["labels"] = "Labels",
				["edit"] = "Bearbeiten"
			};

			res[culture] = dict;
		}
	}
}