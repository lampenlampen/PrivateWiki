using System;
using System.Collections.Generic;
using System.Linq;
using PrivateWiki.Core.Events;
using PrivateWiki.DataModels;
using PrivateWiki.Services.TranslationService;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public class PersonalizationCtrlVM : ReactiveObject
	{
		private AppTheme _appTheme;

		public AppTheme AppTheme
		{
			get => _appTheme;
			set => this.RaiseAndSetIfChanged(ref _appTheme, value);
		}

		private List<string> _languages = Enum.GetValues(typeof(Languages)).Cast<Languages>().Select(x => x.ToString()).ToList();

		public List<string> Languages
		{
			get => _languages;
			set => this.RaiseAndSetIfChanged(ref _languages, value);
		}

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