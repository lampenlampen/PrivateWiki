using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.UWP.UI.UI.TemplateSelector
{
	class PageHistoryTemplateSelector : DataTemplateSelector
	{
		public DataTemplate CreatedTemplate { get; set; }

		public DataTemplate LockedTemplate { get; set; }

		public DataTemplate UnlockedTemplate { get; set; }

		public DataTemplate EditedTemplate { get; set; }

		public DataTemplate DeletedTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{
			var page = (PageHistory<GenericPage>) item;

			return page.Action switch
			{
				PageAction.Created => CreatedTemplate,
				PageAction.Locked => LockedTemplate,
				PageAction.Unlocked => UnlockedTemplate,
				PageAction.Edited => EditedTemplate,
				PageAction.Deleted => DeletedTemplate,
				_ => EditedTemplate
			};
		}
	}
}