using System.IO;
using System.Threading.Tasks;
using Markdig;
using Markdig.Renderers;

namespace PrivateWiki.Rendering.Markdown.Markdig
{
	public class Markdig
	{
		private readonly HtmlRenderer _renderer;
		private readonly HtmlBuilder _htmlBuilder;

		private readonly Task<MarkdownPipeline> _pipelineTask;

		public Markdig(MarkdigPipelineBuilder pipelineBuilder)
		{
			_htmlBuilder = new HtmlBuilder(new StringWriter());
			_htmlBuilder.WriteHtmlStartTag();
			_htmlBuilder.WriteHeadStartTag();

			_pipelineTask = pipelineBuilder.GetMarkdownPipeline(_htmlBuilder);

			_htmlBuilder.WriteHeadEndTag();

			_renderer = new HtmlRenderer(new StringWriter());
		}

		public async Task<string> ToHtmlAsync(string content)
		{
			var stringWriter = new StringWriter();
			var renderer = new HtmlRenderer(stringWriter);

			var pipeline = await _pipelineTask;

			pipeline.Setup(renderer);

			var dom2 = global::Markdig.Markdown.ToHtml(content, stringWriter, pipeline);

			stringWriter.Flush();

			_htmlBuilder.WriteBodyStartTag();

			_htmlBuilder.WriteHtmlSnippet(stringWriter.ToString());

			_htmlBuilder.WriteBodyEndTag();
			_htmlBuilder.WriteHtmlEndTag();

			_htmlBuilder.Flush();

			return _htmlBuilder.ToString();
		}

		public string ToHtml(string content)
		{
			var stringWriter = new StringWriter();
			var renderer = new HtmlRenderer(stringWriter);

			var pipeline = await _pipelineTask;

			pipeline.Setup(renderer);

			var dom2 = global::Markdig.Markdown.ToHtml(content, stringWriter, pipeline);

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