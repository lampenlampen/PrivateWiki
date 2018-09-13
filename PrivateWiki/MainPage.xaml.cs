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
            TestClass();

            EditorFrame.Navigate(typeof(PageEditor), "1");
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
