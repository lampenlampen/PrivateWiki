using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LanguageExt;
using StorageProvider;

namespace PrivateWiki
{
    internal static class NavigationHandler
    {
        public static int MaxItems { get; set; } = 4;

        internal static List<string> Pages { get; set; } = new List<string>();

        public static bool AddPage([NotNull] ContentPage page)
        {
            Pages.Remove(page.Id);
            Pages.Add(page.Id);
            Normalize();
            return true;
        }

        public static bool RemovePage([NotNull] ContentPage page)
        {
            Pages.Remove(page.Id);

            return true;
        }

        private static bool Normalize()
        {
            if (Pages.Count() <= MaxItems) return false;

            Pages.RemoveRange(0, Pages.Count - MaxItems);
            Debug.WriteLine($"Normalize: {Pages.Count-MaxItems}");

            return true;
        }
    }
}
