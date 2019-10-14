using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Models.Pages;
using NLog;

#nullable enable

namespace PrivateWiki
{
	internal static class NavigationHandler
	{
		private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
			Logger.Debug($"Normalize: {Pages.Count - MaxItems}");

			return true;
		}
	}
}