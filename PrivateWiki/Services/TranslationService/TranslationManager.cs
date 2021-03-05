namespace PrivateWiki.Services.TranslationService
{
	public abstract class TranslationManager
	{
		public string TestTranslationString => GetStringResource("test");

		public abstract string GetStringResource(string key);
	}
}