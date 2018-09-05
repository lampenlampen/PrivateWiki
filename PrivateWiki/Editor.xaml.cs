using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Parser;
using PrivateWiki.Render;
using Windows.UI.Text;
using Windows.UI;

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
            var markup = "# Header 1\r\n---\r\nHallo wie geht es dir?\r\n\r\n> huhu\r\n> haha";
            var doc = Document.Parse(markup);

            var renderer = createRenderer(doc);

            var element = renderer.Render();

            ScrollViewer.Content = element;
            
        }

        private MarkupRenderer createRenderer(Document doc)
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
                //CodeBackground = CodeBackground,
                //CodeBorderBrush = CodeBorderBrush,
                //CodeBorderThickness = CodeBorderThickness,
                //InlineCodeBorderThickness = InlineCodeBorderThickness,
                //InlineCodeBackground = InlineCodeBackground,
                //InlineCodeBorderBrush = InlineCodeBorderBrush,
                //InlineCodePadding = InlineCodePadding,
                //InlineCodeFontFamily = InlineCodeFontFamily,
                //CodeForeground = CodeForeground,
                //CodeFontFamily = CodeFontFamily,
                //CodePadding = CodePadding,
                //CodeMargin = CodeMargin,
                //EmojiFontFamily = EmojiFontFamily,
                Header1FontSize = 20,
                Header1FontWeight = FontWeights.Normal,
                Header1Margin = new Thickness(0,15,0,15),
                Header1Foreground = new SolidColorBrush(Colors.Black),
                Header2FontSize = 20,
                Header2FontWeight = FontWeights.Normal,
                Header2Margin = new Thickness(0,15,0,15),
                Header2Foreground = new SolidColorBrush(Colors.Black),
                Header3FontSize = 17,
                Header3FontWeight = FontWeights.Bold,
                Header3Margin = new Thickness(0,10,0,10),
                Header3Foreground = new SolidColorBrush(Colors.Black),
                Header4FontSize = 17,
                Header4FontWeight = FontWeights.Normal,
                Header4Margin = new Thickness(0,10,0,10),
                Header4Foreground = new SolidColorBrush(Colors.Black),
                Header5FontSize = 15,
                Header5FontWeight = FontWeights.Bold,
                Header5Margin = new Thickness(0,10,0,5),
                Header5Foreground = new SolidColorBrush(Colors.Black),
                HorizontalRuleBrush = new SolidColorBrush((Color)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Color), "#FFBEBEBE")),
                HorizontalRuleMargin = new Thickness(0,7,0,7),
                HorizontalRuleThickness = 2,
                //ListMargin = ListMargin,
                //ListGutterWidth = ListGutterWidth,
                //ListBulletSpacing = ListBulletSpacing,
                //ParagraphMargin = ParagraphMargin,
                //QuoteBackground = (Brush) Resources["Transparent"],
                //QuoteBorderBrush = QuoteBorderBrush,
                //QuoteBorderThickness = QuoteBorderThickness,
                //QuoteForeground = QuoteForeground,
                //QuoteMargin = QuoteMargin,
                //QuotePadding = QuotePadding,
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
