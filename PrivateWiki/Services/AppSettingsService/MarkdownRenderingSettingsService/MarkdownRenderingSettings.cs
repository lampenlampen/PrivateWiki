using System.ComponentModel;
using System.Runtime.CompilerServices;
using DynamicData.Annotations;

namespace PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService
{
	public class MarkdownRenderingSettings : IMarkdownRenderingSettingsService
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#region Core

		private bool _isHtmlEnabled = true;

		public bool IsHtmlEnabled
		{
			get => _isHtmlEnabled;
			set
			{
				_isHtmlEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isAbbreviationEnabled = true;

		public bool IsAbbreviationEnabled
		{
			get => _isAbbreviationEnabled;
			set
			{
				_isAbbreviationEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isAutoIdentifierEnabled = true;

		public bool IsAutoIdentifierEnabled
		{
			get => _isAutoIdentifierEnabled;
			set
			{
				_isAutoIdentifierEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isAutoLinkEnabled = true;

		public bool IsAutoLinkEnabled
		{
			get => _isAbbreviationEnabled;
			set
			{
				_isAutoIdentifierEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isBootstrapEnabled = false;

		public bool IsBootstrapEnabled
		{
			get => _isBootstrapEnabled;
			set
			{
				_isAbbreviationEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isCitationEnabled = false;

		public bool IsCitationEnabled
		{
			get => _isCitationEnabled;
			set
			{
				_isCitationEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isDefinitionListEnabled = false;

		public bool IsDefinitionListEnabled
		{
			get => _isDefinitionListEnabled;
			set
			{
				_isDefinitionListEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isEmojiSmileyEnabled = false;

		public bool IsEmojiSmileyEnabled
		{
			get => _isEmojiSmileyEnabled;
			set
			{
				_isEmojiSmileyEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isFigureEnabled = false;

		public bool IsFigureEnabled
		{
			get => _isFigureEnabled;
			set
			{
				_isFigureEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isFooterEnabled = false;

		public bool IsFooterEnabled
		{
			get => _isFooterEnabled;
			set
			{
				_isFooterEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isFootnoteEnabled = false;

		public bool IsFootnoteEnabled
		{
			get => _isFootnoteEnabled;
			set
			{
				_isFootnoteEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isMedialinkEnabled = true;

		public bool IsMedialinkEnabled
		{
			get => _isMedialinkEnabled;
			set
			{
				_isMedialinkEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isSoftlineAsHardlineBreakEnabled = false;

		public bool IsSoftlineAsHardlineBreakEnabled
		{
			get => _isSoftlineAsHardlineBreakEnabled;
			set
			{
				_isSoftlineAsHardlineBreakEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isSmartyPantEnabled = false;

		public bool IsSmartyPantEnabled
		{
			get => _isSmartyPantEnabled;
			set
			{
				_isSmartyPantEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isGenericAttributeEnabled = false;

		public bool IsGenericAttributeEnabled
		{
			get => _isGenericAttributeEnabled;
			set
			{
				_isGenericAttributeEnabled = value;
				OnPropertyChanged();
			}
		}

		#endregion Core

		#region Diagram

		private bool _isDiagramEnabled = false;

		public bool IsDiagramEnabled
		{
			get => _isDiagramEnabled;
			set
			{
				_isDiagramEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isMermaidEnabled = false;

		public bool IsMermaidEnabled
		{
			get => _isMermaidEnabled;
			set
			{
				_isMermaidEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isNomnomlEnabled = false;

		public bool IsNomnomlEnabled
		{
			get => _isNomnomlEnabled;
			set
			{
				_isNomnomlEnabled = value;
				OnPropertyChanged();
			}
		}

		#endregion Diagram

		#region Emphasis

		private bool _isEmphasisEnabled = false;

		public bool IsEmphasisEnabled
		{
			get => _isEmphasisEnabled;
			set
			{
				_isEmphasisEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isStrikethroughEnabled = false;

		public bool IsStrikethroughEnabled
		{
			get => _isStrikethroughEnabled;
			set
			{
				_isStrikethroughEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isSuperSubScriptEnabled = false;

		public bool IsSuperSubScriptEnabled
		{
			get => _isSuperSubScriptEnabled;
			set
			{
				_isSuperSubScriptEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isInsertedEnabled = false;

		public bool IsInsertedEnabled
		{
			get => _isInsertedEnabled;
			set
			{
				_isInsertedEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isMarkedEnabled = false;

		public bool IsMarkedEnabled
		{
			get => _isMarkedEnabled;
			set
			{
				_isMarkedEnabled = value;
				OnPropertyChanged();
			}
		}

		#endregion Emphasis

		#region List

		private bool _isListEnabled = false;

		public bool IsListEnabled
		{
			get => _isListEnabled;
			set
			{
				_isListEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isTaskListEnabled = false;

		public bool IsTaskListEnabled
		{
			get => _isTaskListEnabled;
			set
			{
				_isTaskListEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isListExtraEnabled = false;

		public bool IsListExtraEnabled
		{
			get => _isListExtraEnabled;
			set
			{
				_isListExtraEnabled = value;
				OnPropertyChanged();
			}
		}

		#endregion List

		#region Math

		private bool _isMathEnabled = false;

		public bool IsMathEnabled
		{
			get => _isMathEnabled;
			set
			{
				_isMathEnabled = value;
				OnPropertyChanged();
			}
		}

		#endregion Math

		#region SyntaxHighlighting

		private bool _isSyntaxHighlightingEnabled = false;

		public bool IsSyntaxHighlightingEnabled
		{
			get => _isSyntaxHighlightingEnabled;
			set
			{
				_isSyntaxHighlightingEnabled = value;
				OnPropertyChanged();
			}
		}

		#endregion SyntaxHighlighting

		#region Table

		private bool _isTableEnabled = false;

		public bool IsTableEnabled
		{
			get => _isTableEnabled;
			set
			{
				_isTableEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isGridTableEnabled = false;

		public bool IsGridTableEnabled
		{
			get => _isGridTableEnabled;
			set
			{
				_isGridTableEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isPipeTableEnabled = false;

		public bool IsPiepTableEnabled
		{
			get => _isPipeTableEnabled;
			set
			{
				_isPipeTableEnabled = value;
				OnPropertyChanged();
			}
		}

		#endregion Table
	}
}