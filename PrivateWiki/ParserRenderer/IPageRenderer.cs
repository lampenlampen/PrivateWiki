using JetBrains.Annotations;
using StorageProvider;

namespace PrivateWiki.ParserRenderer
{
    interface IPageRenderer
    {
        string ToHtmlString([NotNull] ContentPage page);
    }
}
