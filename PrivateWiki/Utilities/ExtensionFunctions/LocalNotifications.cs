﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using RavinduL.LocalNotifications;
using RavinduL.LocalNotifications.Notifications;

namespace PrivateWiki.Utilities.ExtensionFunctions
{
	static class LocalNotifications
	{
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
			manager.Show(new SimpleNotification
			{
				TimeSpan = TimeSpan.FromSeconds(3),
				Text = "Page is locked!",
				Glyph = "\uE1F6",
				VerticalAlignment = VerticalAlignment.Bottom,
				Background = new SolidColorBrush(Color.Red.ToWindowsUiColor())
			});
		}
	}
}
