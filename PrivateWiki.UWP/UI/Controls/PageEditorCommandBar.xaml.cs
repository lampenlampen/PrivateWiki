using System.Reactive.Disposables;
using PrivateWiki.ViewModels;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls
{
	public class PageEditorCommandBarBase : ReactiveUserControl<PageEditorCommandBarViewModel>
	{
	}

	public sealed partial class PageEditorCommandBar : PageEditorCommandBarBase
	{
		public PageEditorCommandBar()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				this.BindCommand(ViewModel,
						vm => vm.Save,
						view => view.SaveBtn)
					.DisposeWith(disposable);

				this.BindCommand(ViewModel,
						vm => vm.Delete,
						view => view.DeleteBtn)
					.DisposeWith(disposable);

				this.BindCommand(ViewModel,
						vm => vm.OpenInExternalEditor,
						view => view.OpenExternalEditorBtn)
					.DisposeWith(disposable);

				this.BindCommand(ViewModel,
						vm => vm.Abort,
						view => view.BackBtn)
					.DisposeWith(disposable);
			});
		}
	}
}