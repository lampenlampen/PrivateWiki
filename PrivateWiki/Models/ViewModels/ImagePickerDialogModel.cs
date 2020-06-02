using System;

#nullable enable

namespace PrivateWiki.Models.ViewModels
{
	public class ImagePickerDialogModel
	{
		public string Title { get; private set; }

		public Uri ImageUri { get; private set; }
		//public BitmapImage Image { get; private set; }

		public ImagePickerDialogModel(string title, Uri image)
		{
			Title = title;
			ImageUri = image;
			//Image = new BitmapImage(image);
		}
	}
}