using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using NLog;
using PrivateWiki.ViewModels;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls
{
	public class PageViewerCommandBarBase : ReactiveUserControl<PageViewerCommandBarViewModel>
	{
	}

	public sealed partial class PageViewerCommandBar : PageViewerCommandBarBase
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public new object Content
		{
			get => commandBar.Content;
			set => commandBar.Content = value;
		}

		public IObservable<Unit> ShowSettings => _showSettings;
		private readonly ISubject<Unit> _showSettings;

		public IObservable<Unit> ScrollToTop => _scrollToTop;
		private readonly ISubject<Unit> _scrollToTop;

		public IObservable<Unit> PrintPdf => _printPdf;
		private readonly ISubject<Unit> _printPdf;

		public IObservable<Unit> Edit => _edit;
		private readonly ISubject<Unit> _edit;

		public IObservable<Unit> Search => _search;
		private readonly ISubject<Unit> _search;

		public IObservable<Unit> ShowHistory => _showHistory;
		private readonly ISubject<Unit> _showHistory;

		public IObservable<Unit> ToggleFullscreen => _toggleFullscreen;
		private readonly ISubject<Unit> _toggleFullscreen;

		public IObservable<Unit> Import => _import;
		private readonly ISubject<Unit> _import;

		public IObservable<Unit> Export => _export;
		private readonly ISubject<Unit> _export;

		public IObservable<string> NavigateToPage2 => _navigateToPage;
		private readonly ISubject<string> _navigateToPage;

		public PageViewerCommandBar()
		{
			this.InitializeComponent();

			_showSettings = new Subject<Unit>();
			_scrollToTop = new Subject<Unit>();
			_printPdf = new Subject<Unit>();
			_edit = new Subject<Unit>();
			_search = new Subject<Unit>();
			_showHistory = new Subject<Unit>();
			_toggleFullscreen = new Subject<Unit>();
			_import = new Subject<Unit>();
			_export = new Subject<Unit>();
			_navigateToPage = new Subject<string>();

			this.WhenActivated(disposable =>
			{
				this.OneWayBind(ViewModel,
						vm => vm.DevOptsEnabled,
						view => view.DevOptBtn.Visibility)
					.DisposeWith(disposable);

				SettingsBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x => { _showSettings.OnNext(x); }).DisposeWith(disposable);

				ToTopBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x => { _scrollToTop.OnNext(x); }).DisposeWith(disposable);

				PdfBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x => { _printPdf.OnNext(x); }).DisposeWith(disposable);

				SearchBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x => { _search.OnNext(x); }).DisposeWith(disposable);

				HistoryBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x => { _showHistory.OnNext(x); }).DisposeWith(disposable);

				FullscreenBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x => { _toggleFullscreen.OnNext(x); }).DisposeWith(disposable);

				ImportBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x => { _import.OnNext(x); }).DisposeWith(disposable);

				ExportBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x => { _export.OnNext(x); }).DisposeWith(disposable);

				EditBtn.Events().Click
					.Select(_ => Unit.Default)
					.Subscribe(x => { _edit.OnNext(Unit.Default); }).DisposeWith(disposable);

				NewPageBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel.NewPageClick)
					.DisposeWith(disposable);

				DevOptBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel.DevOptionsClick)
					.DisposeWith(disposable);

				ShowLastVisitedPages();
			});
		}

		private void ShowLastVisitedPages()
		{
			Button GetTextBlock(MostRecentlyViewedPagesItem item)
			{
				var button = new Button
				{
					Content = item.Path.Title,
					FontSize = 20,
					VerticalAlignment = VerticalAlignment.Center,
					Background = new RevealBackgroundBrush(),
					BorderBrush = new RevealBorderBrush(),
					FontStyle = FontStyle.Italic
				};

				var tooltip = new ToolTip();
				tooltip.Content = item.Path.FullPath;
				ToolTipService.SetToolTip(button, tooltip);

				button.Click += (sender, args) =>
				{
					var id = (string) ((Button) sender).Content;

					_navigateToPage.OnNext(id);
					NavigateToPage?.Invoke(sender, id);
				};

				return button;
			}

			TextBlock GetDelimiterBlock()
			{
				return new TextBlock
				{
					Text = ">",
					FontSize = 20,
					VerticalAlignment = VerticalAlignment.Center,
					Margin = new Thickness(2, 0, 2, 0)
				};
			}

			var root = new StackPanel {Orientation = Orientation.Horizontal};

			var pages = ViewModel.MostRecentlyViewedPages;

			if (!pages.Any()) return;

			for (var i = 0; i < pages.Count() - 1; i++)
			{
				var page = pages.ElementAt(i);

				var textBtn = GetTextBlock(page);
				root.Children.Add(textBtn);

				var delimiter = GetDelimiterBlock();
				root.Children.Add(delimiter);
			}

			var textBox = GetTextBlock(pages.Last());
			root.Children.Add(textBox);

			commandBar.Content = root;
		}

		[Obsolete]
		public delegate void NavigateToPageHandler(object sender, string id);

		[Obsolete] public event NavigateToPageHandler NavigateToPage;
	}
}