using PrivateWiki.Services.KeyValueCaches;

// ReSharper disable RedundantArgumentDefaultValue

namespace PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService
{
	public class MarkdownRenderingSettings : IMarkdownRenderingSettingsService
	{
		private readonly IKeyValueCache _cache;

		public MarkdownRenderingSettings(IPersistentKeyValueCache cache)
		{
			_cache = cache;
		}

		private bool Get(string key, bool defaultValue = false)
		{
			var result = _cache.GetBooleanAsync(key).GetAwaiter().GetResult();

			if (result.HasError<KeyNotFoundError>()) return defaultValue;

			return result.Value;
		}

		private void Set(string key, bool value)
		{
			_cache.InsertAsync(key, value).GetAwaiter().GetResult();
		}

		#region Core

		public bool IsHtmlEnabled
		{
			get => Get("rendering_markdown_isHtmlEnabled", false);
			set => Set("rendering_markdown_isHtmlEnabled", value);
		}

		public bool IsAbbreviationEnabled
		{
			get => Get("rendering_markdown_isAbbreviationEnabled", false);
			set => Set("rendering_markdown_isAbbreviationEnabled", value);
		}

		public bool IsAutoIdentifierEnabled
		{
			get => Get("rendering_markdown_isAutoIdentifierEnabled", false);
			set => Set("rendering_markdown_isAutoIdentifierEnabled", value);
		}

		public bool IsAutoLinkEnabled
		{
			get => Get("rendering_markdown_isAutoLinkEnabled", false);
			set => Set("rendering_markdown_isAutoLinkEnabled", value);
		}

		public bool IsBootstrapEnabled
		{
			get => Get("rendering_markdown_isBootstrapEnabled", false);
			set => Set("rendering_markdown_isBootstrapEnabled", value);
		}

		public bool IsCitationEnabled
		{
			get => Get("rendering_markdown_isCitationEnabled", false);
			set => Set("rendering_markdown_isCitationEnabled", value);
		}

		public bool IsDefinitionListEnabled
		{
			get => Get("rendering_markdown_isDefinitionListEnabled", false);
			set => Set("rendering_markdown_isDefinitionListEnabled", value);
		}

		public bool IsEmojiSmileyEnabled
		{
			get => Get("rendering_markdown_isEmojiSmileyEnabled", false);
			set => Set("rendering_markdown_isEmojiSmileyEnabled", value);
		}

		public bool IsFigureEnabled
		{
			get => Get("rendering_markdown_isFigureEnabled", false);
			set => Set("rendering_markdown_isFigureEnabled", value);
		}

		public bool IsFooterEnabled
		{
			get => Get("rendering_markdown_isFooterEnabled", false);
			set => Set("rendering_markdown_isFooterEnabled", value);
		}

		public bool IsFootnoteEnabled
		{
			get => Get("rendering_markdown_isFootnoteEnabled", false);
			set => Set("rendering_markdown_isFootnoteEnabled", value);
		}

		public bool IsMedialinkEnabled
		{
			get => Get("rendering_markdown_isMediaLinkEnabled", false);
			set => Set("rendering_markdown_isMediaLinkEnabled", value);
		}

		public bool IsSoftlineAsHardlineBreakEnabled
		{
			get => Get("rendering_markdown_isSoftlineAsHardlineBreakEnabled", false);
			set => Set("rendering_markdown_isSoftlineAsHardlineBreakEnabled", value);
		}

		public bool IsSmartyPantEnabled
		{
			get => Get("rendering_markdown_isSmartyPantEnabled", false);
			set => Set("rendering_markdown_isSmartyPantEnabled", value);
		}

		public bool IsGenericAttributeEnabled
		{
			get => Get("rendering_markdown_isGenericAttributeEnabled", false);
			set => Set("rendering_markdown_isGenericAttributeEnabled", value);
		}

		#endregion Core

		#region Diagram

		public bool IsDiagramEnabled
		{
			get => Get("rendering_markdown_isDiagramEnabled", false);
			set => Set("rendering_markdown_isDiagramEnabled", value);
		}

		public bool IsMermaidEnabled
		{
			get => Get("rendering_markdown_isMermaidEnabled", false);
			set => Set("rendering_markdown_isMermaidEnabled", value);
		}

		public bool IsNomnomlEnabled
		{
			get => Get("rendering_markdown_isNomnomlEnabled", false);
			set => Set("rendering_markdown_isNomnomlEnabled", value);
		}

		#endregion Diagram

		#region Emphasis

		public bool IsEmphasisEnabled
		{
			get => Get("rendering_markdown_isEmphasisEnabled", false);
			set => Set("rendering_markdown_isEmphasisEnabled", value);
		}

		public bool IsStrikethroughEnabled
		{
			get => Get("rendering_markdown_isStrikethroughEnabled", false);
			set => Set("rendering_markdown_isStrikethroughEnabled", value);
		}

		public bool IsSuperSubScriptEnabled
		{
			get => Get("rendering_markdown_isSuperSubScriptEnabled", false);
			set => Set("rendering_markdown_isSuperSubScriptEnabled", value);
		}

		public bool IsInsertedEnabled
		{
			get => Get("rendering_markdown_isInsertedEnabled", false);
			set => Set("rendering_markdown_isInsertedEnabled", value);
		}

		public bool IsMarkedEnabled
		{
			get => Get("rendering_markdown_isMarkedEnabled", false);
			set => Set("rendering_markdown_isMarkedEnabled", value);
		}

		#endregion Emphasis

		#region List

		public bool IsListEnabled
		{
			get => Get("rendering_markdown_isListEnabled", false);
			set => Set("rendering_markdown_isListEnabled", value);
		}

		public bool IsTaskListEnabled
		{
			get => Get("rendering_markdown_isTaskListEnabled", false);
			set => Set("rendering_markdown_isTaskListEnabled", value);
		}

		public bool IsListExtraEnabled
		{
			get => Get("rendering_markdown_isListExtraEnabled", false);
			set => Set("rendering_markdown_isListExtraEnabled", value);
		}

		#endregion List

		#region Math

		public bool IsMathEnabled
		{
			get => Get("rendering_markdown_isMathEnabled", false);
			set => Set("rendering_markdown_isMathEnabled", value);
		}

		#endregion Math

		#region SyntaxHighlighting

		public bool IsSyntaxHighlightingEnabled
		{
			get => Get("rendering_markdown_isSyntaxHighlightingEnabled", false);
			set => Set("rendering_markdown_isSyntaxHighlightingEnabled", value);
		}

		#endregion SyntaxHighlighting

		#region Table

		public bool IsTableEnabled
		{
			get => Get("rendering_markdown_isTableEnabled", false);
			set => Set("rendering_markdown_isTableEnabled", value);
		}


		public bool IsGridTableEnabled
		{
			get => Get("rendering_markdown_isGridTableEnabled", false);
			set => Set("rendering_markdown_isGridTableEnabled", value);
		}

		public bool IsPiepTableEnabled
		{
			get => Get("rendering_markdown_isPipeTableEnabled", false);
			set => Set("rendering_markdown_isPipeTableEnabled", value);
		}

		#endregion Table
	}
}