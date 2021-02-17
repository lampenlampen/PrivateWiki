using System.Collections.Generic;
using System.Globalization;

namespace PrivateWiki.Services.TranslationService.InCodeTranslationsFiles
{
	public class FrenchTranslation : IInCodeTranslationResources
	{
		public void LoadResources(IDictionary<CultureInfo, Dictionary<string, string>> res)
		{
			var culture = new CultureInfo("fr-FR");
			var dict = new Dictionary<string, string>
			{
				["test"] = "French Lorem Ipsum",
				["printPDFDialogDescription"] = "Pour le moment, cette application ne prend pas directement en charge la fonctionnalité d'impression PDF. ",
				["printPDFDialogTitle"] = "Dialogue d'impression"
			};

			res[culture] = dict;
		}
	}
}