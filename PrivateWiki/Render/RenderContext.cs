using Windows.UI.Xaml.Media;
using Parser.Render;

namespace PrivateWiki.Render
{
    /// <summary>
    /// The Context of the Current Position
    /// </summary>
    public abstract class RenderContext : IRenderContext
    {
        /// <summary>
        /// Gets or sets the Foreground of the Current Context.
        /// </summary>
        public Brush Foreground { get; set; }


        /// <inheritdoc />
        public object Parent { get; set; }
        
        /// <summary>
        /// Indicates whether to override the Foreground Property.
        /// </summary>
        public bool OverrideForeground { get; set; }


        /// <inheritdoc />
        public IRenderContext Clone()
        {
            return (IRenderContext) MemberwiseClone();
        }
    }
}