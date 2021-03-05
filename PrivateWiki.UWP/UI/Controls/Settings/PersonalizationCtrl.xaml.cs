using System.Reactive.Disposables;
using PrivateWiki.ViewModels.Settings;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls.Settings
{
	public class PersonalizationCtrlBase : ReactiveUserControl<PersonalizationCtrlVM> { }

	public sealed partial class PersonalizationCtrl : PersonalizationCtrlBase
	{
		public PersonalizationCtrl()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				LanguageComboBox.ItemsSource = ViewModel.Languages;
				ThemeComboBox.ItemsSource = ViewModel.AppThemes;

				this.Bind(ViewModel,
						vm => vm.CurrentAppLangVm,
						view => view.LanguageComboBox.SelectedItem)
					.DisposeWith(disposable);

				this.Bind(ViewModel,
						vm => vm.SelectedAppTheme,
						view => view.ThemeComboBox.SelectedItem)
					.DisposeWith(disposable);
			});
		}
	}
}