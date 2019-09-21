using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Models.Pages;

#nullable enable

namespace PrivateWiki
{
	internal static class NavigationHandler
	{
		public static int MaxItems { get; set; } = 4;

		internal static List<string> Pages { get; set; } = new List<string>();

		public static bool AddPage( MarkdownPage page)
		{
			Pages.Remove(page.Link);
			Pages.Add(page.Link);
			Normalize();
			return true;
		}

		public static bool RemovePage(MarkdownPage page)
		{
			Pages.Remove(page.Link);

			return true;
		}

		private static bool Normalize()
		{
			if (Pages.Count() <= MaxItems) return false;

			Pages.RemoveRange(0, Pages.Count - MaxItems);
			Debug.WriteLine($"Normalize: {Pages.Count - MaxItems}");

			return true;
		}
	}
}