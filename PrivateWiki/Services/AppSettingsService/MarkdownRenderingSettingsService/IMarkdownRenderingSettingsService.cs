using System.ComponentModel;

namespace PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService
{
	public interface IMarkdownRenderingSettingsService
	{
		#region Core

		public bool IsHtmlEnabled { get; }

		public bool IsAbbreviationEnabled { get; }


		public bool IsAutoIdentifierEnabled { get; }


		public bool IsAutoLinkEnabled { get; }

		public bool IsBootstrapEnabled { get; }


		public bool IsCitationEnabled { get; }

		public bool IsDefinitionListEnabled { get; }

		public bool IsEmojiSmileyEnabled { get; }

		public bool IsFigureEnabled { get; }

		public bool IsFooterEnabled { get; }

		public bool IsFootnoteEnabled { get; }

		public bool IsMedialinkEnabled { get; }

		public bool IsSoftlineAsHardlineBreakEnabled { get; }

		public bool IsSmartyPantEnabled { get; }

		public bool IsGenericAttributeEnabled { get; }

		#endregion Core

		#region Diagram

		public bool IsDiagramEnabled { get; }

		public bool IsMermaidEnabled { get; }

		public bool IsNomnomlEnabled { get; }

		#endregion Diagram

		#region Emphasis

		public bool IsEmphasisEnabled { get; }

		public bool IsStrikethroughEnabled { get; }

		public bool IsSuperSubScriptEnabled { get; }

		public bool IsInsertedEnabled { get; }

		public bool IsMarkedEnabled { get; }

		#endregion Emphasis

		#region List

		public bool IsListEnabled { get; }

		public bool IsTaskListEnabled { get; }

		public bool IsListExtraEnabled { get; }

		#endregion List

		#region Math

		public bool IsMathEnabled { get; }

		#endregion Math

		#region SyntaxHighlighting

		public bool IsSyntaxHighlightingEnabled { get; }

		#endregion SyntaxHighlighting

		#region Table

		public bool IsTableEnabled { get; }

		public bool IsGridTableEnabled { get; }

		public bool IsPiepTableEnabled { get; }

		#endregion Table
	}
}