using System.Threading.Tasks;
using Markdig;
using Markdig.Extensions.EmphasisExtras;
using PrivateWiki.Rendering.Markdown.Markdig.Extensions;
using PrivateWiki.Rendering.Markdown.Markdig.Extensions.MathExtension;
using PrivateWiki.Rendering.Markdown.Markdig.Extensions.TagExtension;
using PrivateWiki.Rendering.Markdown.Markdig.Extensions.WikiLinkExtension;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;

namespace PrivateWiki.Rendering.Markdown.Markdig
{
	public static class MarkdigPipelineBuilder
	{
		public static Task<MarkdownPipeline> GetMarkdownPipeline(HtmlBuilder htmlBuilder)
		{
			return GetMarkdownPipelineImpl(htmlBuilder);
		}

		private static MarkdownPipeline GetMarkdownPipelineImpl2(HtmlBuilder htmlBuilder)
		{
			var pipelineBuilder = new MarkdownPipelineBuilder();

			/*
			
			var handler = new RenderingModelHandler();

			var models = handler.LoadRenderingModels();

			htmlBuilder.UseVSCodeMarkdownStylesheet();
			htmlBuilder.AddKeyboardListener();
			htmlBuilder.AddCharset();

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
							htmlBuilder.UseMermaid();
							htmlBuilder.UseNomnoml();
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
							htmlBuilder.UseMathjax();
						}

						break;
					case SyntaxHighlightingRenderModel syntaxHighlightingRenderModel:
						if (syntaxHighlightingRenderModel.IsEnabled)
						{
							htmlBuilder.UsePrismSyntaxHighlighting();
						}

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

			pipelineBuilder.UseMyWikiLinkExtension();
			pipelineBuilder.UseTagExtension();
			pipelineBuilder.UseYamlFrontMatter();

			htmlBuilder.Flush();

			*/

			return pipelineBuilder.Build();
		}

		private async static Task<MarkdownPipeline> GetMarkdownPipelineImpl(HtmlBuilder htmlBuilder)
		{
			var pipelineBuilder = new MarkdownPipelineBuilder();


			htmlBuilder.UseVSCodeMarkdownStylesheet();
			htmlBuilder.AddKeyboardListener();
			htmlBuilder.AddCharset();

			IRenderingMarkdownSettingsService settings = Application.Instance.AppSettings.RenderingMarkdownSettings;

			if (!await settings.IsHtmlEnabled()) pipelineBuilder.DisableHtml();
			if (await settings.IsAbbreviationEnabled()) pipelineBuilder.UseAbbreviations();
			// TODO AutoIdentifierOptions
			if (await settings.IsAutoIdentifierEnabled()) pipelineBuilder.UseAutoIdentifiers();
			if (await settings.IsBootstrapEnabled()) pipelineBuilder.UseBootstrap();
			if (await settings.IsCitationEnabled()) pipelineBuilder.UseCitations();
			if (await settings.IsDefinitionListEnabled()) pipelineBuilder.UseDefinitionLists();
			// TODO EmojiSmileyOptions
			if (await settings.IsEmojiSmileyEnabled()) pipelineBuilder.UseEmojiAndSmiley();
			if (await settings.IsFigureEnabled()) pipelineBuilder.UseFigures();
			if (await settings.IsFooterEnabled()) pipelineBuilder.UseFooters();
			if (await settings.IsFootnoteEnabled()) pipelineBuilder.UseFootnotes();
			// TODO MediaLinkOptions
			if (await settings.IsMedialinkEnabled()) pipelineBuilder.UseMediaLinks();
			if (await settings.IsSoftlineAsHardlineBreakEnabled()) pipelineBuilder.UseSoftlineBreakAsHardlineBreak();
			// TODO SmartyPantsOptions
			if (await settings.IsSmartyPantEnabled()) pipelineBuilder.UseSmartyPants();
			if (await settings.IsGenericAttributeEnabled()) pipelineBuilder.UseGenericAttributes();

			if (await settings.IsDiagramEnabled())
			{
				pipelineBuilder.UseDiagrams();

				if (await settings.IsMermaidEnabled()) htmlBuilder.UseMermaid();
				if (await settings.IsNomnomlEnabled()) htmlBuilder.UseNomnoml();
			}

			if (await settings.IsEmphasisEnabled())
			{
				EmphasisExtraOptions options = 0;

				if (await settings.IsStrikethroughEnabled()) options |= EmphasisExtraOptions.Strikethrough;
				if (await settings.IsInsertedEnabled()) options |= EmphasisExtraOptions.Inserted;
				if (await settings.IsMarkedEnabled()) options |= EmphasisExtraOptions.Marked;
				if (await settings.IsSuperSubScriptEnabled()) options = options | EmphasisExtraOptions.Subscript | EmphasisExtraOptions.Superscript;

				pipelineBuilder.UseEmphasisExtras(options);
			}

			if (await settings.IsListEnabled())
			{
				if (await settings.IsTaskListEnabled()) pipelineBuilder.UseTaskLists();
				if (await settings.IsListExtraEnabled()) pipelineBuilder.UseListExtras();
			}

			if (await settings.IsMathEnabled())
			{
				pipelineBuilder.UseMyMathExtension();
				htmlBuilder.UseMathjax();
			}

			if (await settings.IsSyntaxHighlightingEnabled())
			{
				htmlBuilder.UsePrismSyntaxHighlighting();
			}

			if (await settings.IsTableEnabled())
			{
				if (await settings.IsGridTableEnabled()) pipelineBuilder.UseGridTables();
				// TODO PipeTableOptions
				if (await settings.IsPipeTableEnabled()) pipelineBuilder.UsePipeTables();
			}

			pipelineBuilder.UseMyWikiLinkExtension();
			pipelineBuilder.UseTagExtension();
			pipelineBuilder.UseYamlFrontMatter();

			htmlBuilder.Flush();

			return pipelineBuilder.Build();
		}
	}
}