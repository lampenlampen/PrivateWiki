using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;
using TreeViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.Pages.ContentPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class HtmlPageViewer : ContentPage
	{
		IObservable<RoutedEventArgs> a;
		public HtmlPageViewer()
		{
			this.InitializeComponent();
			a = Observable.FromEvent<RoutedEventHandler, RoutedEventArgs>(
				handler => commandBar.TopClick += handler,
				handler => commandBar.TopClick -= handler);
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

		private void WebView_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
		{
			throw new NotImplementedException();
		}

		private void Webview_OnContainsFullScreenElementChanged(WebView sender, object args)
		{
			throw new NotImplementedException();
		}

		private void Webview_OnScriptNotify(object sender, NotifyEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Webview_OnLoadCompleted(object sender, NavigationEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
