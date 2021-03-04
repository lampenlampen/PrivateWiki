using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using NLog;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.UWP.Utilities.ExtensionFunctions;
using PrivateWiki.ViewModels;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls
{
	public class GlobalSearchControlBase : ReactiveUserControl<GlobalSearchControlViewModel> { }

	public sealed partial class GlobalSearchControl : GlobalSearchControlBase
	{
		private readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private IObservable<EventPattern<KeyboardAcceleratorInvokedEventArgs>> _onEscapeKeyPressed;

		public GlobalSearchControl()
		{
			this.InitializeComponent();
			// Testing
			this.EnableFocusLogging();

			AddKeyboardAccelerators();

			this.WhenActivated(disposable =>
			{
				Logger.Debug("Search activated");

				SearchBox.Events().TextChanged
					.Where(x => x.args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
					.Select(x => x.sender.Text)
					.BindTo(ViewModel, x => x.SearchQuery)
					.DisposeWith(disposable);

				/*SearchBox.Events().SuggestionChosen
					.Subscribe(x =>
					{
						// Display chosen page as selected item in autosuggestbox
						var page = (GenericPage) x.args.SelectedItem;
						x.sender.Text = page.Path.FullPath;
					})
					.DisposeWith(disposable);*/

				SearchBox.Events().QuerySubmitted
					.Select(x => (GenericPage) x.args.ChosenSuggestion)
					.InvokeCommand(ViewModel, vm => vm.PageSelected)
					.DisposeWith(disposable);

				this.OneWayBind(ViewModel,
						vm => vm.FilteredPages,
						view => view.SearchBox.ItemsSource)
					.DisposeWith(disposable);

				SearchBox.Events().Loaded
					.Subscribe(_ => SetFocus())
					.DisposeWith(disposable);


				SearchBox.Text = "";

				_onEscapeKeyPressed
					.Do(x => x.EventArgs.Handled = true)
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel, x => x.HideSearchWindow)
					.DisposeWith(disposable);
			});
		}

		private void AddKeyboardAccelerators()
		{
			var accelerator = new KeyboardAccelerator
			{
				Key = VirtualKey.Escape
			};
			_onEscapeKeyPressed =
				Observable.FromEventPattern<KeyboardAcceleratorInvokedEventArgs>(accelerator,
					nameof(accelerator.Invoked));

			KeyboardAccelerators.Add(accelerator);

			var accelerator2 = new KeyboardAccelerator
			{
				Key = VirtualKey.Escape
			};
			_onEscapeKeyPressed =
				Observable.FromEventPattern<KeyboardAcceleratorInvokedEventArgs>(accelerator2,
					nameof(accelerator2.Invoked));

			SearchBox.KeyboardAccelerators.Add(accelerator2);
		}

		public void SetFocus()
		{
			SearchBox.IsTabStop = true;
			//var result = SearchBox.Focus(FocusState.Keyboard);

			// slightly delay setting focus
			Task.Run(async () =>
			{
				await Task.Delay(4000).ConfigureAwait(false);

				return Dispatcher.RunAsync(CoreDispatcherPriority.Low,
					() =>
					{
						var result = SearchBox.Focus(FocusState.Programmatic);

						var (a, b, c, d, e) = (SearchBox.Visibility, SearchBox.IsEnabled, SearchBox.IsTabStop,
							SearchBox.IsLoaded, SearchBox.FocusState);

						Logger.Info(
							$"Set Focus to SearchBox successful: {result} (Visibility: {a}, Enabled: {b}, TabStob: {c}, Loaded: {d}, FocusState: {e})");
					});
			});

			var (a, b, c, d, e) = (SearchBox.Visibility, SearchBox.IsEnabled, SearchBox.IsTabStop, SearchBox.IsLoaded,
				SearchBox.FocusState);
			Logger.Info(
				$"Set Focus to SearchBox successful: (Visibility: {a}, Enabled: {b}, TabStob: {c}, Loaded: {d}, FocusState: {e})");
		}
	}
}