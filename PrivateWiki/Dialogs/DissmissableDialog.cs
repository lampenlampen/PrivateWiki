using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace PrivateWiki.Dialogs
{
	public class DissmissableDialog : ContentDialog
	{
		/// <summary>
		/// Reference to the rectangle behind the dialog
		/// </summary>
		private Rectangle _rectangle;

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// Get all Open Popups
			// Normally there are 2 - one for the dialog and one for the rectangle
			var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
			foreach (var popup in popups)
			{
				if (popup.Child is Rectangle rectangle)
				{
					// Store a reference to unregister the event handler later
					_rectangle = rectangle;
					_rectangle.Tapped += OnLockRectangleTapped;
				}
			}
		}

		private void OnLockRectangleTapped(object sender, TappedRoutedEventArgs e)
		{
			// Hide the dialog and unregister the event handler
			Hide();
			_rectangle.Tapped -= OnLockRectangleTapped;
		}
	}
}