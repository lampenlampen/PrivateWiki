using System;

#nullable enable

namespace PrivateWiki.Models
{
	public class NavigationItem
	{


	}

	public class DividerItem : NavigationItem
	{

	}

	public class HeaderItem : NavigationItem
	{
		public string? Text { get; set; }
	}

	public class LinkItem : NavigationItem
	{
		public string? Text { get; set; }

		public Guid PageId { get; set; }
	}
}