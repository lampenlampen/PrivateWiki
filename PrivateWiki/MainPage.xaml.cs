using Microsoft.EntityFrameworkCore;
using PrivateWiki.Data;
using PrivateWiki.Parser;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace PrivateWiki
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            initDatabase();


            TestClass();

            EditorFrame.Navigate(typeof(PageViewer), "test");
        }

        private void initDatabase()
        {
            using (var db = new PageContext())
            {
                db.Database.Migrate();
            }
        }

        private void TestClass()
        {
            var markdown2 = @"---
name: testyaml
---

# Header 1

lorem ipsum";

            var parser = new MarkdigParser();

            var dom = parser.Parse(markdown2);

            System.Console.WriteLine("");

        }
    }
}
