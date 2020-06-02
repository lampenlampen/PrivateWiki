using Markdig;
using Markdig.Extensions.EmphasisExtras;
using PrivateWiki.Rendering.Markdown.Markdig.Extensions;
using PrivateWiki.Rendering.Markdown.Markdig.Extensions.MathExtension;
using PrivateWiki.Rendering.Markdown.Markdig.Extensions.TagExtension;
using PrivateWiki.Rendering.Markdown.Markdig.Extensions.WikiLinkExtension;

namespace PrivateWiki.Rendering.Markdown.Markdig
{
	public static class MarkdigPipelineBuilder
	{
		public static MarkdownPipeline GetMarkdownPipeline(HtmlBuilder htmlBuilder)
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

		private static MarkdownPipeline GetMarkdownPipelineImpl(HtmlBuilder htmlBuilder)
		{
			var pipelineBuilder = new MarkdownPipelineBuilder();


			htmlBuilder.UseVSCodeMarkdownStylesheet();
			htmlBuilder.AddKeyboardListener();
			htmlBuilder.AddCharset();

			var settings = Application.Instance.AppSettings.MarkdownRenderingSettings;

			if (!settings.IsHtmlEnabled) pipelineBuilder.DisableHtml();
			if (settings.IsAbbreviationEnabled) pipelineBuilder.UseAbbreviations();
			// TODO AutoIdentifierOptions
			if (settings.IsAutoIdentifierEnabled) pipelineBuilder.UseAutoIdentifiers();
			if (settings.IsBootstrapEnabled) pipelineBuilder.UseBootstrap();
			if (settings.IsCitationEnabled) pipelineBuilder.UseCitations();
			if (settings.IsDefinitionListEnabled) pipelineBuilder.UseDefinitionLists();
			// TODO EmojiSmileyOptions
			if (settings.IsEmojiSmileyEnabled) pipelineBuilder.UseEmojiAndSmiley();
			if (settings.IsFigureEnabled) pipelineBuilder.UseFigures();
			if (settings.IsFooterEnabled) pipelineBuilder.UseFooters();
			if (settings.IsFootnoteEnabled) pipelineBuilder.UseFootnotes();
			// TODO MediaLinkOptions
			if (settings.IsMedialinkEnabled) pipelineBuilder.UseMediaLinks();
			if (settings.IsSoftlineAsHardlineBreakEnabled) pipelineBuilder.UseSoftlineBreakAsHardlineBreak();
			// TODO SmartyPantsOptions
			if (settings.IsSmartyPantEnabled) pipelineBuilder.UseSmartyPants();
			if (settings.IsGenericAttributeEnabled) pipelineBuilder.UseGenericAttributes();

			if (settings.IsDiagramEnabled)
			{
				pipelineBuilder.UseDiagrams();

				if (settings.IsMermaidEnabled) htmlBuilder.UseMermaid();
				if (settings.IsNomnomlEnabled) htmlBuilder.UseNomnoml();
			}

			if (settings.IsEmphasisEnabled)
			{
				EmphasisExtraOptions options = 0;

				if (settings.IsStrikethroughEnabled) options |= EmphasisExtraOptions.Strikethrough;
				if (settings.IsInsertedEnabled) options |= EmphasisExtraOptions.Inserted;
				if (settings.IsMarkedEnabled) options |= EmphasisExtraOptions.Marked;
				if (settings.IsSuperSubScriptEnabled) options = options | EmphasisExtraOptions.Subscript | EmphasisExtraOptions.Superscript;

				pipelineBuilder.UseEmphasisExtras(options);
			}

			if (settings.IsListEnabled)
			{
				if (settings.IsTaskListEnabled) pipelineBuilder.UseTaskLists();
				if (settings.IsListExtraEnabled) pipelineBuilder.UseListExtras();
			}

			if (settings.IsMathEnabled)
			{
				pipelineBuilder.UseMyMathExtension();
				htmlBuilder.UseMathjax();
			}

			if (settings.IsSyntaxHighlightingEnabled)
			{
				htmlBuilder.UsePrismSyntaxHighlighting();
			}

			if (settings.IsTableEnabled)
			{
				if (settings.IsGridTableEnabled) pipelineBuilder.UseGridTables();
				// TODO PipeTableOptions
				if (settings.IsPiepTableEnabled) pipelineBuilder.UsePipeTables();
			}

			pipelineBuilder.UseMyWikiLinkExtension();
			pipelineBuilder.UseTagExtension();
			pipelineBuilder.UseYamlFrontMatter();

			htmlBuilder.Flush();

			return pipelineBuilder.Build();
		}
	}
}