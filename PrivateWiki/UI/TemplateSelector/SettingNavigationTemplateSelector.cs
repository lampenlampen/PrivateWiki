using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Models;

namespace PrivateWiki.UI.TemplateSelector
{
	public class SettingNavigationTemplateSelector : DataTemplateSelector
	{
		public DataTemplate DividerTemplate { get; set; }
		public DataTemplate LinkTemplate { get; set; }
		public DataTemplate HeaderTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{
			switch (item)
			{
				case HeaderItem _: return HeaderTemplate;
				case DividerItem _: return DividerTemplate;
				case LinkItem _: return LinkTemplate;
			}

			throw new ArgumentException();
		}
	}
}