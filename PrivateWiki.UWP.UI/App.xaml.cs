using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NLog;
using PrivateWiki.UWP.UI.UI;
using PrivateWiki.UWP.UI.UI.Pages;
using RavinduL.LocalNotifications;
using SimpleInjector;

namespace PrivateWiki.UWP.UI
{
	/// <summary>
	///     Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
	/// </summary>
	sealed partial class App : Windows.UI.Xaml.Application
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// By default, current is an instance of the Application class, which needs to be changed to be an instance of the App class.
		public new static App Current { get; private set; }

		[Obsolete] public AppConfig Config { get; } = new AppConfig();

		[Obsolete] public GlobalNotificationManager GlobalNotificationManager { get; private set; }

		[Obsolete] internal Grid NotificationGrid { get; } = new();


		/// <summary>
		///     Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
		///     und daher das logische Äquivalent von main() bzw. WinMain().
		/// </summary>
		public App()
		{
			Current = this;
			InitializeComponent();
			Suspending += OnSuspending;

			var container = new Container();
			InitDIContainer(container);
			ServiceLocator.Container = container;
		}

		private void InitDIContainer(Container container)
		{
			CompositionRoot.CompositionRoot.Bootstrap(container);
			UwpCompositionRoot.Bootstrap(container);
		}


		/// <summary>
		///     Wird aufgerufen, wenn die Anwendung durch den Endbenutzer normal gestartet wird. Weitere Einstiegspunkte
		///     werden z. B. verwendet, wenn die Anwendung gestartet wird, um eine bestimmte Datei zu öffnen.
		/// </summary>
		/// <param name="e">Details über Startanforderung und -prozess.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			Logger.Info("App Launched");

			ServiceLocator.Initialize();

			Grid? rootGrid = Window.Current.Content as Grid;
			Frame rootFrame = rootGrid?.Children.Where((c) => c is Frame).Cast<Frame>().FirstOrDefault();

			if (rootGrid == null)
			{
				rootGrid = new Grid();
				rootFrame = new Frame();

				var notificationGrid = NotificationGrid;

				GlobalNotificationManager = new GlobalNotificationManager(new LocalNotificationManager(notificationGrid));

				rootGrid.Children.Add(rootFrame);
				rootGrid.Children.Add(notificationGrid);

				Window.Current.Content = rootGrid;
			}

			if (e.PrelaunchActivated == false)
			{
				if (rootFrame.Content == null) ShowPage(rootFrame);

				// Sicherstellen, dass das aktuelle Fenster aktiv ist

				Window.Current.Activate();
			}
		}

		protected override void OnActivated(IActivatedEventArgs args)
		{
			Logger.Info("App Activated");

			ServiceLocator.Initialize();

			// Window management
			if (!(Window.Current.Content is Frame rootFrame))
			{
				rootFrame = new Frame();
				Window.Current.Content = rootFrame;
				ShowPage(rootFrame);
			}

			Window.Current.Activate();
		}

		private void ShowPage(Frame rootFrame)
		{
			rootFrame.Content = new MainPage();
		}

		/// <summary>
		///     Wird aufgerufen, wenn die Navigation auf eine bestimmte Seite fehlschlägt
		/// </summary>
		/// <param name="sender">Der Rahmen, bei dem die Navigation fehlgeschlagen ist</param>
		/// <param name="e">Details über den Navigationsfehler</param>
		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		/// <summary>
		///     Wird aufgerufen, wenn die Ausführung der Anwendung angehalten wird.  Der Anwendungszustand wird gespeichert,
		///     ohne zu wissen, ob die Anwendung beendet oder fortgesetzt wird und die Speicherinhalte dabei
		///     unbeschädigt bleiben.
		/// </summary>
		/// <param name="sender">Die Quelle der Anhalteanforderung.</param>
		/// <param name="e">Details zur Anhalteanforderung.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Anwendungszustand speichern und alle Hintergrundaktivitäten beenden
			deferral.Complete();
		}
	}
}