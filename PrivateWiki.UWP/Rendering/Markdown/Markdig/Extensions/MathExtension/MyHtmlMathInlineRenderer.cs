using JetBrains.Annotations;
using Markdig.Extensions.Mathematics;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace PrivateWiki.Rendering.Markdown.Markdig.Extensions.MathExtension
{
	internal class MyHtmlMathInlineRenderer : HtmlObjectRenderer<MathInline>
	{
		protected override void Write([NotNull] HtmlRenderer renderer, [NotNull] MathInline obj)
		{
			renderer.Write("<span").WriteAttributes(obj).Write(">");
			renderer.Write("\\(");
			renderer.WriteEscape(ref obj.Content);
			renderer.Write("\\)");
			renderer.Write("</span>");
		}
	}
}