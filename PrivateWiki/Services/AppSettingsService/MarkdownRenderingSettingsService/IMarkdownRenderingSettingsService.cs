using System;

namespace PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService
{
	[Obsolete("", true)]
	public interface IMarkdownRenderingSettingsService
	{
		#region Core

		public bool IsHtmlEnabled { get; set; }

		public bool IsAbbreviationEnabled { get; set; }


		public bool IsAutoIdentifierEnabled { get; set; }


		public bool IsAutoLinkEnabled { get; set; }

		public bool IsBootstrapEnabled { get; set; }

		public bool IsCitationEnabled { get; set; }

		public bool IsDefinitionListEnabled { get; set; }

		public bool IsEmojiSmileyEnabled { get; set; }

		public bool IsFigureEnabled { get; set; }

		public bool IsFooterEnabled { get; set; }

		public bool IsFootnoteEnabled { get; set; }

		public bool IsMedialinkEnabled { get; set; }

		public bool IsSoftlineAsHardlineBreakEnabled { get; set; }

		public bool IsSmartyPantEnabled { get; set; }

		public bool IsGenericAttributeEnabled { get; set; }

		#endregion Core

		#region Diagram

		public bool IsDiagramEnabled { get; set; }

		public bool IsMermaidEnabled { get; set; }

		public bool IsNomnomlEnabled { get; set; }

		#endregion Diagram

		#region Emphasis

		public bool IsEmphasisEnabled { get; set; }

		public bool IsStrikethroughEnabled { get; set; }

		public bool IsSuperSubScriptEnabled { get; set; }

		public bool IsInsertedEnabled { get; set; }

		public bool IsMarkedEnabled { get; set; }

		#endregion Emphasis

		#region List

		public bool IsListEnabled { get; set; }

		public bool IsTaskListEnabled { get; set; }

		public bool IsListExtraEnabled { get; set; }

		#endregion List

		#region Math

		public bool IsMathEnabled { get; set; }

		#endregion Math

		#region SyntaxHighlighting

		public bool IsSyntaxHighlightingEnabled { get; set; }

		#endregion SyntaxHighlighting

		#region Table

		public bool IsTableEnabled { get; set; }

		public bool IsGridTableEnabled { get; set; }

		public bool IsPiepTableEnabled { get; set; }

		#endregion Table
	}
}