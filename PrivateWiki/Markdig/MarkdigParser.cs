using DataAccessLibrary;
using JetBrains.Annotations;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using PrivateWiki.Markdig.Extensions.CodeBlockExtension;
using PrivateWiki.Markdig.Extensions.MathExtension;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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

		public async Task<string> ToHtmlString(string markdown)
		{
			var webViewFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\WebView");

			var file = await webViewFolder.GetFileAsync("head.html");

			var htmlHead = await FileIO.ReadTextAsync(file);

			var html = $"<!DOCTYPE html>\n<html>\n{htmlHead}\n<body>\n{ToHtmlCustom(markdown)}\n</body></html>";

			return html;
		}

		public async Task<string> ToHtmlString(PageModel page)
		{
			return await ToHtmlString(page.Content);
		}
	}
}