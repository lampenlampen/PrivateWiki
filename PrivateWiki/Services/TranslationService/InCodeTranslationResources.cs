using System.Collections.Generic;
using System.Globalization;
using NLog;

namespace PrivateWiki.Services.TranslationService
{
	public class InCodeTranslationResources : TranslationResources
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Dictionary<CultureInfo, Dictionary<string, string>> _resources = new Dictionary<CultureInfo, Dictionary<string, string>>();

		public InCodeTranslationResources()
		{
			AddDefaultTranslations(new Dictionary<string, string>());
			AddGermanTranslation(new Dictionary<string, string>());
		}

		public override string GetStringResource(string key)
		{
			var cultureInfo = CultureInfo.CurrentCulture;

			var cultureCache = GetCultureCache(cultureInfo);

			string resource;

			cultureCache.TryGetValue(key, out resource);

			while (resource is null && !Equals(cultureInfo, CultureInfo.InvariantCulture))
			{
				Logger.Info($"Translation missing: Key{key}; Culture={cultureInfo.Name}");

				cultureInfo = cultureInfo.Parent;

				cultureCache = GetCultureCache(cultureInfo);
				resource = cultureCache[key];
			}

			if (resource is null) throw new KeyNotFoundException();

			return resource;
		}

		private Dictionary<string, string> GetCultureCache(CultureInfo culture)
		{
			Dictionary<string, string>? cultureCache = null;

			while (!_resources.TryGetValue(culture, out cultureCache))
			{
				Logger.Info($"Culture missing: Culture={culture.Name}");

				culture = culture.Parent;
			}

			return cultureCache;
		}

		private void AddDefaultTranslations(Dictionary<string, string> dict)
		{
			var culture = CultureInfo.InvariantCulture;

			dict["test"] = "Lorem Ipsum";
			dict["invariant_only_test"] = "Invariant Only Lorem Ipsum";
			dict["table_of_contents"] = "Table of Contents";
			dict["labels"] = "Labels";
			dict["edit"] = "Edit";

			_resources[culture] = dict;
		}

		private void AddGermanTranslation(Dictionary<string, string> dict)
		{
			var culture = new CultureInfo("de-de");

			dict["test"] = "German Lorem Ipsum";
			dict["table_of_contents"] = "Inhaltsverzeichnis";
			dict["labels"] = "Labels";
			dict["edit"] = "Bearbeiten";

			_resources[culture] = dict;
		}
	}
}