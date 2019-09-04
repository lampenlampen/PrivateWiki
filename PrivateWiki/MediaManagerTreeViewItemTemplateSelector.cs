using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using muxc=Microsoft.UI.Xaml.Controls;

namespace PrivateWiki
{
	internal class MediaManagerTreeViewItemTemplateSelector : DataTemplateSelector
	{
		public DataTemplate FolderTemplate { get; set; }
		public DataTemplate FileTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{
			var node = (muxc.TreeViewNode)item;

			if (node.Content is StorageFolder) return FolderTemplate;

			return FileTemplate;
		}
	}
}