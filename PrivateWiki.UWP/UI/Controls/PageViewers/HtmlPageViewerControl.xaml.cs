using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using NLog;
using PrivateWiki.Models.Pages;
using PrivateWiki.Models.ViewModels;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls.PageViewers
{
	public class HtmlPageViewerControlBase : ReactiveUserControl<HtmlPageViewerControlViewModel>
	{
	}

	public sealed partial class HtmlPageViewerControl : HtmlPageViewerControlBase
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private Uri _uri;

		private ISubject<string> _navigateToPage;

		// public IObservable<string> NavigateToPage => _navigateToPage;

		public HtmlPageViewerControl()
		{
			this.InitializeComponent();

			_navigateToPage = new Subject<string>();

			this.WhenActivated(disposable =>
			{
				ViewModel.Content.Subscribe(x => { Webview.NavigateToString(x); })
					.DisposeWith(disposable);

				ViewModel.OnScrollToTop
					.Subscribe(_ => Webview.InvokeScriptAsync("eval", new[] {"window.scrollTo(0,0);"}))
					.DisposeWith(disposable);

				ViewModel.RenderContent.Execute(Unit.Default)
					.Subscribe(x => DisplayPage(x))
					.DisposeWith(disposable);

				Webview.Events().NavigationStarting
					.Select(a => a.args)
					.Subscribe(WebViewNavigationStarting)
					.DisposeWith(disposable);

				Webview.Events().ScriptNotify
					.Select(x => KeyboardShortcutConverter.Parse(x.Value))
					.InvokeCommand(ViewModel, x => x.KeyPressed)
					.DisposeWith(disposable);

				Webview.Events().UnsupportedUriSchemeIdentified
					.Select(a => a.args)
					.Subscribe(WebViewUnsupportedUriSchemeIdentified)
					.DisposeWith(disposable);

				Webview.Events().ContainsFullScreenElementChanged
					.Select(_ => Unit.Default)
					.Subscribe(x => WebView_OnContainsFullscreenElementChanged())
					.DisposeWith(disposable);
			});
		}

		private void WebViewUnsupportedUriSchemeIdentified(WebViewUnsupportedUriSchemeIdentifiedEventArgs args)
		{
			var uri = args.Uri;

			args.Handled = true;

			Logger.Debug($"UnsupportedUriScheme: {uri.AbsolutePath}");
		}

		private void Webview_OnScriptNotify(NotifyEventArgs e)
		{
			Logger.Debug($"WebView Script+ {e.Value}");

			var key = KeyboardShortcutConverter.Parse(e.Value);

			ViewModel.KeyPressed.Execute(key);
		}

		private async void WebViewNavigationStarting(WebViewNavigationStartingEventArgs args)
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

			if (uri.AbsoluteUri.StartsWith("ms-local-stream:"))
			{
				if (uri.Equals(_uri))
				{
					// Load the index.html page

					Logger.Info("Load Wiki page");
					return;
				}
				else if (uri.LocalPath.StartsWith("/data/:"))
				{
					// Wikilink
					var link = uri.AbsolutePath.Substring(7);

					Logger.Info("Wikilink clicked.");
					Logger.ConditionalDebug($"Wikilink clicked: {link}");

					ViewModel.WikilinkClicked.Execute(Path.ofLink(link));

					args.Cancel = true;
					return;
				}
				else
				{
					// Local file link

					var dataFolder = await App.Current.Config.GetDataFolderAsync();
					var dataPath = dataFolder.Path;
					var localPath = uri.AbsolutePath.Substring(6);

					var a = System.IO.Path.Combine(dataPath, localPath);

					var file = System.IO.Path.GetFullPath(a);

					Windows.System.Launcher.LaunchFileAsync(await StorageFile.GetFileFromPathAsync(file));

					return;
				}
			}

			// Normal Link
			Logger.Info("Normal link clicked.");
			Logger.ConditionalDebug($"Normal link clicked: {uri}");
			args.Cancel = true;
			ViewModel.LinkClicked.Execute(uri);

			return;
		}

		private async void DisplayPage(string htmlContent)
		{
			var dataFolder = await App.Current.Config.GetDataFolderAsync();
			var file = dataFolder.CreateFileAsync("index.html", CreationCollisionOption.ReplaceExisting);

			await FileIO.WriteTextAsync(await file, htmlContent);

			var uri = Webview.BuildLocalStreamUri("wikidata", "/data/index.html");
			_uri = uri;
			var uriResolver = new MyUriToStreamResolver();

			Webview.NavigateToLocalStreamUri(uri, uriResolver);
		}

		private void WebView_OnContainsFullscreenElementChanged()
		{
			var applicationView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();

			if (Webview.ContainsFullScreenElement)
			{
				applicationView.TryEnterFullScreenMode();
			}
			else if (applicationView.IsFullScreenMode)
			{
				applicationView.ExitFullScreenMode();
			}
		}
	}
}