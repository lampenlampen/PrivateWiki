using System;
using System.Drawing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using NLog;
using PrivateWiki.Utilities;
using RavinduL.LocalNotifications;
using RavinduL.LocalNotifications.Notifications;

namespace PrivateWiki.UI
{
	public class GlobalNotificationManager
	{
		private LocalNotificationManager _manager;

		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public GlobalNotificationManager(LocalNotificationManager manager)
		{
			_manager = manager;
		}

		public void ShowNotImplementedNotification()
		{
			_manager.Show(new SimpleNotification
			{
				TimeSpan = TimeSpan.FromSeconds(3),
				Text = "This feature is not yet implemented!",
				Glyph = "\uE783",
				VerticalAlignment = VerticalAlignment.Bottom,
				Background = Color.Red.ToBrush()
			});
		}

		public void ShowOperationFinishedNotification()
		{
			_manager.Show(new SimpleNotification
			{
				TimeSpan = TimeSpan.FromSeconds(3),
				Text = "Operation Finished!",
				Glyph = "\uE001",
				VerticalAlignment = VerticalAlignment.Bottom,
			});
		}

		public void ShowPageLockedNotification()
		{
			Logger.Debug("Show Page locked notification");

			_manager.Show(new SimpleNotification
			{
				TimeSpan = TimeSpan.FromSeconds(3),
				Text = "Page is locked!",
				Glyph = "\uE1F6",
				VerticalAlignment = VerticalAlignment.Bottom,
				Background = new SolidColorBrush(Color.Red.ToWindowsUiColor())
			});
		}

		public void ShowPageExistsNotificationOnUIThread()
		{
			Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
				() =>
				{
					_manager.Show(new SimpleNotification
					{
						Text = "Page exists already",
						VerticalAlignment = VerticalAlignment.Bottom,
						Background = Color.Red.ToBrush()
					});
				});
		}

		public delegate SimpleNotification NotificationDelegate();

		public void ShowOnUIThread(NotificationDelegate d)
		{
			Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
				() => { _manager.Show(d.Invoke()); });
		}

		public void Show(SimpleNotification notification)
		{
			_manager.Show(notification);
		}
	}

	public static class Notifications
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public static void ShowNotImplementedNotification(this GlobalNotificationManager manager)
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

		public static void ShowOperationFinishedNotification(this GlobalNotificationManager manager)
		{
			manager.Show(new SimpleNotification
			{
				TimeSpan = TimeSpan.FromSeconds(3),
				Text = "Operation Finished!",
				Glyph = "\uE001",
				VerticalAlignment = VerticalAlignment.Bottom,
			});
		}

		public static void ShowPageLockedNotification(this GlobalNotificationManager manager)
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

		public static void ShowPageExistsNotificationOnUIThread(this GlobalNotificationManager manager)
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

		public static void ShowOnUIThread(this GlobalNotificationManager manager, NotificationDelegate d)
		{
			Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
				() => { manager.Show(d.Invoke()); });
		}
	}
}