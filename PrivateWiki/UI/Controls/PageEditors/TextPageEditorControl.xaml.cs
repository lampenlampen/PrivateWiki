using Windows.UI.Xaml.Controls;
using PrivateWiki.Models.ViewModels;
using PrivateWiki.Models.ViewModels.PageEditors;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Controls.PageEditors
{
	public class TextPageEditorControlBase : ReactiveUserControl<TextPageEditorControlViewModel> {}
	
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class TextPageEditorControl : TextPageEditorControlBase
	{
		public TextPageEditorControl()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				commandBar.ViewModel = (PageEditorCommandBarViewModel) ViewModel.CommandBarViewModel;
			});
		}
	}
}