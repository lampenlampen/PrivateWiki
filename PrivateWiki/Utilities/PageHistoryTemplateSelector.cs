using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Models.Pages;

namespace PrivateWiki.Utilities
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
			return item switch
			{
				CreatedHistoryMarkdownPage _=> CreatedTemplate,
				LockedHistoryMarkdownPage _ => LockedTemplate,
				UnlockedHistoryMarkdownPage _ => UnlockedTemplate,
				EditedHistoryMarkdownPage _ => EditedTemplate,
				DeletedHistoryMarkdownPage _ => DeletedTemplate,
				_ => EditedTemplate
			};
		}
	}
}
