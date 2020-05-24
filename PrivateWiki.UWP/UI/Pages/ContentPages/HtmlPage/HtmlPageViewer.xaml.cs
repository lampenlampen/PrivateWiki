using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NLog;
using PrivateWiki.Models.ViewModels;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Pages.ContentPages.HtmlPage
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class HtmlPageViewer : IViewFor<HtmlPageViewerViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(ViewModel), typeof(HtmlPageViewerViewModel), typeof(HtmlPageViewer), new PropertyMetadata(null));

		public HtmlPageViewerViewModel ViewModel
		{
			get => (HtmlPageViewerViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (HtmlPageViewerViewModel) value;
		}

		#endregion

		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public HtmlPageViewer()
		{
			this.InitializeComponent();

			// TODO Remove Test Page
			// Test HtmlPage
			var page = new global::PrivateWiki.Models.Pages.HtmlPage
			{
				Content = "<h1>Heading 1</h1>"
			};

			ViewModel = new HtmlPageViewerViewModel(page);

			ShowPageInWebView();

			this.WhenActivated(disposable =>
			{
				Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
						handler => commandBar.SearchClick += handler,
						handler => commandBar.SearchClick -= handler)
					.Select(x => Unit.Default)
					.Subscribe((_) => { })
					.DisposeWith(disposable);

				commandBar.ShowSettings
					.Subscribe((_) => { Frame.Navigate(typeof(SettingsPage)); })
					.DisposeWith(disposable);
			});
		}

		private void ShowPageInWebView()
		{
			Webview.NavigateToString(ViewModel.Content);
		}

		public override void Top_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		public override void Search_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void TreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
		{
			throw new NotImplementedException();
		}
	}
}