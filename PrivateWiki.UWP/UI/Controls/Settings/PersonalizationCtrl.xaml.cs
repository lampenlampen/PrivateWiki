using System.Reactive.Disposables;
using System.Reactive.Linq;
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

				this.Bind(ViewModel,
						vm => vm.CurrentAppLangVm,
						view => view.LanguageComboBox.SelectedItem)
					.DisposeWith(disposable);

				// Testing
				this.WhenAnyValue(x => x.ViewModel.CurrentAppLangVm)
					.WhereNotNull()
					.Select(x => x.Name)
					.BindTo(Test, x => x.Text)
					.DisposeWith(disposable);
			});
		}
	}
}