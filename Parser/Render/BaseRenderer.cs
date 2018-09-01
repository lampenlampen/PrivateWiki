using System;
using System.Collections.Generic;
using Parser.Blocks;
using Parser.Blocks.List;

namespace Parser.Render
{
    public abstract class BaseRenderer
    {
        /// <summary>
        /// Gets the document that will be rendered.
        /// </summary>
        protected Document doc { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRenderer"/> class.
        /// </summary>
        /// <param name="doc">Markup Document to Render.</param>
        public BaseRenderer(Document doc)
        {
            this.doc = doc;
        }

        /// <summary>
        /// Renders all Content to the Provided Parent UI.
        /// </summary>
        /// <param name="context">UI Context</param>
        public virtual void Render(IRenderContext context)
        {
            RenderBlocks(doc.Dom, context);
        }

        /// <summary>
        /// Renders a list of block elements.
        /// </summary>
        protected virtual void RenderBlocks(IList<Block> blocks, IRenderContext context)
        {
            foreach (var block in blocks)
            {
                RenderBlock(block, context);
            }
        }

        /// <summary>
        /// Called to render a block element.
        /// </summary>
        protected void RenderBlock(Block block, IRenderContext context)
        {
            switch (block.Type)
            {
                case BlockType.Root:
                    break;
                case BlockType.TextBlock:
                    RenderTextBlock((TextBlock) block, context);
                    break;
                case BlockType.QuoteBlock:
                    RenderQuoteBlock((QuoteBlock) block, context);
                    break;
                case BlockType.CodeBlock:
                    RenderCodeBlock((CodeBlock) block, context);
                    break;
                case BlockType.HeaderBlock:
                    RenderHeaderBlock((HeaderBlock) block, context);
                    break;
                case BlockType.ListBlock:
                    RenderListBlock((ListBlock) block, context);
                    break;
                case BlockType.ListItemBlock:
                    RenderListItem((ListElement) block, context);
                    break;
                case BlockType.HorizontalRuleBlock:
                    RenderHorizontalRuleBlock((HorizontalRuleBlock) block, context);
                    break;
                case BlockType.TableBlock:
                    RenderTableBlock((TableBlock) block, context);
                    break;
                case BlockType.MathBlock:
                    RenderMathBlock((MathBlock) block, context);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Renders a text block.
        /// </summary>
        protected abstract void RenderTextBlock(TextBlock block, IRenderContext context);

        /// <summary>
        /// Render a Quote block.
        /// </summary>
        protected abstract void RenderQuoteBlock(QuoteBlock block, IRenderContext context);

        /// <summary>
        /// Render a Code Block.
        /// </summary>
        protected abstract void RenderCodeBlock(CodeBlock block, IRenderContext context);

        /// <summary>
        /// Render a Header Block.
        /// </summary>
        protected abstract void RenderHeaderBlock(HeaderBlock block, IRenderContext context);

        /// <summary>
        /// Renders a ListBlock.
        /// </summary>
        protected abstract void RenderListBlock(ListBlock block, IRenderContext context);

        /// <summary>
        /// Renders a ListElement.
        /// </summary>
        protected abstract void RenderListItem(ListElement block, IRenderContext context);

        /// <summary>
        /// Renders a HorizontalRule block.
        /// </summary>
        protected abstract void RenderHorizontalRuleBlock(HorizontalRuleBlock block, IRenderContext context);

        /// <summary>
        /// Renders a Table block.
        /// </summary>
        protected abstract void RenderTableBlock(TableBlock block, IRenderContext context);

        /// <summary>
        /// Renders a Math block.
        /// </summary>
        protected abstract void RenderMathBlock(MathBlock block, IRenderContext context);
    }
}