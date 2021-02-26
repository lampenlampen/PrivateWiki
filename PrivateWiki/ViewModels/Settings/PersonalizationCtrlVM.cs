using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using PrivateWiki.Core;
using PrivateWiki.Core.ApplicationLanguage;
using PrivateWiki.Core.Events;
using PrivateWiki.Services.TranslationService;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public class PersonalizationCtrlVM : ReactiveObject
	{
		private readonly IQueryHandler<GetSupportedCultures, SupportedCultures> _supportedCulturesQuery;
		private readonly IQueryHandler<GetCurrentAppUICulture, CurrentAppUICulture> _currentAppUiCulture;
		private readonly IObserver<CultureChangedEventArgs> _cultureChangedEvent;

		private AppTheme _appTheme;

		public AppTheme AppTheme
		{
			get => _appTheme;
			set => this.RaiseAndSetIfChanged(ref _appTheme, value);
		}

		public ReadOnlyCollection<AppLangVm> Languages { get; }

		public PersonalizationCtrlResources Resources { get; }

		public ReactiveCommand<AppLangVm, Unit> ChangeCurrentUICulture { get; }

		public PersonalizationCtrlVM(
			TranslationResources resources,
			IQueryHandler<GetSupportedCultures, SupportedCultures> supportedCulturesQuery,
			IQueryHandler<GetCurrentAppUICulture, CurrentAppUICulture> currentAppUiCulture,
			IObserver<CultureChangedEventArgs> cultureChangedEvent)
		{
			_supportedCulturesQuery = supportedCulturesQuery;
			_currentAppUiCulture = currentAppUiCulture;
			_cultureChangedEvent = cultureChangedEvent;
			Resources = new PersonalizationCtrlResources(resources);
			Languages =
				new ReadOnlyCollection<AppLangVm>(
					_supportedCulturesQuery.Handle(
							new GetSupportedCultures())
						.SupportedAppCultures
						.Select(x => new AppLangVm(x)).ToList());

			ChangeCurrentUICulture = ReactiveCommand.Create<AppLangVm>(x => _cultureChangedEvent.OnNext(new CultureChangedEventArgs(x.UICultureInfo)));
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

	public class AppLangVm
	{
		public CultureInfo UICultureInfo { get; }

		public AppLangVm(CultureInfo cultureInfo)
		{
			UICultureInfo = cultureInfo;
		}

		public string Name => UICultureInfo.DisplayName;
	}
}