using PrivateWiki.Services.TranslationService;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public class PersonalizationCtrlVM : ReactiveObject
	{
		public PersonalizationCtrlResources Resources { get; }

		public PersonalizationCtrlVM(TranslationResources resources)
		{
			Resources = new PersonalizationCtrlResources(resources);
		}
	}

	public class PersonalizationCtrlResources
	{
		private readonly TranslationResources _resources;

		public PersonalizationCtrlResources(TranslationResources resources)
		{
			_resources = resources;
		}

		public string Personalization => _resources.GetStringResource("personalization");
		public string PersonalizationSubHeader => _resources.GetStringResource("personalization_subheader");
		public string Language => _resources.GetStringResource("language");
		public string Theme => _resources.GetStringResource("theme");
	}
}