using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using PrivateWiki.Core;
using PrivateWiki.Core.ApplicationLanguage;
using PrivateWiki.Core.AppTheme;
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

		private readonly IQueryHandler<GetCurrentAppTheme, AppTheme> _currentAppThemeQueryHandler;
		private readonly IQueryHandler<GetAppThemes, AppThemes> _appThemesQueryHandler;


		public ViewModelActivator Activator { get; }

		private AppTheme _appTheme;

		public AppTheme AppTheme
		{
			get => _appTheme;
			set => this.RaiseAndSetIfChanged(ref _appTheme, value);
		}

		private readonly ISourceCache<AppTheme, string> _appThemes = new SourceCache<AppTheme, string>(x => x.Name);

		public readonly ReadOnlyObservableCollection<AppTheme> AppThemes;

		public ReadOnlyCollection<AppLangVm> Languages { get; }

		private AppLangVm _currentAppLang;

		public AppLangVm CurrentAppLangVm
		{
			get => _currentAppLang;
			set => this.RaiseAndSetIfChanged(ref _currentAppLang, value);
		}

		private AppTheme _selectedAppTheme;

		public AppTheme SelectedAppTheme
		{
			get => _selectedAppTheme;
			set => this.RaiseAndSetIfChanged(ref _selectedAppTheme, value);
		}

		public PersonalizationCtrlResources Resources { get; }

		public PersonalizationCtrlVM(
			TranslationManager manager,
			IQueryHandler<GetSupportedCultures, SupportedCultures> supportedCulturesQuery,
			IQueryHandler<GetCurrentAppUICulture, CurrentAppUICulture> currentAppUiCulture,
			ICommandHandler<CultureChangedEventArgs> cultureChangedEvent,
			ICommandHandler<ThemeChangedEventArgs> themeChangedEvent,
			IQueryHandler<GetCurrentAppTheme, AppTheme> currentAppThemeQueryHandler,
			IQueryHandler<GetAppThemes, AppThemes> appThemesQueryHandler)
		{
			Activator = new ViewModelActivator();

			_supportedCulturesQuery = supportedCulturesQuery;
			_currentAppUiCulture = currentAppUiCulture;
			_cultureChangedCmd = cultureChangedEvent;
			_themeChangedCmd = themeChangedEvent;
			_currentAppThemeQueryHandler = currentAppThemeQueryHandler;
			_appThemesQueryHandler = appThemesQueryHandler;
			Resources = new PersonalizationCtrlResources(manager);

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

			this.WhenAnyValue(x => x.SelectedAppTheme)
				.WhereNotNull()
				.Subscribe(x => _themeChangedCmd.Handle(new ThemeChangedEventArgs(x)));

			_appThemes.Connect()
				.Bind(out AppThemes)
				.Subscribe();

			this.WhenActivated(OnActivated);
		}

		private void OnActivated(CompositeDisposable disposable)
		{
			var themes = _appThemesQueryHandler.Handle(new GetAppThemes()).SupportedAppThemes;
			_appThemes.Edit(updater => updater.AddOrUpdate(themes));

			_appTheme = _currentAppThemeQueryHandler.Handle(new GetCurrentAppTheme());
			this.RaisePropertyChanged(nameof(_appTheme));
		}
	}

	public class PersonalizationCtrlResources
	{
		private readonly TranslationManager _manager;

		public PersonalizationCtrlResources(TranslationManager manager)
		{
			_manager = manager;
		}

		public string Personalization => _manager.GetStringResource("personalization");
		public string PersonalizationSubHeader => _manager.GetStringResource("personalization_subheader");
		public string Language => _manager.GetStringResource("language");
		public string Theme => _manager.GetStringResource("theme");
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
		private readonly TranslationManager _translationManager;

		public AppTheme AppTheme { get; }

		public AppThemeVm(AppTheme appTheme, TranslationManager translationManager)
		{
			_translationManager = translationManager;
			AppTheme = appTheme;
		}

		public string DisplayName => AppTheme.ToString().ToLowerInvariant();
	}
}