using System.Collections.Generic;
using System.Globalization;

namespace PrivateWiki.Services.TranslationService.InCodeTranslations
{
	public class DefaultTranslation : IInCodeTranslationResources
	{
		public void LoadResources(IDictionary<CultureInfo, Dictionary<string, string>> res)
		{
			var culture = CultureInfo.InvariantCulture;
			var dict = new Dictionary<string, string>
			{
				["test"] = "Lorem Ipsum",
				["invariant_only_test"] = "Invariant Only Lorem Ipsum",
				["table_of_contents"] = "Table of Contents",
				["labels"] = "Labels",
				["edit"] = "Edit"
			};

			res[culture] = dict;
		}
	}
}