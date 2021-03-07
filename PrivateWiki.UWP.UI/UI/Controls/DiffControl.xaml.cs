using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.UWP.UI.Data;
using RuntimeComponent;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.UI.Controls
{
	public sealed partial class DiffControl : UserControl
	{
		private bool _isWebViewNavigationCompleted = false;
		private bool _isDiffLoadCompleted = false;

		private DiffWebViewCommunicator Model { get; set; } = new DiffWebViewCommunicator();


		public DiffControl()
		{
			this.InitializeComponent();

			init();
		}

		private async void init()
		{
			var (old, newText) = await LoadDiff();
			Model.Old = old;
			Model.New = newText;

			var old1 =
				"# PrivateWiki\r\n\r\nA private wiki built on UWP and with markdown as wiki language.\r\n\r\n## Features\r\n\r\n## Screenshots\r\n\r\nThe Page Viewer:\r\n\r\n![Page Viewer](Images/PrivateWiki_Viewer.png)\r\n\r\nAnd the Editor:\r\n![Page Editor](Images/PrivateWiki_Editor.png)\r\n\r\nAnd the Settings Page:\r\n![Settings Page](Images/PrivateWiki_Settings_Rendering.png)";
			var new1 =
				"# PrivateWiki\r\n\r\nA private wiki built on UWP and with markdown as wiki language.\r\n\r\n## Features\r\n\r\n- Page viewing and editing\r\n- Page History\r\n- Search: ugly but working; only searches for the page link not content\r\n- Customizable rendering pipeline\r\n- Export to markdown and html\r\n\r\n\r\n## Screenshots\r\n\r\nThe Page Viewer:\r\n\r\n![Page Viewer](Images/PrivateWiki_Viewer.png)\r\n\r\nAnd the Editor:\r\n\r\n![Page Editor](Images/PrivateWiki_Editor.png)\r\n\r\nAnd the Settings Page:\r\n\r\n![Settings Page](Images/PrivateWiki_Settings_Rendering.png)";

			Model.Old = old1;
			Model.New = new1;

			DiffWebView.Navigate(new Uri("ms-appx-web:///assets/DiffView/index.html"));
		}

		private async Task<(string, string)> LoadDiff()
		{
			var assetsFolder = await FileSystemAccess.GetAssetsFolderAsync();
			var diffFolder = await assetsFolder.GetFolderAsync("DiffView");
			var old = await FileIO.ReadTextAsync(await diffFolder.GetFileAsync("old.txt"));
			var newText = await FileIO.ReadTextAsync(await diffFolder.GetFileAsync("new.txt"));

			return (old, newText);
		}

		private async void DiffWebView_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
		{
			DiffWebView.AddWebAllowedObject("model", Model);

			_isDiffLoadCompleted = true;

			ShowDiff();
		}


		private async void DiffWebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
		{
			_isWebViewNavigationCompleted = true;

			ShowDiff();

			int width;
			int height;

			// get the total width and height
			var widthString = await sender.InvokeScriptAsync("eval", new[] {"document.body.scrollWidth.toString()"});
			var heightString = await sender.InvokeScriptAsync("eval", new[] {"document.body.scrollHeight.toString()"});

			if (!int.TryParse(widthString, out width))
			{
				throw new Exception("Unable to get page width");
			}

			if (!int.TryParse(heightString, out height))
			{
				throw new Exception("Unable to get page height");
			}

			// resize the webview to the content
			//sender.Width = width;
			sender.Height = height;
		}

		private async void ShowDiff()
		{
			if (_isWebViewNavigationCompleted && _isDiffLoadCompleted)
			{
				await DiffWebView.InvokeScriptAsync("eval", new string[] {"showDiff();"});
			}
		}

		private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			//DiffWebView.AddWebAllowedObject("model", Model);
			await DiffWebView.InvokeScriptAsync("eval", new string[] {"showDiff();"});

			DiffWebView_OnNavigationCompleted(DiffWebView, null);
		}
	}
}