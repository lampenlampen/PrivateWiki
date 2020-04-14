using System;
using Windows.UI.Xaml.Media.Imaging;

#nullable enable

namespace PrivateWiki
{
	public class ImagePickerDialogModel
	{
		public string Title { get; private set; }
		public Uri ImageUri { get; private set; }
		public BitmapImage Image { get; private set; }

		public ImagePickerDialogModel(string title, Uri image)
		{
			Title = title;
			ImageUri = image;
			Image = new BitmapImage(image);
		}
	}
}