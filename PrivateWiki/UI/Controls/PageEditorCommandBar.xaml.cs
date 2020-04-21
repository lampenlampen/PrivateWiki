using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PrivateWiki.Models.ViewModels;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UI.Controls
{
	public class PageEditorCommandBarBase : ReactiveUserControl<PageEditorCommandBarViewModel> {}
	
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
