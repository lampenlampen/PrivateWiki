using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Data.DataAccess;
using PrivateWiki.Pages;

namespace PrivateWiki
{
    /// <summary>
    ///     Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
    /// </summary>
    sealed partial class App : Application
	{
        /// <summary>
        ///     Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
        ///     und daher das logische Äquivalent von main() bzw. WinMain().
        /// </summary>
        public App()
		{
			InitializeComponent();
			Suspending += OnSuspending;

			var dataAccess = new DataAccessImpl();
			dataAccess.InitializeDatabase();
			DefaultPages.InsertDefaultPages();

			var pages = dataAccess.GetPages();

		}

        /// <summary>
        ///     Wird aufgerufen, wenn die Anwendung durch den Endbenutzer normal gestartet wird. Weitere Einstiegspunkte
        ///     werden z. B. verwendet, wenn die Anwendung gestartet wird, um eine bestimmte Datei zu öffnen.
        /// </summary>
        /// <param name="e">Details über Startanforderung und -prozess.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			var rootFrame = Window.Current.Content as Frame;

			// App-Initialisierung nicht wiederholen, wenn das Fenster bereits Inhalte enthält.
			// Nur sicherstellen, dass das Fenster aktiv ist.
			if (rootFrame == null)
			{
				// Frame erstellen, der als Navigationskontext fungiert und zum Parameter der ersten Seite navigieren
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: Zustand von zuvor angehaltener Anwendung laden
				}

				// Den Frame im aktuellen Fenster platzieren
				Window.Current.Content = rootFrame;
			}

			if (e.PrelaunchActivated == false)
			{
				if (rootFrame.Content == null) rootFrame.Navigate(typeof(MainPage), e.Arguments);

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