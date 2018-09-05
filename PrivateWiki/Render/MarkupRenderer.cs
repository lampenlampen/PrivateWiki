using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render;
using Parser;
using Parser.Blocks;
using Parser.Blocks.List;
using Parser.Render;
using TextBlock = Parser.Blocks.TextBlock;

namespace PrivateWiki.Render
{
    public class MarkupRenderer : BaseRenderer
    {
        /// <summary>
        /// Gets or sets the Root Framwork Element.
        /// </summary>
        private FrameworkElement RootElement { get; set; }

        /// <summary>
        /// Gets the interface that is used to register hyperlinks.
        /// </summary>
        protected ILinkRegister LinkRegister { get; }

        /// <summary>
        /// Gets the interface that is used to resolve images.
        /// </summary>
        protected IImageResolver ImageResolver { get; }

        /// <summary>
        /// Gets the Parser to parse code strings into syntax highlighted text.
        /// </summary>
        protected ICodeBlockResolver CodeBlockResolver { get; }

        /// <summary>
        /// Gets the Default Emoji Font.
        /// </summary>
        /// <returns></returns>
        public FontFamily DefaultEmojiFont { get; }

        /// <summary>
        /// Gets or sets a brush that provides the background of the control.
        /// </summary>
        public Brush Background { get; set; }

        /// <summary>
        /// Gets or sets a brush that desribes the border fill of a control.
        /// </summary>
        public Brush BorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the font used to display test in the control.
        /// </summary>
        public FontFamily FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the style in which the text is rendered.
        /// </summary>
        public FontStyle FontStyle { get; set; }

        /// <summary>
        /// Gets ot sets the thickness of the specified text.
        /// </summary>
        public FontWeight FontWeight { get; set; }

        /// <summary>
        /// Gets or sets a brush that describes the foreground color.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        /// Indicates whether text selection is enabled.
        /// </summary>
        public bool IsTextSelectionEnabled { get; set; }

        /// <summary>
        /// Gets or sets the brush used to fill the background of a code block.
        /// </summary>
        public Brush CodeBackground { get; set; }

        /// <summary>
        /// Gets or sets the brush used to fill the background of inline code.
        /// </summary>
        public Brush InlineCodeBackground { get; set; }

        /// <summary>
        /// Gets or sets the brush used to fill the border of inline code.
        /// </summary>
        public Brush InlineCodeBorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render the border fill of a code block.
        /// </summary>
        public Brush CodeBorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render the text inside a code block. If this is <c>null</c>, then <see cref="Foreground"/> is used.
        /// </summary>
        public Brush CodeForeground { get; set; }

        /// <summary>
        /// Gets or sets the font used to display code. If this is <c>null</c>, then <see cref="FontFamily"/> is used.
        /// </summary>
        public FontFamily CodeFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font used to display code. If this is <c>null</c>, then <see cref="FontFamily"/> is used.
        /// </summary>
        public FontFamily InlineCodeFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font used to display emojis. If this is <c>null</c>, then Segoe UI Emoji font is used.
        /// </summary>
        public FontFamily EmojiFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 1 headers.
        /// </summary>
        public FontWeight Header1FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 1 headers.
        /// </summary>
        public Brush Header1Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 2 headers.
        /// </summary>
        public FontWeight Header2FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 2 headers.
        /// </summary>
        public Brush Header2Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 3 headers.
        /// </summary>
        public FontWeight Header3FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 3 headers.
        /// </summary>
        public Brush Header3Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 4 headers.
        /// </summary>
        public FontWeight Header4FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 4 headers.
        /// </summary>
        public Brush Header4Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 5 headers.
        /// </summary>
        public FontWeight Header5FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 5 headers.
        /// </summary>
        public Brush Header5Foreground { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render a horizontal rule. If this is <c>null</c>, then <see cref="Foreground"/> is used.
        /// </summary>
        public Brush HorizontalRuleBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush used to fill the background of a quote block.
        /// </summary>
        /// 
        public Brush QuoteBackground { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render a quote block. If this is <c>null</c>, then <see cref="Foreground"/> is used.
        /// </summary>
        public Brush QuoteBorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render the text inside a quote block. If this is <c>null</c>, then <see cref="Foreground"/> is used.
        /// </summary>
        public Brush QuoteForeground { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render the rable borders. If this is <c>null</c>, then <see cref="Foreground"/> is used.
        /// </summary>
        public Brush TableBorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render links. If this is <c>null</c>, then <see cref="Foreground"/> is used.
        /// </summary>
        public Brush LinkForeground { get; set; }

        /// <summary>
        /// Gets or sets the distance between the border and its child object.
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// Gets or sets the border thickness of a control.
        /// </summary>
        public Thickness BorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the border around code blocks.
        /// </summary>
        public Thickness CodeBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the border around inline code.
        /// </summary>
        public Thickness InlineCodeBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the space outside of code blocks.
        /// </summary>
        public Thickness CodeMargin { get; set; }

        /// <summary>
        /// Gets or sets the space between the code border and the text.
        /// </summary>
        public Thickness CodePadding { get; set; }

        /// <summary>
        /// Gets or sets the space between the inline code border and the text.
        /// </summary>
        public Thickness InlineCodePadding { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 1 headers.
        /// </summary>
        public double Header1FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 1 headers.
        /// </summary>
        public Thickness Header1Margin { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 2 headers.
        /// </summary>
        public double Header2FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 2 headers.
        /// </summary>
        public Thickness Header2Margin { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 3 headers.
        /// </summary>
        public double Header3FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 3 headers.
        /// </summary>
        public Thickness Header3Margin { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 4 headers.
        /// </summary>
        public double Header4FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 4 headers.
        /// </summary>
        public Thickness Header4Margin { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 5 headers.
        /// </summary>
        public double Header5FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 5 headers.
        /// </summary>
        public Thickness Header5Margin { get; set; }

        /// <summary>
        /// Gets or sets the margin used for horizontal rules.
        /// </summary>
        public Thickness HorizontalRuleMargin { get; set; }
        /// <summary>
        /// Gets or sets the vertical thickness of the horizontal rule.
        /// </summary>
        public double HorizontalRuleThickness { get; set; }

        /// <summary>
        /// Gets or sets the margin used by lists.
        /// </summary>
        public Thickness ListMargin { get; set; }

        /// <summary>
        /// Gets or sets the width of the space used by list item bullets/numbers.
        /// </summary>
        public double ListGutterWidth { get; set; }

        /// <summary>
        /// Gets or sets the space between the list item bullets/numbers and the list item content.
        /// </summary>
        public double ListBulletSpacing { get; set; }

        /// <summary>
        /// Gets or sets the margin used for paragraphs.
        /// </summary>
        public Thickness ParagraphMargin { get; set; }

        /// <summary>
        /// Gets or sets the thickness of quote borders.
        /// </summary>
        public Thickness QuoteBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the space outside of quote borders.
        /// </summary>
        public Thickness QuoteMargin { get; set; }

        /// <summary>
        /// Gets or sets the space between the quote border and the text.
        /// </summary>
        public Thickness QuotePadding { get; set; }

        /// <summary>
        /// Gets or sets the thickness of any table borders.
        /// </summary>
        public double TableBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the padding inside each cell.
        /// </summary>
        public Thickness TableCellPadding { get; set; }

        /// <summary>
        /// Gets or sets the margin used by tables.
        /// </summary>
        public Thickness TableMargin { get; set; }

        /// <summary>
        /// Gets or sets the size of the text in this control.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the uniform spacing between characters, in units of 1/1000 of an em.
        /// </summary>
        public int CharacterSpacing { get; set; }

        /// <summary>
        /// Gets or sets the wird wrapping behavior.
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        /// <summary>
        /// Gets or sets the degree to which a font is condensed or expanded on the screen.
        /// </summary>
        public FontStretch FontStretch { get; set; }

        /// <summary>
        /// Gets or sets the stretch used for images.
        /// </summary>
        public Stretch ImageStretch { get; set; }

        /// <summary>
        /// Gets or sets the MaxHeight for images.
        /// </summary>
        public double ImageMaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the MaxWidth for images.
        /// </summary>
        public double ImageMaxWidth { get; set; }

        /// <summary>
        /// Indicating whether to wrapp tet in the Code Block, or use Horizontal Scroll.
        /// </summary>
        public bool WrapCodeBlock { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Render"/> class.
        /// </summary>
        /// <param name="doc">The Document to Render.</param>
        /// <param name="linkRegister">The LinkRegister</param>
        /// <param name="imageResolver">The Image Resolver</param>
        /// <param name="codeBlockResolver">The Code Block Resolver</param>
        public MarkupRenderer(Document doc, ILinkRegister linkRegister, IImageResolver imageResolver,
            ICodeBlockResolver codeBlockResolver) : base(doc)
        {
            LinkRegister = linkRegister;
            ImageResolver = imageResolver;
            CodeBlockResolver = codeBlockResolver;
            DefaultEmojiFont = new FontFamily("Segoe UI Emoji");
        }

        /// <summary>
        /// Called externally to render the markupu to a text block.
        /// </summary>
        /// <returns>A XAML UI Element.</returns>
        public UIElement Render()
        {
            var stackPanel = new StackPanel();
            RootElement = stackPanel;
            Render(new UIElementCollectionRenderContext(stackPanel.Children) {Foreground = Foreground});

            // Set background and border properties
            stackPanel.Background = Background;
            stackPanel.BorderBrush = BorderBrush;
            stackPanel.BorderThickness = BorderThickness;
            stackPanel.Padding = Padding;

            return stackPanel;
        }

        /// <summary>
        /// Creates a new RichTextBlock, if last element of the provided collection isn't already a RichTextBlock.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected RichTextBlock CreateOrReuseRichTextBlock(IRenderContext context)
        {
            if (!(context is UIElementCollectionRenderContext localContext))
            {
                throw new RenderContextIncorectException();
            }

            var blockUIElementCollection = localContext.BlockUiElementCollection;

            // Reuse the last RichTextBlock, if possible
            if (blockUIElementCollection != null && blockUIElementCollection.Count > 0 &&
                blockUIElementCollection.Last() is RichTextBlock)
            {
                return (RichTextBlock) blockUIElementCollection.Last();
            }

            var result = new RichTextBlock
            {
                CharacterSpacing = CharacterSpacing,
                FontFamily = FontFamily,
                FontSize = FontSize,
                FontStretch = FontStretch,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                Foreground = Foreground,
                IsTextSelectionEnabled = IsTextSelectionEnabled,
                TextWrapping = TextWrapping
            };

            localContext.BlockUiElementCollection.Add(result);

            return result;
        }

        /// <summary>
        /// Renders a TextBlock.
        /// </summary>
        protected override void RenderTextBlock(TextBlock block, IRenderContext context)
        {
            // TODO Render Inlines

            var paragraph = new Paragraph
            {
                Margin = ParagraphMargin
            };
            paragraph.Inlines.Add(new Run {Text = block.Text});

            var richTextBlock = CreateOrReuseRichTextBlock(context);
            richTextBlock.Blocks.Add(paragraph);
        }

        /// <summary>
        /// Renders a quote block.
        /// </summary>
        protected override void RenderQuoteBlock(QuoteBlock block, IRenderContext context)
        {
            if (!(context is UIElementCollectionRenderContext localContext))
            {
                throw new RenderContextIncorectException();
            }

            var blockUIElementCollection = localContext.BlockUiElementCollection;

            var stackPanel = new StackPanel();
            var childContext = new UIElementCollectionRenderContext(stackPanel.Children, context)
            {
                Parent = stackPanel
            };

            if (QuoteForeground != null && !localContext.OverrideForeground)
            {
                childContext.Foreground = QuoteForeground;
            }
            
            RenderBlocks(block.Blocks, childContext);

            var border = new Border
            {
                Margin = QuoteMargin,
                Background = QuoteBackground,
                BorderBrush = childContext.OverrideForeground
                    ? childContext.Foreground
                    : QuoteBorderBrush ?? childContext.Foreground,
                BorderThickness = QuoteBorderThickness,
                Padding = QuotePadding,
                Child = stackPanel
            };
            blockUIElementCollection.Add(border);
        }

        /// <summary>
        /// Renders a code block.
        /// </summary>
        protected override void RenderCodeBlock(CodeBlock block, IRenderContext context)
        {
            if (!(context is UIElementCollectionRenderContext localContext))
            {
                throw new RenderContextIncorectException();
            }

            var blockUIElementCollection = localContext.BlockUiElementCollection;

            var foreground = localContext.Foreground;
            if (CodeForeground != null && !localContext.OverrideForeground)
            {
                foreground = CodeForeground;
            }

            var richTextBlock = new RichTextBlock
            {
                FontFamily = CodeFontFamily ?? FontFamily,
                Foreground = foreground,
                LineHeight = FontSize * 1.4
            };
            
            var paragraph = new Paragraph();
            richTextBlock.Blocks.Add(paragraph);
            
            // Allows external Syntax Highlighting
            var hasCustomSyntax = CodeBlockResolver.ParseSyntax(paragraph.Inlines, block.Text, block.CodeLanguage);

            if (!hasCustomSyntax)
            {
                paragraph.Inlines.Add(new Run {Text = block.Text});
            }
            
            // Ensures that Code has Horizontal Scroll and doesn't wrap.
            var viewer = new ScrollViewer
            {
                Background = CodeBackground,
                BorderBrush = CodeBorderBrush,
                BorderThickness = BorderThickness,
                Padding = CodePadding,
                Margin = CodeMargin,
                Content = richTextBlock
            };

            if (!WrapCodeBlock)
            {
                viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                viewer.HorizontalScrollMode = ScrollMode.Auto;
                viewer.VerticalScrollMode = ScrollMode.Disabled;
                viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
            
            blockUIElementCollection.Add(viewer);
        }

        /// <summary>
        /// Renders a header element.
        /// </summary>
        protected override void RenderHeaderBlock(HeaderBlock block, IRenderContext context)
        {
            var richTextBlock = CreateOrReuseRichTextBlock(context);

            var paragraph = new Paragraph();
            var childInlines = paragraph.Inlines;

            switch (block.Level)
            {
                case 1:
                    paragraph.Margin = Header1Margin;
                    paragraph.FontSize = Header1FontSize;
                    paragraph.FontWeight = Header1FontWeight;
                    paragraph.Foreground = Header1Foreground;
                    break;
                case 2:
                    paragraph.Margin = Header2Margin;
                    paragraph.FontSize = Header2FontSize;
                    paragraph.FontWeight = Header2FontWeight;
                    paragraph.Foreground = Header2Foreground;
                    break;
                case 3:
                    paragraph.Margin = Header3Margin;
                    paragraph.FontSize = Header3FontSize;
                    paragraph.FontWeight = Header3FontWeight;
                    paragraph.Foreground = Header3Foreground;
                    break;
                case 4:
                    paragraph.Margin = Header4Margin;
                    paragraph.FontSize = Header4FontSize;
                    paragraph.FontWeight = Header4FontWeight;
                    paragraph.Foreground = Header4Foreground;
                    break;
                case 5:
                    paragraph.Margin = Header5Margin;
                    paragraph.FontSize = Header5FontSize;
                    paragraph.FontWeight = Header5FontWeight;
                    paragraph.Foreground = Header5Foreground;
                    
                    var underline = new Underline();
                    childInlines = underline.Inlines;
                    paragraph.Inlines.Add(underline);
                    break;
            }

            var run = new Run() { Text = block.HeadingText };
            paragraph.Inlines.Add(run);
            
            richTextBlock.Blocks.Add(paragraph);
            
        }

        protected override void RenderListBlock(ListBlock block, IRenderContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void RenderListItem(ListElement block, IRenderContext context)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Renders a horizontal rule.
        /// </summary>
        protected override void RenderHorizontalRuleBlock(HorizontalRuleBlock block, IRenderContext context)
        {
            if (!(context is UIElementCollectionRenderContext localContext))
            {
                throw new RenderContextIncorectException();
            }

            var blockUIElementCollection = localContext.BlockUiElementCollection;

            var foreground = localContext.Foreground;

            if (HorizontalRuleBrush != null && !localContext.OverrideForeground)
            {
                foreground = HorizontalRuleBrush;
            }

            var rectangle = new Rectangle
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = HorizontalRuleThickness,
                Fill = foreground,
                Margin = HorizontalRuleMargin
            };
            
            blockUIElementCollection.Add(rectangle);
        }

        protected override void RenderTableBlock(TableBlock block, IRenderContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void RenderMathBlock(MathBlock block, IRenderContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}