using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace PrivateWiki
{
	public class ImagePickerDialogModel
	{
		public string Title { get; set; }
		public string Image { get; set; }

		public ImagePickerDialogModel([NotNull] string title, [NotNull] string image)
		{
			Title = title ?? throw new ArgumentNullException(nameof(title));
			Image = image ?? throw new ArgumentNullException(nameof(image));
		}
	}
}