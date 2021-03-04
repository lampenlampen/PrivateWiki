using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PrivateWiki.Core;
using PrivateWiki.Core.ApplicationLanguage;
using PrivateWiki.Core.Events;
using PrivateWiki.DataModels;
using PrivateWiki.Services.TranslationService;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public class PersonalizationCtrlVM : ReactiveObject, IActivatableViewModel
	{
		private readonly IQueryHandler<GetSupportedCultures, SupportedCultures> _supportedCulturesQuery;
		private readonly IQueryHandler<GetCurrentAppUICulture, CurrentAppUICulture> _currentAppUiCulture;
		private readonly ICommandHandler<CultureChangedEventArgs> _cultureChangedCmd;
		private readonly ICommandHandler<ThemeChangedEventArgs> _themeChangedCmd;

		public ViewModelActivator Activator { get; }

		private AppTheme _appTheme;

		public AppTheme AppTheme
		{
			get => _appTheme;
			set => this.RaiseAndSetIfChanged(ref _appTheme, value);
		}

		public ReadOnlyCollection<AppLangVm> Languages { get; }

		private AppLangVm _currentAppLang;

		public AppLangVm CurrentAppLangVm
		{
			get => _currentAppLang;
			set => this.RaiseAndSetIfChanged(ref _currentAppLang, value);
		}

		public PersonalizationCtrlResources Resources { get; }

		public PersonalizationCtrlVM(
			TranslationResources resources,
			IQueryHandler<GetSupportedCultures, SupportedCultures> supportedCulturesQuery,
			IQueryHandler<GetCurrentAppUICulture, CurrentAppUICulture> currentAppUiCulture,
			ICommandHandler<CultureChangedEventArgs> cultureChangedEvent,
			ICommandHandler<ThemeChangedEventArgs> themeChangedEvent)
		{
			Activator = new ViewModelActivator();

			_supportedCulturesQuery = supportedCulturesQuery;
			_currentAppUiCulture = currentAppUiCulture;
			_cultureChangedCmd = cultureChangedEvent;
			_themeChangedCmd = themeChangedEvent;
			Resources = new PersonalizationCtrlResources(resources);

			Languages =
				new ReadOnlyCollection<AppLangVm>(
					_supportedCulturesQuery.Handle(
							new GetSupportedCultures())
						.SupportedAppCultures
						.Select(x => new AppLangVm(x)).ToList());

			CurrentAppLangVm = new AppLangVm(_currentAppUiCulture.Handle(new GetCurrentAppUICulture()).CurrentCulture);

			this.WhenAnyValue(x => x.CurrentAppLangVm)
				.WhereNotNull()
				.Do(x =>
				{
					var newCulture = x.UICultureInfo;
					CultureInfo.CurrentUICulture = newCulture;

					_cultureChangedCmd.Handle(new CultureChangedEventArgs(newCulture));
				})
				.Subscribe();

			this.WhenActivated((CompositeDisposable disposable) => { });
		}

		private void OnActivated() { }
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

	public class AppThemeVm
	{
		private readonly TranslationResources _translationResources;

		public AppTheme AppTheme { get; }

		public AppThemeVm(AppTheme appTheme, TranslationResources translationResources)
		{
			_translationResources = translationResources;
			AppTheme = appTheme;
		}

		public string DisplayName => AppTheme.ToString().ToLowerInvariant();
	}
}