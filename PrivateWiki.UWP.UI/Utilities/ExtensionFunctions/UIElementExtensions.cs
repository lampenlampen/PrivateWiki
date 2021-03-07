using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NLog;

namespace PrivateWiki.UWP.UI.Utilities.ExtensionFunctions
{
	public static class UIElementExtensions
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();

		public static void Shell_GotFocus(this UIElement element, object sender, RoutedEventArgs e)
		{
			var objectType = (e.OriginalSource as FrameworkElement).ToString();
			var objectName = (e.OriginalSource as FrameworkElement).Name;

			var MyFocus = $"Focus on: --{objectType} -- {objectName}";

			Logger.Debug(MyFocus);
		}

		public static void EnableFocusLogging(this UIElement element)
		{
			element.GotFocus += (sender, args) =>
			{
				var objectType = (args.OriginalSource as FrameworkElement).ToString();
				var objectName = (args.OriginalSource as FrameworkElement).Name;

				var MyFocus = $"Focus on: --{objectType} -- {objectName}";

				Logger.Debug(MyFocus);
			};
		}

		public static Frame GetFrame(this UIElement element) => element.FindParent<Frame>();
	}
}