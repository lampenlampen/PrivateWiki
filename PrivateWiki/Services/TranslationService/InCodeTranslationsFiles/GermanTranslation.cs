using System.Collections.Generic;
using System.Globalization;

namespace PrivateWiki.Services.TranslationService.InCodeTranslationsFiles
{
	public class GermanTranslation : IInCodeTranslationResources
	{
		public void LoadResources(IDictionary<CultureInfo, Dictionary<string, string>> res)
		{
			var culture = new CultureInfo("de-DE");
			var dict = new Dictionary<string, string>
			{
				["test"] = "German Lorem Ipsum",
				["table_of_contents"] = "Inhaltsverzeichnis",
				["labels"] = "Labels",
				["edit"] = "Bearbeiten",
				["export"] = "Export",
				["favorite"] = "Favoriten",
				["fullscreen"] = "Vollbild",
				["history"] = "Historie",
				["import"] = "Import",
				["mediaManager"] = "Medien Manager",
				["pdf"] = "PDF",
				["search"] = "Suchen",
				["settings"] = "Einstellungen",
				["siteManager"] = "Seiten Manager",
				["tags"] = "Tags",
				["toc"] = "Inhaltsverzeichnis",
				["toggle"] = "Toggle",
				["toTop"] = "To Top",
				["close"] = "Schließen",
				["newPage"] = "Neue Seite",
				["language"] = "Sprache",

				// Import Page Dialog
				["importPageDialogDescription"] = "Eine vorhandene Seite mit gleicher Id wird dabei überschrieben.",
				["importPageDialogTitle"] = "Seite importieren",

				// Print PDF Dialog
				["printPDFDialogDescription"] = "Diese App unterstützt zurzeit keinen PDF-Druck. Sie können aber die Seite im Browser öffnen und dessen Druck Funktion nutzen.",
				["printPDFDialogOpenInBrowser"] = "Im Browser öffnen",
				["printPDFDialogTitle"] = "Als PDF speichern"
			};

			res[culture] = dict;
		}
	}
}