using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Pages;
using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Models.Storage;
using NLog;
using RavinduL.LocalNotifications;
using StorageBackend.SQLite;

namespace PrivateWiki
{
	/// <summary>
	///     Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
	/// </summary>
	sealed partial class App : Application
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// By default, current is an instance of the Application class, which needs to be changed to be an instance of the App class.
		public static new App Current;

		public LocalNotificationManager manager;

		/// <summary>
		///     Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
		///     und daher das logische Äquivalent von main() bzw. WinMain().
		/// </summary>
		public App()
		{
			Current = this;
			InitializeComponent();
			Suspending += OnSuspending;
			
			var storage = new SqLiteStorage("test");
			var sqliteBackend = new SqLiteBackend(storage, SystemClock.Instance);

			DefaultPages.InsertDefaultMarkdownPagesAsync(sqliteBackend, SystemClock.Instance);

			Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
			NLog.LogManager.Configuration.Variables["LogPath"] = storageFolder.Path;

			RegisterUncaughtExceptionLoggerAsync();
		}

		private void RegisterUncaughtExceptionLoggerAsync()
		{
			UnhandledException += (sender, args) =>
			{
				Logger.Error(args.Exception);
				Logger.Error(args.Message);
			};
		}

		/// <summary>
		///     Wird aufgerufen, wenn die Anwendung durch den Endbenutzer normal gestartet wird. Weitere Einstiegspunkte
		///     werden z. B. verwendet, wenn die Anwendung gestartet wird, um eine bestimmte Datei zu öffnen.
		/// </summary>
		/// <param name="e">Details über Startanforderung und -prozess.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			Grid rootGrid = Window.Current.Content as Grid;
			Frame rootFrame = rootGrid?.Children.Where((c) => c is Frame).Cast<Frame>().FirstOrDefault();

			if (rootGrid == null)
			{
				rootGrid = new Grid();
				rootFrame = new Frame();

				var notificationGrid = new Grid();

				manager = new LocalNotificationManager(notificationGrid);

				rootGrid.Children.Add(rootFrame);
				rootGrid.Children.Add(notificationGrid);

				// ...

				Window.Current.Content = rootGrid;
			}

			if (e.PrelaunchActivated == false)
			{
				if (rootFrame.Content == null) rootFrame.Navigate(typeof(PageViewer), "start");

				// Sicherstellen, dass das aktuelle Fenster aktiv ist
				Window.Current.Activate();
			}
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