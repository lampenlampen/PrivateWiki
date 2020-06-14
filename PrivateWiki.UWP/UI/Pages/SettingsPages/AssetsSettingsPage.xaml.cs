using System;
using System.Reactive.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using PrivateWiki.DataModels;
using PrivateWiki.Services.FileExplorerService;
using PrivateWiki.UWP.UI.Events;
using PrivateWiki.ViewModels.Settings;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Pages.SettingsPages
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
					.Subscribe(async _ =>
					{
						var fileExplorerService = App.Current.Application.Container.GetInstance<IFileExplorerService>();
						fileExplorerService.ShowFolderAsync(new Folder((await App.Current.Config.GetDataFolderAsync()).Path));
						//Windows.System.Launcher.LaunchFolderAsync(await App.Current.Config.GetDataFolderAsync());
					})
					.DisposeWith(disposable);
			});
		}
	}
}