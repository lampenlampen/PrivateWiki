using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Parser;
using PrivateWiki.Render;
using Windows.UI.Text;
using Windows.UI;
using System;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Editor : Page
    {
        public Editor()
        {
            InitializeComponent();

            ShowContent();
        }

        public void ShowContent()
        {
            var textParagraph =
                @"Sankt Petersburg (russisch Санкт-Петербург Sankt-Peterburg; kurz auch St. Petersburg, örtlicher Spitzname Piter nach der ursprünglich dem Niederländischen nachempfundenen Namensform Санкт-Питербурх Sankt-Piterburch) ist mit fünf Millionen Einwohnern (2012)[2] die nach Moskau zweitgrößte Stadt Russlands und die viertgrößte Europas.
Sankt Petersburg liegt im Nordwesten des Landes an der Mündung der Newa in die Newabucht am Ostende des Finnischen Meerbusens der Ostsee und ist die nördlichste Millionenstadt der Welt. Die Stadt wurde 1703 von Peter dem Großen auf Sumpfgelände nahe dem Meer gegründet, um den Anspruch Russlands auf Zugang zur Ostsee durchzusetzen. Über 200 Jahre lang trug sie den heutigen Namen, von 1914 bis 1924 hieß sie Petrograd(Петроград), und sie wurde von 1924 bis 1991 zu Ehren von Lenin, dem Gründer der Sowjetunion, Leningrad(Ленинград) genannt(die drei Namen sowie den im russischen Alltagssprachgebrauch häufig verwendeten Kurznamen der Stadt, Piter, anhören ?/ i).
Die Stadt war vom 18.bis ins 20.Jahrhundert die Hauptstadt des Russischen Kaiserreiches, ist ein europaweit wichtiges Kulturzentrum und beherbergt den wichtigsten russischen Ostseehafen.
Die historische Innenstadt mit 2.300 Palästen, Prunkbauten und Schlössern ist seit 1991 als Weltkulturerbe der UNESCO unter dem Sammelbegriff Historic Centre of Saint Petersburg and Related Groups of Monuments eingetragen.[3] [4] In dieser Vielfalt ist St.Petersburg weltweit nur noch mit Venedig vergleichbar.
Mit dem 462 Meter hohen Lakhta Center befindet sich das höchste Gebäude Europas in der Stadt.";

            var loremIpsum =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse facilisis varius enim, eu suscipit orci pulvinar ac. Vestibulum condimentum nec mi laoreet aliquet. Morbi eu convallis metus. Pellentesque nec ligula non risus euismod elementum non eu augue. In nec magna consectetur enim congue auctor condimentum at felis. Donec et massa orci. Nullam venenatis cursus risus at maximus. Vivamus tempus tellus nec felis consectetur, sed aliquam magna dictum. Nulla in mi et nisl tempor ornare. Vestibulum ac consequat arcu, nec ultrices diam. Cras vitae felis convallis, auctor risus et, egestas augue.";

            var markup =
                $"# Header 1\r\n> Hallo\r\n> Hallo\r\n> > huhu\r\n{loremIpsum}\r\n\r\n## Header2\r\n{loremIpsum}\r\n\r\n### Header3\r\n\r\n{loremIpsum}\r\n\r\n#### Header 4\r\n\r\n{loremIpsum} \r\n\r\n##### Header 5\r\n{loremIpsum}\r\n\r\n---\r\n\r\nHallo wie geht es dir?\r\n\r\n```\r\npublic class TestClass\r\n{{\r\n}}\r\n```\r\n\r\n{textParagraph}";

            var doc = Document.Parse(markup);
            Console.WriteLine(doc);

            var renderer = CreateRenderer(doc);

            var element = renderer.Render();

            ScrollViewer.Content = element;
        }

        private MarkupRenderer CreateRenderer(Document doc)
        {
            var renderer = new MarkupRenderer(doc, null, null, null)
            {
                Background = Background,
                //BorderBrush = BorderBrush,
                //BorderThickness = BorderThickness,
                CharacterSpacing = CharacterSpacing,
                FontFamily = FontFamily,
                FontSize = FontSize,
                FontStretch = FontStretch,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                Foreground = Foreground,
                IsTextSelectionEnabled = true,
                Padding = Padding,
                //CodeBackground = new SolidColorBrush(convertHexToColor("#FFF6F8FA")),
                CodeBackground = new SolidColorBrush(Color.FromArgb(250, 246, 248, 250)),
                CodeBorderBrush = new SolidColorBrush(Color.FromArgb(250, 190, 190, 190)),
                CodeBorderThickness = new Thickness(1, 1, 1, 1),
                //InlineCodeBorderThickness = InlineCodeBorderThickness,
                //InlineCodeBackground = InlineCodeBackground,
                //InlineCodeBorderBrush = InlineCodeBorderBrush,
                //InlineCodePadding = InlineCodePadding,
                //InlineCodeFontFamily = InlineCodeFontFamily,
                //CodeForeground = CodeForeground,
                CodeFontFamily = new FontFamily("Consolas"),
                CodePadding = new Thickness(20, 15, 20, 15),
                CodeMargin = new Thickness(0, 15, 0, 15),
                //EmojiFontFamily = new FontFamily("Segoe UI Emoji"),
                Header1FontSize = 50,
                Header1FontWeight = FontWeights.Medium,
                Header1Margin = new Thickness(0, 15, 0, 15),
                Header1Foreground = new SolidColorBrush(Colors.Black),
                Header2FontSize = 40,
                Header2FontWeight = FontWeights.Medium,
                Header2Margin = new Thickness(0, 15, 0, 15),
                Header2Foreground = new SolidColorBrush(Colors.Black),
                Header3FontSize = 35,
                Header3FontWeight = FontWeights.Normal,
                Header3Margin = new Thickness(0, 10, 0, 10),
                Header3Foreground = new SolidColorBrush(Colors.Black),
                Header4FontSize = 25,
                Header4FontWeight = FontWeights.Normal,
                Header4Margin = new Thickness(0, 10, 0, 10),
                Header4Foreground = new SolidColorBrush(Colors.Black),
                Header5FontSize = 20,
                Header5FontWeight = FontWeights.Normal,
                Header5Margin = new Thickness(0, 10, 0, 5),
                Header5Foreground = new SolidColorBrush(Colors.Black),
                HorizontalRuleBrush = new SolidColorBrush(Color.FromArgb(250, 190, 190, 190)),
                HorizontalRuleMargin = new Thickness(0, 20, 0, 20),
                HorizontalRuleThickness = 2,
                //ListMargin = ListMargin,
                //ListGutterWidth = ListGutterWidth,
                //ListBulletSpacing = ListBulletSpacing,
                ParagraphMargin = new Thickness(0, 5, 0, 5),
                QuoteBackground = new SolidColorBrush(Color.FromArgb(10, 0, 0, 0)),
                QuoteBorderBrush = new SolidColorBrush(Color.FromArgb(250, 190, 190, 190)),
                QuoteBorderThickness = new Thickness(3, 0, 0, 0),
                QuoteForeground = new SolidColorBrush(Color.FromArgb(255, 110, 116, 124)),
                QuoteMargin = new Thickness(0, 15, 0, 15),
                QuotePadding = new Thickness(15, 10, 15, 10),
                //TableBorderBrush = TableBorderBrush,
                //TableBorderThickness = TableBorderThickness,
                //TableCellPadding = TableCellPadding,
                //TableMargin = TableMargin,
                TextWrapping = TextWrapping.WrapWholeWords,
                //LinkForeground = LinkForeground,
                //ImageStretch = ImageStretch,
                //ImageMaxHeight = ImageMaxHeight,
                //ImageMaxWidth = ImageMaxWidth,
                //WrapCodeBlock = WrapCodeBlock
            };

            return renderer;
        }
    }
}