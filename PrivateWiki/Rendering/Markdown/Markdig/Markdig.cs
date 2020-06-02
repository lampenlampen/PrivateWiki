using System.IO;
using Markdig;
using Markdig.Renderers;

namespace PrivateWiki.Rendering.Markdown.Markdig
{
	internal class Markdig
	{
		private readonly MarkdownPipeline _pipeline;
		private readonly HtmlRenderer _renderer;
		private readonly HtmlBuilder _htmlBuilder;

		public Markdig()
		{
			_htmlBuilder = new HtmlBuilder(new StringWriter());
			_htmlBuilder.WriteHtmlStartTag();
			_htmlBuilder.WriteHeadStartTag();

			_pipeline = MarkdigPipelineBuilder.GetMarkdownPipeline(_htmlBuilder);

			_htmlBuilder.WriteHeadEndTag();

			_renderer = new HtmlRenderer(new StringWriter());
		}

		public string ToHtml(string content)
		{
			var stringWriter = new StringWriter();
			var renderer = new HtmlRenderer(stringWriter);

			_pipeline.Setup(renderer);

			var dom2 = global::Markdig.Markdown.ToHtml(content, stringWriter, _pipeline);

			stringWriter.Flush();

			_htmlBuilder.WriteBodyStartTag();

			_htmlBuilder.WriteHtmlSnippet(stringWriter.ToString());

			_htmlBuilder.WriteBodyEndTag();
			_htmlBuilder.WriteHtmlEndTag();

			_htmlBuilder.Flush();

			return _htmlBuilder.ToString();
		}
	}
}