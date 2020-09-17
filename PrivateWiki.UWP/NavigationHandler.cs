using System;
using System.Collections.Generic;
using NLog;
using PrivateWiki.DataModels.Pages;

#nullable enable

namespace PrivateWiki.UWP
{
	[Obsolete]
	internal static class NavigationHandler
	{
		private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		public static int MaxItems { get; set; } = 4;

		internal static List<Path> Pages { get; set; } = new List<Path>();

		public static bool AddPage(Page page)
		{
			Pages.Remove(page.Path);
			Pages.Add(page.Path);
			Normalize();
			return true;
		}

		private static bool Normalize()
		{
			if (Pages.Count <= MaxItems) return false;

			Pages.RemoveRange(0, Pages.Count - MaxItems);
			Logger.Debug($"Normalize: {Pages.Count - MaxItems}");

			return true;
		}
	}
}