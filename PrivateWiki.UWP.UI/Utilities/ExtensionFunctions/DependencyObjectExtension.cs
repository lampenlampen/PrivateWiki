using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace PrivateWiki.UWP.UI.Utilities.ExtensionFunctions
{
	public static class DependencyObjectExtension
	{
		public static T? FindParent<T>(this DependencyObject child) where T : notnull, DependencyObject
		{
			T? parent = null;
			DependencyObject CurrentParent = VisualTreeHelper.GetParent(child);
			while (CurrentParent != null)
			{
				if (CurrentParent is T)
				{
					parent = (T) CurrentParent;
					break;
				}

				CurrentParent = VisualTreeHelper.GetParent(CurrentParent);
			}

			return parent;
		}
	}
}