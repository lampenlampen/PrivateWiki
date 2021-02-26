using System.Collections.Generic;
using System.Globalization;

namespace PrivateWiki.Services.TranslationService.InCodeTranslationsFiles
{
	public class EnglishTranslation : IInCodeTranslationResources
	{
		public void LoadResources(IDictionary<CultureInfo, Dictionary<string, string>> res)
		{
			var dict = new Dictionary<string, string>
			{
				["test"] = "Lorem Ipsum",
				["invariant_only_test"] = "Invariant Only Lorem Ipsum",
				["table_of_contents"] = "Table of Contents",
				["labels"] = "Labels",
				["edit"] = "Edit",
				["export"] = "Export",
				["favorite"] = "Favorite",
				["fullscreen"] = "Fullscreen",
				["history"] = "History",
				["import"] = "Import",
				["mediaManager"] = "Media Manager",
				["pdf"] = "PDF",
				["search"] = "Search",
				["settings"] = "Settings",
				["siteManager"] = "Site Manager",
				["tags"] = "Tags",
				["toc"] = "Table of Contents",
				["toggle"] = "Toggle",
				["toTop"] = "To Top",
				["close"] = "Close",
				["developerSettings"] = "Developer Settings",
				["newPage"] = "New Page",
				["site"] = "Site",
				["general"] = "General",
				["navigation"] = "Navigation",
				["pages"] = "Pages",
				["labels"] = "Labels",
				["assets"] = "Assets",
				["theme"] = "Theme",
				["modules"] = "Modules",
				["rendering"] = "Rendering",
				["storage"] = "Storage",
				["sync"] = "Synchronization",
				["system"] = "System",
				["developerTools"] = "Developer Tools",
				["language"] = "Language",
				["personalization"] = "Personalization",
				["personalization_subheader"] = "Customize this app.",

				// Import Page Dialog
				["importPageDialogDescription"] = "If a page with the same id exists already, it will be overriden by the imported one!",
				["importPageDialogTitle"] = "Import Page",

				// Print PDF Dialog
				["printPDFDialogDescription"] =
					"This App currently does not directly support pdf printing functionality.\r\nIt is possible to open this Page in your Browser, where you can print the page to PDF.",
				["printPDFDialogOpenInBrowser"] = "Open In Browser",
				["printPDFDialogTitle"] = "Print Dialog"
			};

			res[CultureInfo.InvariantCulture] = dict;
			res[new CultureInfo("en")] = dict;
		}
	}
}