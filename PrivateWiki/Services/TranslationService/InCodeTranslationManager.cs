using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using PrivateWiki.Core;
using PrivateWiki.Services.Logger;
using PrivateWiki.Services.TranslationService.InCodeTranslationsFiles;

namespace PrivateWiki.Services.TranslationService
{
	public class InCodeTranslationManager : TranslationManager
	{
		private readonly ICommandHandler<LogEntry> _logger;

		private readonly Dictionary<CultureInfo, Dictionary<string, string>> _resources = new();

		public IReadOnlyDictionary<CultureInfo, ReadOnlyDictionary<string, string>> Resources
		{
			get
			{
				var dict = new Dictionary<CultureInfo, ReadOnlyDictionary<string, string>>();

				foreach (var resourcesKey in _resources.Keys)
				{
					dict[resourcesKey] = new ReadOnlyDictionary<string, string>(_resources[resourcesKey]);
				}

				return new ReadOnlyDictionary<CultureInfo, ReadOnlyDictionary<string, string>>(dict);
			}
		}

		public InCodeTranslationManager(ICommandHandler<LogEntry> logger)
		{
			_logger = logger;

			new EnglishTranslation().LoadResources(_resources);
			new GermanTranslation().LoadResources(_resources);
			new FrenchTranslation().LoadResources(_resources);
		}

		public override string GetStringResource(string key)
		{
			var cultureInfo = CultureInfo.CurrentCulture;

			var cultureCache = GetCultureCache(cultureInfo);

			cultureCache.TryGetValue(key, out string resource);

			while (resource is null && !Equals(cultureInfo, CultureInfo.InvariantCulture))
			{
				_logger.Handle(new LogEntry(LoggingEventType.Information, $"Translation missing: Key{key}; Culture={cultureInfo.Name}"));

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
				_logger.Handle(new LogEntry(LoggingEventType.Information, $"Culture missing: Culture={culture.Name}"));

				culture = culture.Parent;
			}

			return cultureCache;
		}
	}
}