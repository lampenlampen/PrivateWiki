using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Models;

namespace PrivateWiki
{
	public class SettingSyncTemplateSelector : DataTemplateSelector
	{
		public DataTemplate LFSTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{

			switch (item)
			{
				case LFSModel _: return LFSTemplate;
			}

			throw new ArgumentException();
		}
	}
}