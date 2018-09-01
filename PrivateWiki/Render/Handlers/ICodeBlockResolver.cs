using Windows.UI.Xaml.Documents;

namespace PrivateWiki.Render.Handlers
{
    /// <summary>
    /// A Parser to parse code strings into syntax highlighted text.
    /// </summary>
    public interface ICodeBlockResolver
    {
        /// <summary>
        /// Parses Code Block Text into Rich text.
        /// </summary>
        /// <param name="inlineCollection">Block to add formatted text to.</param>
        /// <param name="text">The raw code block text</param>
        /// <param name="codeLanguage">The language of the code block.</param>
        /// <returns></returns>
        bool ParseSyntax(InlineCollection inlineCollection, string text, string codeLanguage);
    }
}