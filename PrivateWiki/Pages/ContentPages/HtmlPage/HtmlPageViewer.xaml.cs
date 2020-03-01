using System;
using System.Collections.Generic;
using System.Drawing;
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
using Contracts;
using Models.Pages;
using Models.ViewModels;
using NLog;
using PrivateWiki.Utilities;
using RavinduL.LocalNotifications.Notifications;
using ReactiveUI;
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;
using TreeViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.Pages.ContentPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class HtmlPageViewer : ContentPage, IViewFor<HtmlPageViewerViewModel>
	{
		#region ViewModel
		
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(ViewModel), typeof(HtmlPageViewerViewModel), typeof(HtmlPageViewer), new PropertyMetadata(null));

		public HtmlPageViewerViewModel ViewModel
		{
			get => (HtmlPageViewerViewModel)GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (HtmlPageViewerViewModel)value;
		}
		
		#endregion

		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public HtmlPageViewer()
		{
			this.InitializeComponent();

			// TODO Remove Test Page
			// Test HtmlPage
			var page = new global::Models.Pages.HtmlPage
			{
				Content = "<h1>Heading 1</h1>"
			};
			
			ViewModel = new HtmlPageViewerViewModel(page);

			ShowPageInWebView();

			this.WhenActivated(disposable =>
			{
				Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
					handler => commandBar.SearchClick += handler,
					handler => commandBar.SearchClick -= handler).Select(x => Unit.Default).InvokeCommand().DisposeWith(disposable);
			});
		}

		private void ShowPageInWebView()
		{
			Webview.NavigateToString(ViewModel.Content);
		}

		public override void Top_Click(object sender, RoutedEventArgs e)
		{
			// Testing
			// throw new NotImplementedException();
			
			Frame.Navigate(typeof(GenericTextPageEditor), "start");
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
