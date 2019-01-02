using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PrivateWiki
{
	class MediaManagerTreeViewItemTemplateSelector : DataTemplateSelector
	{
		public DataTemplate FolderTemplate { get; set; }
		public DataTemplate FileTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{
			var node = (TreeViewNode) item;

			if (node.Content is StorageFolder)
			{
				return FolderTemplate;
			}

			return FileTemplate;
		}
	}
}