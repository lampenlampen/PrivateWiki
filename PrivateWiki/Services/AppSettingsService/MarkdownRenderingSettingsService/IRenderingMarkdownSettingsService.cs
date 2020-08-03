using System.Threading.Tasks;

namespace PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService
{
	public interface IRenderingMarkdownSettingsService
	{
		#region Core

		Task IsHtmlEnabled(bool value);
		Task<bool> IsHtmlEnabled();

		Task IsAbbreviationEnabled(bool value);
		Task<bool> IsAbbreviationEnabled();


		Task IsAutoIdentifierEnabled(bool value);
		Task<bool> IsAutoIdentifierEnabled();


		Task IsAutoLinkEnabled(bool value);
		Task<bool> IsAutoLinkEnabled();

		Task IsBootstrapEnabled(bool value);
		Task<bool> IsBootstrapEnabled();

		Task IsCitationEnabled(bool value);
		Task<bool> IsCitationEnabled();

		Task IsDefinitionListEnabled(bool value);
		Task<bool> IsDefinitionListEnabled();

		Task IsEmojiSmileyEnabled(bool value);
		Task<bool> IsEmojiSmileyEnabled();

		Task IsFigureEnabled(bool value);
		Task<bool> IsFigureEnabled();

		Task IsFooterEnabled(bool value);
		Task<bool> IsFooterEnabled();

		Task IsFootnoteEnabled(bool value);
		Task<bool> IsFootnoteEnabled();

		Task IsMedialinkEnabled(bool value);
		Task<bool> IsMedialinkEnabled();

		Task IsSoftlineAsHardlineBreakEnabled(bool value);
		Task<bool> IsSoftlineAsHardlineBreakEnabled();

		Task IsSmartyPantEnabled(bool value);
		Task<bool> IsSmartyPantEnabled();

		Task IsGenericAttributeEnabled(bool value);
		Task<bool> IsGenericAttributeEnabled();

		#endregion Core

		#region Diagram

		Task IsDiagramEnabled(bool value);
		Task<bool> IsDiagramEnabled();

		Task IsMermaidEnabled(bool value);
		Task<bool> IsMermaidEnabled();

		Task IsNomnomlEnabled(bool value);
		Task<bool> IsNomnomlEnabled();

		#endregion Diagram

		#region Emphasis

		Task IsEmphasisEnabled(bool value);
		Task<bool> IsEmphasisEnabled();

		Task IsStrikethroughEnabled(bool value);
		Task<bool> IsStrikethroughEnabled();

		Task IsSuperSubScriptEnabled(bool value);
		Task<bool> IsSuperSubScriptEnabled();

		Task IsInsertedEnabled(bool value);
		Task<bool> IsInsertedEnabled();

		Task IsMarkedEnabled(bool value);
		Task<bool> IsMarkedEnabled();

		#endregion Emphasis

		#region List

		Task IsListEnabled(bool value);
		Task<bool> IsListEnabled();

		Task IsTaskListEnabled(bool value);
		Task<bool> IsTaskListEnabled();

		Task IsListExtraEnabled(bool value);
		Task<bool> IsListExtraEnabled();

		#endregion List

		#region Math

		Task IsMathEnabled(bool value);
		Task<bool> IsMathEnabled();

		#endregion Math

		#region SyntaxHighlighting

		Task IsSyntaxHighlightingEnabled(bool value);
		Task<bool> IsSyntaxHighlightingEnabled();

		#endregion SyntaxHighlighting

		#region Table

		Task IsTableEnabled(bool value);
		Task<bool> IsTableEnabled();

		Task IsGridTableEnabled(bool value);
		Task<bool> IsGridTableEnabled();

		Task IsPiepTableEnabled(bool value);
		Task<bool> IsPiepTableEnabled();

		#endregion Table

	}
}