using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using JetBrains.Annotations;
using PrivateWiki.Core;
using PrivateWiki.Core.Events;
using PrivateWiki.DataModels;
using PrivateWiki.Services.TranslationService;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.UI.Pages.SettingsPagesOld
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class DeveloperSettingsPage : Page, INotifyPropertyChanged
	{
		private readonly TranslationManager _translationManager;

		private readonly IObservable<CultureChangedEventArgs> _cultureChangedEvent;
		private readonly ICommandHandler<CultureChangedEventArgs> _cultureChangedObserver;
		private readonly ICommandHandler<ThemeChangedEventArgs> _themeChangedObserver;

		private readonly DevSettingsPageStrings _translations;

		public DeveloperSettingsPage()
		{
			this.InitializeComponent();

			var container = ServiceLocator.Container;

			_cultureChangedEvent = container.GetInstance<IObservable<CultureChangedEventArgs>>();
			_cultureChangedObserver = container.GetInstance<ICommandHandler<CultureChangedEventArgs>>();
			_themeChangedObserver = container.GetInstance<ICommandHandler<ThemeChangedEventArgs>>();
			_translationManager = container.GetInstance<TranslationManager>();

			_translations = new DevSettingsPageStrings(_translationManager);

			UpdateUiTest();

			_cultureChangedEvent.Subscribe(_ => { UpdateUiTest(); });

			GermanBtn.Events().Click
				.Subscribe(_ =>
				{
					var germanCulture = new CultureInfo("de-DE");
					CultureInfo.CurrentCulture = germanCulture;
					CultureInfo.CurrentUICulture = germanCulture;

					_cultureChangedObserver.Handle(new CultureChangedEventArgs(germanCulture));
				});

			English.Events().Click
				.Subscribe(_ =>
				{
					var englishCulture = new CultureInfo("en-EN");
					CultureInfo.CurrentCulture = englishCulture;
					CultureInfo.CurrentUICulture = englishCulture;

					_cultureChangedObserver.Handle(new CultureChangedEventArgs(englishCulture));
				});

			LightThemeBtn.Events().Click
				.Subscribe(_ => { _themeChangedObserver.Handle(new ThemeChangedEventArgs(AppTheme.DefaultLightModeTheme)); });

			DarkThemeBtn.Events().Click
				.Subscribe(_ => { _themeChangedObserver.Handle(new ThemeChangedEventArgs(AppTheme.DefaultDarkModeTheme)); });
		}

		private void UpdateUiTest()
		{
			OnPropertyChanged(nameof(_translations));
			//this.Bindings.Update();
		}


		private void SettingsHeader_OnApplyClick(object sender, RoutedEventArgs e) { }

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e) { }

		public event PropertyChangedEventHandler? PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class DevSettingsPageStrings
	{
		private readonly TranslationManager _manager;

		public DevSettingsPageStrings(TranslationManager manager)
		{
			_manager = manager;
		}

		public string Language => _manager.GetStringResource("language");
	}
}