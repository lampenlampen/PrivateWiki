using System.Collections.Generic;
using System.Globalization;

namespace PrivateWiki.Services.TranslationService
{
	public interface IInCodeTranslationResources
	{
		public void LoadResources(IDictionary<CultureInfo, Dictionary<string, string>> dict);
	}
}