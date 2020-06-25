using PrivateWiki.Services.AppSettingsService.KeyValueCaches;

namespace PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService
{
	public class MarkdownRenderingSettings
	{
		private readonly IKeyValueCache _cache;

		public MarkdownRenderingSettings(IKeyValueCache cache)
		{
			_cache = cache;
		}

		public bool IsHtmlEnabled
		{
			get
			{
				var result = _cache.GetBooleanAsync("rendering_markdown_isHtmlEnabled").GetAwaiter().GetResult();
				
				if (result.HasError<KeyNotFoundError>()) return false;

				return result.Value;
			}
			set
			{
				_cache.InsertAsync("rendering_markdown_isHtmlEnabled", value).GetAwaiter().GetResult();
			}
		}
		
		
		
		/*
		private readonly IKeyValueCache _coreAppSettings;

		public MarkdownRenderingSettings(IKeyValueCache coreAppSettings)
		{
			_coreAppSettings = coreAppSettings;
		}

		#region Core

		public bool IsHtmlEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isHtmlEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isHtmlEnabled", value);
		}

		public bool IsAbbreviationEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isAbbreviationEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isAbbreviationEnabled", value);
		}

		public bool IsAutoIdentifierEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isAutoIdentifierEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isAutoIdentifierEnabled", value);
		}

		public bool IsAutoLinkEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isAutoLinkEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isAutoLinkEnabled", value);
		}

		public bool IsBootstrapEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isBootstrapEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isBootstrapEnabled", value);
		}

		public bool IsCitationEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isCitationEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isCitationEnabled", value);
		}

		public bool IsDefinitionListEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isDefinitionListEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isDefinitionListEnabled", value);
		}

		public bool IsEmojiSmileyEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isEmojiSmileyEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isEmojiSmileyEnabled", value);
		}

		public bool IsFigureEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isFigureEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isFigureEnabled", value);
		}

		public bool IsFooterEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isFooterEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isFooterEnabled", value);
		}

		public bool IsFootnoteEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isFootnoteEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isFootnoteEnabled", value);
		}

		public bool IsMedialinkEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isMediaLinkEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isMediaLinkEnabled", value);
		}

		public bool IsSoftlineAsHardlineBreakEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isSoftlineAsHardlineBreakEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isSoftlineAsHardlineBreakEnabled", value);
		}

		public bool IsSmartyPantEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isSmartyPantEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isSmartyPantEnabled", value);
		}

		public bool IsGenericAttributeEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isGenericAttributeEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isGenericAttributeEnabled", value);
		}

		#endregion Core

		#region Diagram

		public bool IsDiagramEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isDiagramEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isDiagramEnabled", value);
		}

		public bool IsMermaidEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isMermaidEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isMermaidEnabled", value);
		}

		public bool IsNomnomlEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isNomnomlEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isNomnomlEnabled", value);
		}

		#endregion Diagram

		#region Emphasis

		public bool IsEmphasisEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isEmphasisEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isEmphasisEnabled", value);
		}

		public bool IsStrikethroughEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isStrikethroughEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isStrikethroughEnabled", value);
		}

		public bool IsSuperSubScriptEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isSuperSubScriptEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isSuperSubScriptEnabled", value);
		}

		public bool IsInsertedEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isInsertedEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isInsertedEnabled", value);
		}

		public bool IsMarkedEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isMarkedEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isMarkedEnabled", value);
		}

		#endregion Emphasis

		#region List

		public bool IsListEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isListEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isListEnabled", value);
		}

		private bool _isTaskListEnabled = false;

		public bool IsTaskListEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isTaskListEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isTaskListEnabled", value);
		}

		public bool IsListExtraEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isListExtraEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isListExtraEnabled", value);
		}

		#endregion List

		#region Math

		public bool IsMathEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isMathEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isMathEnabled", value);
		}

		#endregion Math

		#region SyntaxHighlighting

		public bool IsSyntaxHighlightingEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isSyntaxHighlightingEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isSyntaxHighlightingEnabled", value);
		}

		#endregion SyntaxHighlighting

		#region Table

		public bool IsTableEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isTableEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isTableEnabled", value);
		}
		

		public bool IsGridTableEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isGridTableEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isGridTableEnabled", value);
		}

		public bool IsPiepTableEnabled
		{
			get
			{
				var result = _coreAppSettings.GetBooleanAsync("rendering_markdown_isPipeTableEnabled");
				return result.IsSuccess ? result.Value : false;
			}
			set => _coreAppSettings.InsertAsync("rendering_markdown_isPipeTableEnabled", value);
		}

		#endregion Table
		*/
	}
}