using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using PrivateWiki.Models.Pages;
using PrivateWiki.Models.ViewModels;
using ReactiveUI;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PageEditor : Page, IViewFor<PageEditorViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(PageEditorViewModel), typeof(PageEditor), new PropertyMetadata(null));

		public PageEditorViewModel ViewModel
		{
			get => (PageEditorViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (PageEditorViewModel) value;
		}

		#endregion

		public PageEditor()
		{
			InitializeComponent();
			ViewModel = new PageEditorViewModel();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			var path = Path.ofLink((string) e.Parameter);
			ViewModel.ShowPage.Execute(path);
		}
	}
}