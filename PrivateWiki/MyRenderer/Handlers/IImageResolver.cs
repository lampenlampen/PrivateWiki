using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace PrivateWiki.Render.Handlers
{
    /// <summary>
    /// An interface used to resolve images in the markdown.
    /// </summary>
    public interface IImageResolver
    {
        /// <summary>
        /// Resolves an Image from a Url.
        /// </summary>
        /// <param name="url">Url to Resolve</param>
        /// <param name="tooltip">Tooltip for Image.</param>
        /// <returns></returns>
        Task<ImageSource> ResolveImageAsync(string url, string tooltip);
    }
}