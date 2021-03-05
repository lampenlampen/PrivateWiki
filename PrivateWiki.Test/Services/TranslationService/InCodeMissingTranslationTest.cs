using PrivateWiki.Core.Logging;
using PrivateWiki.Dependencies.Components.NLog;
using PrivateWiki.Services.TranslationService;
using Xunit;

namespace PrivateWiki.Test.Services.TranslationService
{
	public class InCodeMissingTranslationTest
	{
		[Fact]
		public void CheckForMissingTranslations()
		{
			var translations = new InCodeTranslationManager(new LogCmdHandler(new NLogAdapter()));
			var res = translations.Resources;
		}
	}
}