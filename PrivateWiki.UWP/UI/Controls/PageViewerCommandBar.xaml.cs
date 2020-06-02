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
using PrivateWiki.Models.Pages;
using PrivateWiki.Models.ViewModels;
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
					.Subscribe(x =>
					{
						_showSettings.OnNext(x);
						SettingsClick?.Invoke(this, new RoutedEventArgs());
					}).DisposeWith(disposable);

				ToTopBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x =>
					{
						_scrollToTop.OnNext(x);
						TopClick?.Invoke(this, new RoutedEventArgs());
					}).DisposeWith(disposable);

				PdfBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x =>
					{
						_printPdf.OnNext(x);
						PdfClick?.Invoke(this, new RoutedEventArgs());
					}).DisposeWith(disposable);

				SearchBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x =>
					{
						_search.OnNext(x);
						SearchClick?.Invoke(this, new RoutedEventArgs());
					}).DisposeWith(disposable);

				HistoryBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x =>
					{
						_showHistory.OnNext(x);
						HistoryClick?.Invoke(this, new RoutedEventArgs());
					}).DisposeWith(disposable);

				FullscreenBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x =>
					{
						_toggleFullscreen.OnNext(x);
						FullscreenClick?.Invoke(this, new RoutedEventArgs());
					}).DisposeWith(disposable);

				ImportBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x =>
					{
						_import.OnNext(x);
						ImportClick?.Invoke(this, new RoutedEventArgs());
					}).DisposeWith(disposable);

				ExportBtn.Events().Click
					.Select(x => Unit.Default)
					.Subscribe(x =>
					{
						_export.OnNext(x);
						ExportClick?.Invoke(this, new RoutedEventArgs());
					}).DisposeWith(disposable);

				EditBtn.Events().Click
					.Select(_ => Unit.Default)
					.Subscribe(x =>
					{
						_edit.OnNext(Unit.Default);
						EditClick?.Invoke(this, new RoutedEventArgs());
					}).DisposeWith(disposable);

				NewPageBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel.NewPageClick)
					.DisposeWith(disposable);

				DevOptBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel.DevOptionsClick)
					.DisposeWith(disposable);
			});

			ShowLastVisitedPages();
		}

		[Obsolete] public event RoutedEventHandler TopClick;

		[Obsolete] public event RoutedEventHandler PdfClick;

		[Obsolete] public event RoutedEventHandler EditClick;

		[Obsolete] public event RoutedEventHandler SearchClick;

		[Obsolete] public event RoutedEventHandler HistoryClick;

		[Obsolete] public event RoutedEventHandler FullscreenClick;

		[Obsolete] public event RoutedEventHandler ExportClick;

		[Obsolete] public event RoutedEventHandler ImportClick;

		[Obsolete] public event RoutedEventHandler SettingsClick;

		private void ShowLastVisitedPages()
		{
			var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
			var stack = NavigationHandler.Pages;

			if (stack.Count <= 0) return;

			for (var index = 0; index < stack.Count - 1; index++)
			{
				var page = stack[index];
				var textBlock = GetTextBlock(page);
				stackPanel.Children.Add(textBlock);

				var delimiterBox = GetDelimiterBlock();
				stackPanel.Children.Add(delimiterBox);
			}

			var lastTextBox = GetTextBlock(stack.Last());
			stackPanel.Children.Add(lastTextBox);

			commandBar.Content = stackPanel;

			Button GetTextBlock(Path path)
			{
				var button = new Button
				{
					Content = path.Title,
					FontSize = 20,
					VerticalAlignment = VerticalAlignment.Center,
					Background = new RevealBackgroundBrush(),
					BorderBrush = new RevealBorderBrush(),
					FontStyle = FontStyle.Italic
				};

				var tooltip = new ToolTip();
				tooltip.Content = path.FullPath;
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
		}

		public delegate void NavigateToPageHandler(object sender, string id);

		[Obsolete] public event NavigateToPageHandler NavigateToPage;
	}
}