using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.ViewModels.Settings;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Pages.SettingsPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LabelsSettingsPage : Page, IViewFor<LabelsSettingsPageViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(ViewModel), typeof(LabelsSettingsPageViewModel), typeof(LabelsSettingsPage), new PropertyMetadata(null));

		public LabelsSettingsPageViewModel ViewModel
		{
			get => (LabelsSettingsPageViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (LabelsSettingsPageViewModel) value;
		}

		#endregion

		public LabelsSettingsPage()
		{
			this.InitializeComponent();

			ViewModel = new LabelsSettingsPageViewModel();

			this.WhenActivated(disposable => { });
		}
	}
}