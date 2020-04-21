using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Models.ViewModels;
using PrivateWiki.Models.ViewModels.PageEditors;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Controls.PageEditors
{
	public class MarkdownPageEditorControlBase : ReactiveUserControl<MarkdownPageEditorControlViewModel> {}
	
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MarkdownPageEditorControl : MarkdownPageEditorControlBase
	{
		public MarkdownPageEditorControl()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				commandBar.ViewModel = (PageEditorCommandBarViewModel) ViewModel.CommandBarViewModel;
				
				this.Bind(ViewModel,
					vm => vm.Content,
					view => view.PageEditorTextBox.Text);
			});
		}
	}
}