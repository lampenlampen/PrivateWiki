using System;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Settings;
using PrivateWiki.Services.KeyValueCaches;

namespace PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService
{
	public class RenderingMarkdownSettingsService : IRenderingMarkdownSettingsService
	{
		private readonly IKeyValueCache _cache;

		public RenderingMarkdownSettingsService(IPersistentKeyValueCache cache)
		{
			_cache = cache;
		}

		private async Task<bool> GetBool(string key, bool defaultValue = false)
		{
			var result = await _cache.GetBooleanAsync(key);

			if (result.IsSuccess) return result.Value;
			if (result.HasError<KeyNotFoundError>()) return defaultValue;

			result.Log();

			throw new Exception();
		}

		public Task IsHtmlEnabled(bool value) => _cache.InsertAsync(Keys.IsHtmlEnabled, value);

		public Task<bool> IsHtmlEnabled() => GetBool(Keys.IsHtmlEnabled);

		public Task IsAbbreviationEnabled(bool value) => _cache.InsertAsync(Keys.IsAbbreviationEnabled, value);

		public Task<bool> IsAbbreviationEnabled() => GetBool(Keys.IsAbbreviationEnabled);

		public Task IsAutoIdentifierEnabled(bool value) => _cache.InsertAsync(Keys.IsAutoIdentifierEnabled, value);

		public Task<bool> IsAutoIdentifierEnabled() => GetBool(Keys.IsAutoIdentifierEnabled);

		public Task IsAutoLinkEnabled(bool value) => _cache.InsertAsync(Keys.IsAutoLinkEnabled, value);

		public Task<bool> IsAutoLinkEnabled() => GetBool(Keys.IsAutoLinkEnabled);

		public Task IsBootstrapEnabled(bool value) => _cache.InsertAsync(Keys.IsBootstrapEnabled, value);

		public Task<bool> IsBootstrapEnabled() => GetBool(Keys.IsBootstrapEnabled);

		public Task IsCitationEnabled(bool value) => _cache.InsertAsync(Keys.IsCitationEnabled, value);

		public Task<bool> IsCitationEnabled() => GetBool(Keys.IsCitationEnabled);

		public Task IsDefinitionListEnabled(bool value) => _cache.InsertAsync(Keys.IsDefinitionListEnabled, value);

		public Task<bool> IsDefinitionListEnabled() => GetBool(Keys.IsDefinitionListEnabled);

		public Task IsEmojiSmileyEnabled(bool value) => _cache.InsertAsync(Keys.IsEmojiSmileyEnabled, value);

		public Task<bool> IsEmojiSmileyEnabled() => GetBool(Keys.IsEmojiSmileyEnabled);

		public Task IsFigureEnabled(bool value) => _cache.InsertAsync(Keys.IsFigureEnabled, value);

		public Task<bool> IsFigureEnabled() => GetBool(Keys.IsFigureEnabled);

		public Task IsFooterEnabled(bool value) => _cache.InsertAsync(Keys.IsFooterEnabled, value);

		public Task<bool> IsFooterEnabled() => GetBool(Keys.IsFooterEnabled);

		public Task IsFootnoteEnabled(bool value) => _cache.InsertAsync(Keys.IsFootnoteEnabled, value);

		public Task<bool> IsFootnoteEnabled() => GetBool(Keys.IsFootnoteEnabled);

		public Task IsMedialinkEnabled(bool value) => _cache.InsertAsync(Keys.IsMedialinkEnabled, value);

		public Task<bool> IsMedialinkEnabled() => GetBool(Keys.IsMedialinkEnabled);

		public Task IsSoftlineAsHardlineBreakEnabled(bool value) => _cache.InsertAsync(Keys.IsSoftlineAsHardlineBreakEnabled, value);

		public Task<bool> IsSoftlineAsHardlineBreakEnabled() => GetBool(Keys.IsSoftlineAsHardlineBreakEnabled);

		public Task IsSmartyPantEnabled(bool value) => _cache.InsertAsync(Keys.IsSmartyPantEnabled, value);

		public Task<bool> IsSmartyPantEnabled() => GetBool(Keys.IsSmartyPantEnabled);

		public Task IsGenericAttributeEnabled(bool value) => _cache.InsertAsync(Keys.IsGenericAttributeEnabled, value);

		public Task<bool> IsGenericAttributeEnabled() => GetBool(Keys.IsGenericAttributeEnabled);

		public Task IsDiagramEnabled(bool value) => _cache.InsertAsync(Keys.IsDiagramEnabled, value);

		public Task<bool> IsDiagramEnabled() => GetBool(Keys.IsDiagramEnabled);

		public Task IsMermaidEnabled(bool value) => _cache.InsertAsync(Keys.IsMermaidEnabled, value);

		public Task<bool> IsMermaidEnabled() => GetBool(Keys.IsMermaidEnabled);

		public Task IsNomnomlEnabled(bool value) => _cache.InsertAsync(Keys.IsNomnomlEnabled, value);

		public Task<bool> IsNomnomlEnabled() => GetBool(Keys.IsNomnomlEnabled);

		public Task IsEmphasisEnabled(bool value) => _cache.InsertAsync(Keys.IsEmphasisEnabled, value);

		public Task<bool> IsEmphasisEnabled() => GetBool(Keys.IsEmphasisEnabled);

		public Task IsStrikethroughEnabled(bool value) => _cache.InsertAsync(Keys.IsStrikethroughEnabled, value);

		public Task<bool> IsStrikethroughEnabled() => GetBool(Keys.IsStrikethroughEnabled);

		public Task IsSuperSubScriptEnabled(bool value) => _cache.InsertAsync(Keys.IsSuperSubScriptEnabled, value);

		public Task<bool> IsSuperSubScriptEnabled() => GetBool(Keys.IsSuperSubScriptEnabled);

		public Task IsInsertedEnabled(bool value) => _cache.InsertAsync(Keys.IsInsertedEnabled, value);

		public Task<bool> IsInsertedEnabled() => GetBool(Keys.IsInsertedEnabled);

		public Task IsMarkedEnabled(bool value) => _cache.InsertAsync(Keys.IsMarkedEnabled, value);

		public Task<bool> IsMarkedEnabled() => GetBool(Keys.IsMarkedEnabled);

		public Task IsListEnabled(bool value) => _cache.InsertAsync(Keys.IsListEnabled, value);

		public Task<bool> IsListEnabled() => GetBool(Keys.IsListEnabled);

		public Task IsTaskListEnabled(bool value) => _cache.InsertAsync(Keys.IsTaskListEnabled, value);

		public Task<bool> IsTaskListEnabled() => GetBool(Keys.IsTaskListEnabled);

		public Task IsListExtraEnabled(bool value) => _cache.InsertAsync(Keys.IsListExtraEnabled, value);

		public Task<bool> IsListExtraEnabled() => GetBool(Keys.IsListExtraEnabled);

		public Task IsMathEnabled(bool value) => _cache.InsertAsync(Keys.IsMathEnabled, value);

		public Task<bool> IsMathEnabled() => GetBool(Keys.IsMathEnabled);

		public Task IsSyntaxHighlightingEnabled(bool value) => _cache.InsertAsync(Keys.IsSyntaxHighlightingEnabled, value);

		public Task<bool> IsSyntaxHighlightingEnabled() => GetBool(Keys.IsSyntaxHighlightingEnabled);

		public Task IsTableEnabled(bool value) => _cache.InsertAsync(Keys.IsTableEnabled, value);

		public Task<bool> IsTableEnabled() => GetBool(Keys.IsTableEnabled);

		public Task IsGridTableEnabled(bool value) => _cache.InsertAsync(Keys.IsGridTableEnabled, value);

		public Task<bool> IsGridTableEnabled() => GetBool(Keys.IsGridTableEnabled);

		public Task IsPipeTableEnabled(bool value) => _cache.InsertAsync(Keys.IsPipeTableEnabled, value);

		public Task<bool> IsPipeTableEnabled() => GetBool(Keys.IsPipeTableEnabled);

		public async Task<RenderingMarkdownSettingsModel> GetRenderingMarkdownSettingsModelAsync()
		{
			// TODO Enhance IPersistentKeyValueCache to retrieve multiple values at once.
			return new RenderingMarkdownSettingsModel
			{
				IsHtmlEnabled = await IsHtmlEnabled().ConfigureAwait(false),
				IsAbbreviationEnabled = await IsAbbreviationEnabled().ConfigureAwait(false),
				IsAutoIdentifierEnabled = await IsAutoIdentifierEnabled().ConfigureAwait(false),
				IsAutoLinkEnabled = await IsAutoLinkEnabled().ConfigureAwait(false),
				IsBootstrapEnabled = await IsBootstrapEnabled().ConfigureAwait(false),
				IsCitationEnabled = await IsCitationEnabled().ConfigureAwait(false),
				IsDefinitionListEnabled = await IsDefinitionListEnabled().ConfigureAwait(false),
				IsEmojiSmileyEnabled = await IsEmojiSmileyEnabled().ConfigureAwait(false),
				IsFigureEnabled = await IsFigureEnabled().ConfigureAwait(false),
				IsFooterEnabled = await IsFooterEnabled().ConfigureAwait(false),
				IsFootnoteEnabled = await IsFootnoteEnabled().ConfigureAwait(false),
				IsMedialinkEnabled = await IsMedialinkEnabled().ConfigureAwait(false),
				IsSoftlineAsHardlineBreakEnabled = await IsSoftlineAsHardlineBreakEnabled().ConfigureAwait(false),
				IsSmartyPantEnabled = await IsSmartyPantEnabled().ConfigureAwait(false),
				IsGenericAttributeEnabled = await IsGenericAttributeEnabled().ConfigureAwait(false),
				IsDiagramEnabled = await IsDiagramEnabled().ConfigureAwait(false),
				IsMermaidEnabled = await IsMermaidEnabled().ConfigureAwait(false),
				IsNomnomlEnabled = await IsNomnomlEnabled().ConfigureAwait(false),
				IsEmphasisEnabled = await IsEmphasisEnabled().ConfigureAwait(false),
				IsStrikethroughEnabled = await IsStrikethroughEnabled().ConfigureAwait(false),
				IsSuperSubScriptEnabled = await IsSuperSubScriptEnabled().ConfigureAwait(false),
				IsInsertedEnabled = await IsInsertedEnabled().ConfigureAwait(false),
				IsMarkedEnabled = await IsMarkedEnabled().ConfigureAwait(false),
				IsListEnabled = await IsListEnabled().ConfigureAwait(false),
				IsTaskListEnabled = await IsTaskListEnabled().ConfigureAwait(false),
				IsListExtraEnabled = await IsListExtraEnabled().ConfigureAwait(false),
				IsMathEnabled = await IsMathEnabled().ConfigureAwait(false),
				IsSyntaxHighlightingEnabled = await IsSyntaxHighlightingEnabled().ConfigureAwait(false),
				IsTableEnabled = await IsTableEnabled().ConfigureAwait(false),
				IsGridTableEnabled = await IsGridTableEnabled().ConfigureAwait(false),
				IsPipeTableEnabled = await IsPipeTableEnabled().ConfigureAwait(false)
			};
		}

		public Task SaveRenderingMarkdownSettingsModelAsync(RenderingMarkdownSettingsModel model)
		{
			return Task.WhenAll(
				IsHtmlEnabled(model.IsHtmlEnabled),
				IsAbbreviationEnabled(model.IsAbbreviationEnabled),
				IsAutoIdentifierEnabled(model.IsAutoIdentifierEnabled),
				IsAutoLinkEnabled(model.IsAutoLinkEnabled),
				IsBootstrapEnabled(model.IsBootstrapEnabled),
				IsCitationEnabled(model.IsCitationEnabled),
				IsDefinitionListEnabled(model.IsDefinitionListEnabled),
				IsEmojiSmileyEnabled(model.IsEmojiSmileyEnabled),
				IsFigureEnabled(model.IsFigureEnabled),
				IsFooterEnabled(model.IsFooterEnabled),
				IsFootnoteEnabled(model.IsFootnoteEnabled),
				IsMedialinkEnabled(model.IsMedialinkEnabled),
				IsSoftlineAsHardlineBreakEnabled(model.IsSoftlineAsHardlineBreakEnabled),
				IsSmartyPantEnabled(model.IsSmartyPantEnabled),
				IsGenericAttributeEnabled(model.IsGenericAttributeEnabled),
				IsDiagramEnabled(model.IsDiagramEnabled),
				IsMermaidEnabled(model.IsMermaidEnabled),
				IsNomnomlEnabled(model.IsNomnomlEnabled),
				IsEmphasisEnabled(model.IsEmphasisEnabled),
				IsStrikethroughEnabled(model.IsStrikethroughEnabled),
				IsSuperSubScriptEnabled(model.IsSuperSubScriptEnabled),
				IsInsertedEnabled(model.IsInsertedEnabled),
				IsMarkedEnabled(model.IsMarkedEnabled),
				IsListEnabled(model.IsListEnabled),
				IsTaskListEnabled(model.IsTaskListEnabled),
				IsListExtraEnabled(model.IsListExtraEnabled),
				IsMathEnabled(model.IsMathEnabled),
				IsSyntaxHighlightingEnabled(model.IsSyntaxHighlightingEnabled),
				IsTableEnabled(model.IsTableEnabled),
				IsGridTableEnabled(model.IsGridTableEnabled),
				IsPipeTableEnabled(model.IsPipeTableEnabled)
			);
		}

		private static class Keys
		{
			public const string IsHtmlEnabled = "settings_rendering_markdown_isHtmlEnabled";

			public const string IsAbbreviationEnabled = "settings_rendering_markdown_isAbbreviationEnabled";

			public const string IsAutoIdentifierEnabled = "settings_rendering_markdown_isAutoIdentifierEnabled";

			public const string IsAutoLinkEnabled = "settings_rendering_markdown_isAutoLinkEnabled";

			public const string IsBootstrapEnabled = "settings_rendering_markdown_isBootstrapEnabled";

			public const string IsCitationEnabled = "settings_rendering_markdown_isCitationEnabled";

			public const string IsDefinitionListEnabled = "settings_rendering_markdown_isDefinitionListEnabled";

			public const string IsEmojiSmileyEnabled = "settings_rendering_markdown_isEmojiSmileyEnabled";

			public const string IsFigureEnabled = "settings_rendering_markdown_isFigureEnabled";

			public const string IsFooterEnabled = "settings_rendering_markdown_isFooterEnabled";

			public const string IsFootnoteEnabled = "settings_rendering_markdown_isFootnoteEnabled";

			public const string IsMedialinkEnabled = "settings_rendering_markdown_isMediaLinkEnabled";

			public const string IsSoftlineAsHardlineBreakEnabled = "settings_rendering_markdown_isSoftlineAsHardlineBreakEnabled";

			public const string IsSmartyPantEnabled = "settings_rendering_markdown_isSmartyPantEnabled";

			public const string IsGenericAttributeEnabled = "settings_rendering_markdown_isGenericAttributeEnabled";

			public const string IsDiagramEnabled = "settings_rendering_markdown_isDiagramEnabled";

			public const string IsMermaidEnabled = "settings_rendering_markdown_isMermaidEnabled";

			public const string IsNomnomlEnabled = "settings_rendering_markdown_isNomnomlEnabled";

			public const string IsEmphasisEnabled = "settings_rendering_markdown_isEmphasisEnabled";

			public const string IsStrikethroughEnabled = "settings_rendering_markdown_isStrikethroughEnabled";

			public const string IsSuperSubScriptEnabled = "settings_rendering_markdown_isSuperSubScriptEnabled";

			public const string IsInsertedEnabled = "settings_rendering_markdown_isInsertedEnabled";

			public const string IsMarkedEnabled = "settings_rendering_markdown_isMarkedEnabled";

			public const string IsListEnabled = "settings_rendering_markdown_isListEnabled";

			public const string IsTaskListEnabled = "settings_rendering_markdown_isTaskListEnabled";

			public const string IsListExtraEnabled = "settings_rendering_markdown_isListExtraEnabled";

			public const string IsMathEnabled = "settings_rendering_markdown_isMathEnabled";

			public const string IsSyntaxHighlightingEnabled = "settings_rendering_markdown_isSyntaxHighlightingEnabled";

			public const string IsTableEnabled = "settings_rendering_markdown_isTableEnabled";

			public const string IsGridTableEnabled = "settings_rendering_markdown_isGridTableEnabled";

			public const string IsPipeTableEnabled = "settings_rendering_markdown_isPipeTableEnabled";
		}
	}
}