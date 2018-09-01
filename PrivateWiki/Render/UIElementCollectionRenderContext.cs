using Windows.UI.Xaml.Controls;
using Parser.Render;

namespace PrivateWiki.Render
{
    /// <summary>
    /// The Context of the Current Document Rendering.
    /// </summary>
    public class UIElementCollectionRenderContext : RenderContext
    {
        /// <summary>
        /// Gets or sets the list to add to.
        /// </summary>
        public UIElementCollection BlockUiElementCollection { get; set; }

        internal UIElementCollectionRenderContext(UIElementCollection blockUiElementCollection)
        {
            BlockUiElementCollection = blockUiElementCollection;
        }

        internal UIElementCollectionRenderContext(UIElementCollection blockUiElementCollection, IRenderContext context) : this(blockUiElementCollection)
        {
            Parent = context.Parent;

            if (context is RenderContext localContext)
            {
                Foreground = localContext.Foreground;
                OverrideForeground = localContext.OverrideForeground;
            }
        }
    }
}