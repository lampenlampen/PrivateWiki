using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
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
	}
}
