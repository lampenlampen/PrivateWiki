using System;
using System.Reactive.Disposables;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using PrivateWiki.Models.ViewModels.Settings;
using PrivateWiki.UI.Events;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Pages.SettingsPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class AssetsSettingsPage : IViewFor<AssetsSettingsPageViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(ViewModel), typeof(AssetsSettingsPageViewModel), typeof(AssetsSettingsPage), new PropertyMetadata(null));

		public AssetsSettingsPageViewModel ViewModel
		{
			get => (AssetsSettingsPageViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (AssetsSettingsPageViewModel) value;
		}

		#endregion

		public AssetsSettingsPage()
		{
			this.InitializeComponent();
			ViewModel = new AssetsSettingsPageViewModel();

			this.WhenActivated(disposable =>
			{
				TreeViewMedia.Events().ItemInvoked
					.Subscribe()
					.DisposeWith(disposable);

				OpenInExplorerText.Events().Click
					.Subscribe(async _ => { Launcher.LaunchFolderAsync(await App.Current.Config.GetDataFolderAsync()); })
					.DisposeWith(disposable);
			});
		}
	}
}