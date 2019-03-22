using System.IO;
using System.Text;
using DataAccessLibrary;
using JetBrains.Annotations;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using PrivateWiki.Markdig.Extensions.CodeBlockExtension;
using PrivateWiki.Markdig.Extensions.MathExtension;
using StorageProvider;

namespace PrivateWiki.Markdig
{
	public class MarkdigParser : IPageParser, IPageRenderer
	{
		[NotNull] private readonly MarkdownPipeline _pipeline;
		[NotNull] private readonly HtmlRenderer _renderer;

		public MarkdigParser()
		{
			_pipeline = new MarkdownPipelineBuilder()
				.UseAutoIdentifiers()
				.UseAutoLinks()
				.UsePipeTables()
				.UseMediaLinks()
				.UseDiagrams()
				.UseEmphasisExtras()
				.UseMyHtmlCodeBlockRenderer()
				.UseMyMathExtension()
				.Build();

			_renderer = new HtmlRenderer(new StringWriter());
		}

		private string ToHtmlCustom(string markdown)
		{
			var writer = _renderer.Writer as StringWriter;

			_pipeline.Setup(_renderer);

			var document = Parse(markdown);
			_renderer.Render(document);
			writer.Flush();

			var html = writer.ToString();

			return html;
		}

		public MarkdownDocument Parse(string markdown)
		{
			var dom = Markdown.Parse(markdown, _pipeline);
			return dom;
		}

		public MarkdownDocument Parse(PageModel page)
		{
			return Parse(page.Content);
		}

		public string ToHtmlString(string markdown)
		{
			var builder = new StringBuilder();
			builder.AppendLine("<!DOCTYPE html>");
			builder.AppendLine("<html>");
			builder.AppendLine("<head>");

			// Charset = UTF-8
			builder.AppendLine("<meta charset=\"UTF-8\">");

			builder.AppendLine(
				"<script>" +
				"function codeCopyClickFunction() { window.external.notify(\"codeButtonCopy\");}" +
				"</script>");

			// MathJax Script
			builder.AppendLine("<script type=\"text/javascript\">");
			builder.AppendLine(
				"MathJax.Hub.Config({tex2jax: {inlineMath: [['$', '$'], [\"\\(\", \"\\)\"]], processEscapes: true}});");
			builder.AppendLine("</script>");
			builder.AppendLine(
				"<script type=\"text/javascript\" async src=\"https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.5/MathJax.js?config=TeX-MML-AM_CHTML\">");
			builder.AppendLine("MathJax.Hub.Register.StartupHook(\"onLoad\",function () {");
			builder.AppendLine("MathJax.Hub.Config({elements: document.querySelectorAll(\".math\")});");
			builder.AppendLine("});");
			builder.AppendLine("</script>");

			// Mermaid Script
			builder.AppendLine(
				"<script src=\"https://cdnjs.cloudflare.com/ajax/libs/mermaid/7.1.2/mermaid.min.js\"></script>");
			builder.AppendLine("<script>mermaid.initialize({startOnLoad:true});</script>");

			builder.AppendLine("</head>");
			builder.AppendLine("<body>");
			builder.AppendLine(ToHtmlCustom(markdown));
			builder.AppendLine("</body>");
			builder.AppendLine("</html>");
			return builder.ToString();
		}

		public string ToHtmlString(PageModel page)
		{
			return ToHtmlString(page.Content);
		}
	}
}