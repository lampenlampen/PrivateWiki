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
using PrivateWiki.UWP.Models;
using PrivateWiki.UWP.Settings;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Pages.SettingsPagesOld
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class DeveloperSettingsPage : Page, INotifyPropertyChanged
	{
		private DeveloperSettingsModel Model { get; set; }

		private readonly TranslationResources _translationResources;

		private readonly IObservable<CultureChangedEventArgs> _cultureChangedEvent;
		private readonly IObserver<CultureChangedEventArgs> _cultureChangedObserver;

		private readonly DevSettingsPageStrings _translations;

		public string LanguageTextLoc { get; private set; }


		public DeveloperSettingsPage()
		{
			this.InitializeComponent();
			ApplicationData.Current.DataChanged += RoamingDataChanged;

			if (Model == null)
			{
				LoadModel();
			}

			var container = Application.Instance.Container;

			_cultureChangedEvent = container.GetInstance<IObservable<CultureChangedEventArgs>>();
			_cultureChangedObserver = container.GetInstance<IObserver<CultureChangedEventArgs>>();
			_translationResources = container.GetInstance<TranslationResources>();

			_translations = new DevSettingsPageStrings(_translationResources);

			UpdateUiTest();

			_cultureChangedEvent.Subscribe(x => { UpdateUiTest(); });

			GermanBtn.Events().Click
				.Subscribe(x =>
				{
					var germanCulture = new CultureInfo("de-DE");
					CultureInfo.CurrentCulture = germanCulture;
					CultureInfo.CurrentUICulture = germanCulture;

					_cultureChangedObserver.OnNext(new CultureChangedEventArgs(germanCulture));
				});

			English.Events().Click
				.Subscribe(x =>
				{
					var englishCulture = new CultureInfo("en-EN");
					CultureInfo.CurrentCulture = englishCulture;
					CultureInfo.CurrentUICulture = englishCulture;

					_cultureChangedObserver.OnNext(new CultureChangedEventArgs(englishCulture));
				});
		}

		private void UpdateUiTest()
		{
			LanguageTextLoc = _translationResources.GetStringResource("language");
			OnPropertyChanged(nameof(_translations));
			//this.Bindings.Update();
		}

		private void RoamingDataChanged(ApplicationData sender, object args)
		{
			LoadModel();
		}


		private void SettingsHeader_OnApplyClick(object sender, RoutedEventArgs e)
		{
			var handler = new DeveloperSettingsModelHandler();
			handler.SaveDeveloperSettingsModel(Model);
		}

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e)
		{
			LoadModel();
		}

		private void LoadModel()
		{
			var handler = new DeveloperSettingsModelHandler();
			Model = handler.LoadDeveloperSettingsModel();
			Bindings.Update();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
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

		public string LanguageTextLoc => _resources.GetStringResource("language");
	}
}