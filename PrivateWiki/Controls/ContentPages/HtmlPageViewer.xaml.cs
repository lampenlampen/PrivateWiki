using System;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Windows.UI.Xaml.Controls;
using NLog;
using PrivateWiki.Models.ViewModels;
using ReactiveUI;
using Path = Models.Pages.Path;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.Controls.ContentPages
{
	public class HtmlPageViewerBase : ReactiveUserControl<HtmlPageViewerViewModel>
	{
	}

	public sealed partial class HtmlPageViewer : HtmlPageViewerBase
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		
		private ISubject<string> _navigateToPage;

		public IObservable<string> NavigateToPage => _navigateToPage;

		public HtmlPageViewer()
		{
			this.InitializeComponent();

			_navigateToPage = new Subject<string>();

			this.WhenActivated(disposable =>
			{
				ViewModel.Content.Subscribe(x => { Webview.NavigateToString(x); })
					.DisposeWith(disposable);

				ViewModel.RenderContent.Execute(Unit.Default)
					.Subscribe(x => Webview.NavigateToString(x))
					.DisposeWith(disposable);

				Webview.Events().NavigationStarting
					.Select(a => a.args)
					.Subscribe(WebViewNavigationStarting)
					.DisposeWith(disposable);
			});
		}

		private void WebViewNavigationStarting(WebViewNavigationStartingEventArgs args)
		{
			var uri = args.Uri;

			if (uri == null)
			{
				// initial page loading from `NavigateToString`-method.

				return;
			}

			if (uri.AbsoluteUri.StartsWith("about::"))
			{
				// Wikilink
				var link = uri.ToString().Substring(7);
				
				Logger.Info("Wikilink clicked.");
				Logger.ConditionalDebug($"Wikilink clicked: {link}");

				ViewModel.WikilinkClicked.Execute(Path.ofLink(link));

				args.Cancel = true;
				return;
			}

			if (uri.AbsoluteUri.Contains("about:"))
			{
				// Local link
				Logger.Info("Local link clicked.");

				return;
			}
			
			// Normal Link
			Logger.Info("Normal link clicked.");
			Logger.ConditionalDebug($"Normal link clicked: {uri}");
			args.Cancel = true;
			ViewModel.LinkClicked.Execute(uri);
			
			return;
		}
	}
}