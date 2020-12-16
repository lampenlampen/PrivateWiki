namespace PrivateWiki.Services.TranslationService
{
	public abstract class TranslationResources
	{
		public string TestTranslationString => GetStringResource("test");

		public abstract string GetStringResource(string key);
	}
}