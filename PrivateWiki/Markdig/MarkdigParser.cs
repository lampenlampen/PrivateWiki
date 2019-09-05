using JetBrains.Annotations;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using PrivateWiki.Markdig.Extensions.CodeBlockExtension;
using PrivateWiki.Markdig.Extensions.MathExtension;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Markdig.Extensions.EmphasisExtras;
using PrivateWiki.Models;
using PrivateWiki.Settings;
using StorageBackend;

namespace PrivateWiki.Markdig
{
	public class MarkdigParser : IPageParser, IPageRenderer
	{
		[NotNull] private readonly MarkdownPipeline _pipeline;
		[NotNull] private readonly HtmlRenderer _renderer;

		public MarkdigParser()
		{
			_pipeline = BuildMarkdownPipeline();

			_renderer = new HtmlRenderer(new StringWriter());
		}

		private MarkdownPipeline BuildMarkdownPipeline()
		{
			var pipelineBuilder = new MarkdownPipelineBuilder();

			var handler = new RenderingModelHandler();

			var models = handler.LoadRenderingModels();

			foreach (var model in models)
			{
				switch (model)
				{
					case CoreRenderModel coreRenderModel:
						if (!coreRenderModel.IsHtmlTagEnabled) pipelineBuilder.DisableHtml();
						if (coreRenderModel.IsAbbreviationEnabled) pipelineBuilder.UseAbbreviations();
						// TODO AutoIdentifierOptions
						if (coreRenderModel.IsAutoIdentifierEnabled) pipelineBuilder.UseAutoIdentifiers();
						if (coreRenderModel.IsBootstrapEnabled) pipelineBuilder.UseBootstrap();
						if (coreRenderModel.IsCitationEnabled) pipelineBuilder.UseCitations();
						if (coreRenderModel.IsDefinitionListEnabled) pipelineBuilder.UseDefinitionLists();
						// TODO EmojisSmileyOptions
						if (coreRenderModel.IsEmojiSmileyEnabled) pipelineBuilder.UseEmojiAndSmiley();
						if (coreRenderModel.IsFigureEnabled) pipelineBuilder.UseFigures();
						if (coreRenderModel.IsFooterEnabled) pipelineBuilder.UseFooters();
						if (coreRenderModel.IsFootnoteEnabled) pipelineBuilder.UseFootnotes();
						// TODO MediaLinksOptions
						if (coreRenderModel.IsMediaLinkEnabled) pipelineBuilder.UseMediaLinks();
						if (coreRenderModel.IsSoftLineAsHardlineBreakEnabled)
							pipelineBuilder.UseSoftlineBreakAsHardlineBreak();
						// TODO SmartPantsOptions
						if (coreRenderModel.IsSmartyPantEnabled) pipelineBuilder.UseSmartyPants();
						if (coreRenderModel.IsGenericAttributeEnabled) pipelineBuilder.UseGenericAttributes();
						break;
					case DiagramRenderModel diagramRenderModel:
						if (diagramRenderModel.IsEnabled)
						{
							// TODO enable individual diagrams
							pipelineBuilder.UseDiagrams();
						}

						break;
					case EmphasisExtraRenderModel emphasisExtraRenderModel:
						if (emphasisExtraRenderModel.IsEnabled)
						{
							EmphasisExtraOptions options = 0;
							if (emphasisExtraRenderModel.IsStrikethroughEnabled)
								options |= EmphasisExtraOptions.Strikethrough;
							if (emphasisExtraRenderModel.IsInsertedEnabled) options |= EmphasisExtraOptions.Inserted;
							if (emphasisExtraRenderModel.IsMarkedEnabled) options |= EmphasisExtraOptions.Marked;
							if (emphasisExtraRenderModel.IsSuperSubScriptEnabled)
								options = options | EmphasisExtraOptions.Subscript | EmphasisExtraOptions.Superscript;

							pipelineBuilder.UseEmphasisExtras(options);
						}

						break;
					case ListRenderModel listRenderModel:
						if (listRenderModel.IsEnabled)
						{
							if (listRenderModel.IsTaskListEnabled) pipelineBuilder.UseTaskLists();
							if (listRenderModel.IsListExtraEnabled) pipelineBuilder.UseListExtras();
						}

						break;
					case MathRenderModel mathRenderModel:
						if (mathRenderModel.IsEnabled)
						{
							pipelineBuilder.UseMyMathExtension();
						}

						break;
					case SyntaxHighlightingRenderModel syntaxHighlightingRenderModel:
						if (syntaxHighlightingRenderModel.IsEnabled) pipelineBuilder.UseMyHtmlCodeBlockRenderer();
						break;
					case TableRenderModel tableRenderModel:
						if (tableRenderModel.IsEnabled)
						{
							if (tableRenderModel.IsGridTableEnabled) pipelineBuilder.UseGridTables();
							// TODO PipeTableOptions
							if (tableRenderModel.IsPipeTableEnabled) pipelineBuilder.UsePipeTables();
						}

						break;
				}
			}

			return pipelineBuilder.Build();
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
			var webViewFolder =
				await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\WebView");

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