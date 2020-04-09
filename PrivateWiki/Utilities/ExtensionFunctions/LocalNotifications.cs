using System;
using System.Drawing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using NLog;
using RavinduL.LocalNotifications;
using RavinduL.LocalNotifications.Notifications;

namespace PrivateWiki.Utilities.ExtensionFunctions
{
	static class LocalNotifications
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public static void ShowNotImplementedNotification(this LocalNotificationManager manager)
		{
			manager.Show(new SimpleNotification
			{
				TimeSpan = TimeSpan.FromSeconds(3),
				Text = "This feature is not yet implemented!",
				Glyph = "\uE783",
				VerticalAlignment = VerticalAlignment.Bottom,
				Background = Color.Red.ToBrush()
			});
		}

		public static void ShowOperationFinishedNotification(this LocalNotificationManager manager)
		{
			manager.Show(new SimpleNotification
			{
				TimeSpan = TimeSpan.FromSeconds(3),
				Text = "Operation Finished!",
				Glyph = "\uE001",
				VerticalAlignment = VerticalAlignment.Bottom,
			});
		}

		public static void ShowPageLockedNotification(this LocalNotificationManager manager)
		{
			Logger.Debug("Show Page locked notification");

			manager.Show(new SimpleNotification
			{
				TimeSpan = TimeSpan.FromSeconds(3),
				Text = "Page is locked!",
				Glyph = "\uE1F6",
				VerticalAlignment = VerticalAlignment.Bottom,
				Background = new SolidColorBrush(Color.Red.ToWindowsUiColor())
			});
		}

		public static void ShowPageExistsNotificationOnUIThread(this LocalNotificationManager manager)
		{
			Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
				() =>
				{
					manager.Show(new SimpleNotification
					{
						Text = "Page exists already",
						VerticalAlignment = VerticalAlignment.Bottom,
						Background = Color.Red.ToBrush()
					});
				});
		}

		public delegate SimpleNotification NotificationDelegate();

		public static void ShowOnUIThread(this LocalNotificationManager manager, NotificationDelegate d)
		{
			Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
				() => { manager.Show(d.Invoke()); });
		}
	}
}