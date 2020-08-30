using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using NLog;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.ViewModels;
using PrivateWiki.ViewModels.PageEditors;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Controls.PageEditors
{
	public abstract class PageEditorControlBase<T> : ReactiveUserControl<T> where T : PageEditorControlViewModelBase
	{
	}

	public abstract class MarkdownPageEditorControlBase : PageEditorControlBase<MarkdownPageEditorControlViewModel>
	{
	}

	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MarkdownPageEditorControl : MarkdownPageEditorControlBase
	{
		private readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly ISubject<Label> _onDeleteLabel = new Subject<Label>();

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

				this.WhenAnyValue(x => x.ViewModel.PageLabels)
					.WhereNotNull()
					.BindTo(LabelsListView, x => x.ItemsSource)
					.DisposeWith(disposable);

				_onDeleteLabel
					.InvokeCommand(ViewModel, vm => vm.RemoveLabel)
					.DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.AddLabelsList)
					.WhereNotNull()
					.BindTo(AddLabelBox, x => x.ItemsSource)
					.DisposeWith(disposable);

				AddLabelFlyout.Events().Closing
					.Select(_ => AddLabelBox.SelectedItems.Select(x => (Label) x))
					.InvokeCommand(ViewModel, vm => vm.AddLabels)
					.DisposeWith(disposable);

				this.Bind(ViewModel,
						vm => vm.AddLabelsQueryText,
						view => view.FilterQueryTextBox.Text)
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

		private void DeleteLabelBtn(object sender, RoutedEventArgs e)
		{
			var label = (Label) ((Button) sender).DataContext;

			_onDeleteLabel.OnNext(label);
		}
	}
}