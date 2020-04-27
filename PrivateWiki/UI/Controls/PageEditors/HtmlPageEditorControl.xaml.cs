using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Models.ViewModels;
using PrivateWiki.Models.ViewModels.PageEditors;
using PrivateWiki.Rendering;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Controls.PageEditors
{
	public class HtmlPageEditorControlBase : PageEditorControlBase<HtmlPageEditorControlViewModel>
	{
	}

	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class HtmlPageEditorControl : HtmlPageEditorControlBase
	{
		public HtmlPageEditorControl()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				commandBar.ViewModel = (PageEditorCommandBarViewModel) ViewModel.CommandBarViewModel;

				this.Bind(ViewModel,
						vm => vm.Content,
						view => view.PageEditorTextBox.Text)
					.DisposeWith(disposable);

				Pivot.Events().SelectionChanged
					.Subscribe(_ => ViewModel.CurrentPivotItem = SelectedItemToPivotItemConverter(Pivot.SelectedItem))
					.DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.CurrentPivotItem)
					.Subscribe(OnPivotSelectionChanged)
					.DisposeWith(disposable);

				Preview_WebView.Events().NavigationStarting
					.Select(x => x.args)
					.Subscribe(ShowPreviewLinksAsNotifications)
					.DisposeWith(disposable);
			});
		}

		private HtmlPageEditorControlPivotItem SelectedItemToPivotItemConverter(object selectedItem)
		{
			var selectedPivotItem = (PivotItem) selectedItem;

			switch (selectedPivotItem.Name)
			{
				case "EditorPivotItem":
					return HtmlPageEditorControlPivotItem.Editor;
				case "PreviewPivotItem":
					return HtmlPageEditorControlPivotItem.Preview;
				case "MetadataPivotItem":
					return HtmlPageEditorControlPivotItem.Metadata;
				default:
					throw new ArgumentOutOfRangeException(nameof(selectedItem), selectedItem, $"PivotItem {selectedPivotItem.Name} not handled!");
			}
		}

		private async void OnPivotSelectionChanged(HtmlPageEditorControlPivotItem item)
		{
			ContentRenderer renderer = new ContentRenderer();
			string content;
			switch (item)
			{
				case HtmlPageEditorControlPivotItem.Editor:
					break;
				case HtmlPageEditorControlPivotItem.Preview:
					content = await renderer.RenderContentAsync(ViewModel.Content, "markdown");
					Preview_WebView.NavigateToString(content);
					break;
				case HtmlPageEditorControlPivotItem.Metadata:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(item), item, null);
			}
		}

		private void ShowPreviewLinksAsNotifications(WebViewNavigationStartingEventArgs args)
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
				App.Current.GlobalNotificationManager.ShowLinkClickedNotification(link);

				args.Cancel = true;
				return;
			}

			if (uri.AbsoluteUri.Contains("about:"))
			{
				// Local link
				return;
			}

			// Normal Link
			App.Current.GlobalNotificationManager.ShowLinkClickedNotification(uri.ToString());
			args.Cancel = true;
		}
	}
}