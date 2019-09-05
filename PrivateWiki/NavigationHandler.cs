using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using StorageBackend;

namespace PrivateWiki
{
	internal static class NavigationHandler
	{
		public static int MaxItems { get; set; } = 4;

		internal static List<string> Pages { get; set; } = new List<string>();

		public static bool AddPage([NotNull] PageModel page)
		{
			Pages.Remove(page.Link);
			Pages.Add(page.Link);
			Normalize();
			return true;
		}

		public static bool RemovePage([NotNull] PageModel page)
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