using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using PrivateWiki.Models.Pages;

#nullable enable


namespace PrivateWiki.Markdig
{
	public class Markdig : IPageParser, IPageRenderer
	{
		private readonly MarkdownPipeline _pipeline;
		private readonly HtmlRenderer _renderer;

		public Markdig()
		{
			_pipeline = MarkdigPipelineBuilder.GetMarkdownPipeline();
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

		public MarkdownDocument Parse(MarkdownPage page)
		{
			return Parse(page.Content);
		}

		public async Task<string> ToHtmlCustomAsync(MarkdownDocument doc)
		{
			var stringWriter = new StringWriter();
			var renderer = new HtmlRenderer(stringWriter);

			_pipeline.Setup(renderer);

			renderer.Render(doc);
			stringWriter.Flush();

			var webViewFolder =
				await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\WebView");

			var file = await webViewFolder.GetFileAsync("head.html");

			var htmlHead = await FileIO.ReadTextAsync(file);

			var html = $"<!DOCTYPE html>\n<html>\n{htmlHead}\n<body>\n{stringWriter.ToString()}\n</body></html>";

			return html;
		}

		public string ToHtmlCustom(MarkdownDocument doc)
		{
			var task = Task.Run(async () => await ToHtmlCustomAsync(doc));
			task.Wait();
			return task.Result;
		}

		public async Task<string> ToHtmlString(string markdown)
		{
			var webViewFolder =
				await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\WebView");

			var file = await webViewFolder.GetFileAsync("head.html");

			var htmlHead = await FileIO.ReadTextAsync(file);

			var html = $"<!DOCTYPE html>\n<html>\n{htmlHead}\n<body>\n{ToHtmlCustom(markdown)}\n</body></html>";

			return html;
		}

		public async Task<string> ToHtmlString(MarkdownPage page)
		{
			return await ToHtmlString(page.Content);
		}
	}
}