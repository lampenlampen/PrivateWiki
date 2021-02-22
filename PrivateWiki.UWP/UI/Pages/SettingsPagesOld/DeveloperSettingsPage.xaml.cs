using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using JetBrains.Annotations;
using PrivateWiki.Core.Events;
using PrivateWiki.Services.TranslationService;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Pages.SettingsPagesOld
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class DeveloperSettingsPage : Page, INotifyPropertyChanged
	{
		private readonly TranslationResources _translationResources;

		private readonly IObservable<CultureChangedEventArgs> _cultureChangedEvent;
		private readonly IObserver<CultureChangedEventArgs> _cultureChangedObserver;
		private readonly IObserver<ThemeChangedEventArgs> _themeChangedObserver;

		private readonly DevSettingsPageStrings _translations;

		public DeveloperSettingsPage()
		{
			this.InitializeComponent();
			ApplicationData.Current.DataChanged += RoamingDataChanged;

			var container = Application.Instance.Container;

			_cultureChangedEvent = container.GetInstance<IObservable<CultureChangedEventArgs>>();
			_cultureChangedObserver = container.GetInstance<IObserver<CultureChangedEventArgs>>();
			_themeChangedObserver = container.GetInstance<IObserver<ThemeChangedEventArgs>>();
			_translationResources = container.GetInstance<TranslationResources>();

			_translations = new DevSettingsPageStrings(_translationResources);

			UpdateUiTest();

			_cultureChangedEvent.Subscribe(_ => { UpdateUiTest(); });

			GermanBtn.Events().Click
				.Subscribe(_ =>
				{
					var germanCulture = new CultureInfo("de-DE");
					CultureInfo.CurrentCulture = germanCulture;
					CultureInfo.CurrentUICulture = germanCulture;

					_cultureChangedObserver.OnNext(new CultureChangedEventArgs(germanCulture));
				});

			English.Events().Click
				.Subscribe(_ =>
				{
					var englishCulture = new CultureInfo("en-EN");
					CultureInfo.CurrentCulture = englishCulture;
					CultureInfo.CurrentUICulture = englishCulture;

					_cultureChangedObserver.OnNext(new CultureChangedEventArgs(englishCulture));
				});

			LightThemeBtn.Events().Click
				.Subscribe(_ => { _themeChangedObserver.OnNext(new ThemeChangedEventArgs(AppTheme.Light)); });

			DarkThemeBtn.Events().Click
				.Subscribe(_ => { _themeChangedObserver.OnNext(new ThemeChangedEventArgs(AppTheme.Dark)); });
		}

		private void UpdateUiTest()
		{
			OnPropertyChanged(nameof(_translations));
			//this.Bindings.Update();
		}

		private void RoamingDataChanged(ApplicationData sender, object args) { }


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
		private readonly TranslationResources _resources;

		public DevSettingsPageStrings(TranslationResources resources)
		{
			_resources = resources;
		}

		public string Language => _resources.GetStringResource("language");
	}
}