using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace PrivateWiki.Render.Handlers
{
    /// <summary>
    /// An Interface used to handle links in the markup.
    /// </summary>
    public interface ILinkRegister
    {
        /// <summary>
        /// Registers a Hyperlink with a LinkUrl.
        /// </summary>
        /// <param name="newHyperLink">Hyperlink to Register</param>
        /// <param name="url">Url to Register</param>
        void RegisterNewHyperLink(Hyperlink newHyperLink, string url);

        /// <summary>
        /// Registers a Hyperlink with a LinkUrl.
        /// </summary>
        /// <param name="newImageLink">ImageLink to Register.</param>
        /// <param name="linkUrl">Url to Register.</param>
        /// <param name="isHyperLink">Is Image an Hyperlink.</param>
        void RegisterNewHyperLink(Image newImageLink, string linkUrl, bool isHyperLink);
    }
}