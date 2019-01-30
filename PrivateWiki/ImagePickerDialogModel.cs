using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Windows.UI.Xaml.Media.Imaging;

namespace PrivateWiki
{
	public class ImagePickerDialogModel
	{
		public string Title { get; private set; }
		public Uri ImageUri { get; private set; }
		public BitmapImage Image { get; private set; }

		public ImagePickerDialogModel([NotNull] string title, [NotNull] Uri image)
		{
			Title = title ?? throw new ArgumentNullException(nameof(title)); ;
			ImageUri = image ?? throw new ArgumentNullException(nameof(image)); ;
			Image = new BitmapImage(image) ?? throw new ArgumentNullException(nameof(image)); ;
		}
	}
}