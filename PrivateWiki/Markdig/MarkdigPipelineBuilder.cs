using Markdig;
using Markdig.Extensions.EmphasisExtras;
using PrivateWiki.Markdig.Extensions.CodeBlockExtension;
using PrivateWiki.Markdig.Extensions.MathExtension;
using PrivateWiki.Models;
using PrivateWiki.Settings;

namespace PrivateWiki.Markdig
{
	public static class MarkdigPipelineBuilder
	{
		public static MarkdownPipeline GetMarkdownPipeline()
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
	}
}