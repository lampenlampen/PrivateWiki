using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Models;

namespace PrivateWiki
{
	public class SettingStorageTemplateSelector : DataTemplateSelector
	{
		public DataTemplate DividerTemplate { get; set; }
		public DataTemplate LFSTemplate { get; set; }
		public DataTemplate HeaderTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{

			switch (item)
			{
				case HeaderItem _: return HeaderTemplate;
				case DividerItem _: return DividerTemplate;
				case LFSModel _: return LFSTemplate;
			}

			throw new ArgumentException();
		}
	}
}