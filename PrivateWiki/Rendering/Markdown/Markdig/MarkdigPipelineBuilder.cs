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
	public class MarkdigPipelineBuilder
	{
		private readonly IRenderingMarkdownSettingsService _settings;

		public MarkdigPipelineBuilder(IRenderingMarkdownSettingsService renderingSettings)
		{
			this._settings = renderingSettings;
		}

		public Task<MarkdownPipeline> GetMarkdownPipeline(HtmlBuilder htmlBuilder)
		{
			return GetMarkdownPipelineImpl(htmlBuilder);
		}

		private async Task<MarkdownPipeline> GetMarkdownPipelineImpl(HtmlBuilder htmlBuilder)
		{
			var pipelineBuilder = new MarkdownPipelineBuilder();


			htmlBuilder.UseVSCodeMarkdownStylesheet();
			htmlBuilder.AddKeyboardListener();
			htmlBuilder.AddCharset();

			if (!await _settings.IsHtmlEnabled()) pipelineBuilder.DisableHtml();
			if (await _settings.IsAbbreviationEnabled()) pipelineBuilder.UseAbbreviations();
			// TODO AutoIdentifierOptions
			if (await _settings.IsAutoIdentifierEnabled()) pipelineBuilder.UseAutoIdentifiers();
			if (await _settings.IsBootstrapEnabled()) pipelineBuilder.UseBootstrap();
			if (await _settings.IsCitationEnabled()) pipelineBuilder.UseCitations();
			if (await _settings.IsDefinitionListEnabled()) pipelineBuilder.UseDefinitionLists();
			// TODO EmojiSmileyOptions
			if (await _settings.IsEmojiSmileyEnabled()) pipelineBuilder.UseEmojiAndSmiley();
			if (await _settings.IsFigureEnabled()) pipelineBuilder.UseFigures();
			if (await _settings.IsFooterEnabled()) pipelineBuilder.UseFooters();
			if (await _settings.IsFootnoteEnabled()) pipelineBuilder.UseFootnotes();
			// TODO MediaLinkOptions
			if (await _settings.IsMedialinkEnabled()) pipelineBuilder.UseMediaLinks();
			if (await _settings.IsSoftlineAsHardlineBreakEnabled()) pipelineBuilder.UseSoftlineBreakAsHardlineBreak();
			// TODO SmartyPantsOptions
			if (await _settings.IsSmartyPantEnabled()) pipelineBuilder.UseSmartyPants();
			if (await _settings.IsGenericAttributeEnabled()) pipelineBuilder.UseGenericAttributes();

			if (await _settings.IsDiagramEnabled())
			{
				pipelineBuilder.UseDiagrams();

				if (await _settings.IsMermaidEnabled()) htmlBuilder.UseMermaid();
				if (await _settings.IsNomnomlEnabled()) htmlBuilder.UseNomnoml();
			}

			if (await _settings.IsEmphasisEnabled())
			{
				EmphasisExtraOptions options = 0;

				if (await _settings.IsStrikethroughEnabled()) options |= EmphasisExtraOptions.Strikethrough;
				if (await _settings.IsInsertedEnabled()) options |= EmphasisExtraOptions.Inserted;
				if (await _settings.IsMarkedEnabled()) options |= EmphasisExtraOptions.Marked;
				if (await _settings.IsSuperSubScriptEnabled()) options = options | EmphasisExtraOptions.Subscript | EmphasisExtraOptions.Superscript;

				pipelineBuilder.UseEmphasisExtras(options);
			}

			if (await _settings.IsListEnabled())
			{
				if (await _settings.IsTaskListEnabled()) pipelineBuilder.UseTaskLists();
				if (await _settings.IsListExtraEnabled()) pipelineBuilder.UseListExtras();
			}

			if (await _settings.IsMathEnabled())
			{
				pipelineBuilder.UseMyMathExtension();
				htmlBuilder.UseMathjax();
			}

			if (await _settings.IsSyntaxHighlightingEnabled())
			{
				htmlBuilder.UsePrismSyntaxHighlighting();
			}

			if (await _settings.IsTableEnabled())
			{
				if (await _settings.IsGridTableEnabled()) pipelineBuilder.UseGridTables();
				// TODO PipeTableOptions
				if (await _settings.IsPipeTableEnabled()) pipelineBuilder.UsePipeTables();
			}

			pipelineBuilder.UseMyWikiLinkExtension();
			pipelineBuilder.UseTagExtension();
			pipelineBuilder.UseYamlFrontMatter();

			htmlBuilder.Flush();

			return pipelineBuilder.Build();
		}
	}
}