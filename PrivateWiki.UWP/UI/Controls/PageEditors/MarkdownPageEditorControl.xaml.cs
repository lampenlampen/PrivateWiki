using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;
using NLog;
using PrivateWiki.ViewModels;
using PrivateWiki.ViewModels.PageEditors;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Controls.PageEditors
{
	public class PageEditorControlBase<T> : ReactiveUserControl<T> where T : PageEditorControlViewModelBase { }

	public abstract class MarkdownPageEditorControlBase : PageEditorControlBase<MarkdownPageEditorControlViewModel> { }

	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MarkdownPageEditorControl : MarkdownPageEditorControlBase
	{
		private readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public MarkdownPageEditorControl()
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

				this.WhenAnyValue(x => x.ViewModel.PreviewContent)
					.Subscribe(x => Preview_WebView.NavigateToString(x))
					.DisposeWith(disposable);

				this.OneWayBind(ViewModel,
						vm => vm.HtmlPreviewContent,
						view => view.Preview_Html.Text)
					.DisposeWith(disposable);

				Preview_WebView.Events().NavigationStarting
					.Select(x => x.args)
					.Subscribe(ShowPreviewLinksAsNotifications)
					.DisposeWith(disposable);
			});
		}

		private MarkdownPageEditorControlPivotItem SelectedItemToPivotItemConverter(object selectedItem)
		{
			var selectedPivotItem = (PivotItem) selectedItem;

			switch (selectedPivotItem.Name)
			{
				case "EditorPivotItem":
					return MarkdownPageEditorControlPivotItem.Editor;
				case "PreviewPivotItem":
					return MarkdownPageEditorControlPivotItem.Preview;
				case "HtmlPreviewPivotItem":
					return MarkdownPageEditorControlPivotItem.HtmlPreview;
				case "MetadataPivotItem":
					return MarkdownPageEditorControlPivotItem.Metadata;
				default:
					throw new ArgumentOutOfRangeException(nameof(selectedItem), selectedItem, $"PivotItem {selectedPivotItem.Name} not handled!");
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