using JetBrains.Annotations;
using Markdig.Syntax;
using StorageProvider;

namespace PrivateWiki.ParserRenderer
{
    interface IPageParser
    {
        MarkdownDocument Parse([NotNull] ContentPage page);

    }
}
