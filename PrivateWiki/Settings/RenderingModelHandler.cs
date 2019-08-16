using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using PrivateWiki.Models;

#nullable enable

namespace PrivateWiki.Settings
{
	class RenderingModelHandler
	{
		public bool SaveRenderingModels(IEnumerable<RenderModel> models)
		{
			var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
			var settingsContainer =
				roamingSettings.CreateContainer("settings", ApplicationDataCreateDisposition.Always);

			var renderingContainer =
				settingsContainer.CreateContainer("rendering", ApplicationDataCreateDisposition.Always);

			foreach (var model in models)
			{
				switch (model)
				{
					case CoreRenderModel coreRenderModel:
						SaveCoreModel(renderingContainer, coreRenderModel);
						break;
					case DiagramRenderModel diagramRenderModel:
						SaveDiagramModel(renderingContainer, diagramRenderModel);
						break;
					case EmphasisExtraModel emphasisExtraModel:
						SaveEmphasisExtraModel(renderingContainer, emphasisExtraModel);
						break;
					case ListRenderModel listRenderModel:
						SaveListModel(renderingContainer, listRenderModel);
						break;
					case MathRenderModel mathRenderModel:
						SaveMathModel(renderingContainer, mathRenderModel);
						break;
					case SyntaxHighlightingRenderModel syntaxHighlightingRenderModel:
						SaveSyntaxHighlightingModel(renderingContainer, syntaxHighlightingRenderModel);
						break;
					case TableRenderModel tableRenderModel:
						SaveTableModel(renderingContainer, tableRenderModel);
						break;
					case RenderModel renderModel:
						//TODO
						break;
				}
			}
			
			return true;
		}

		private bool SaveCoreModel(ApplicationDataContainer container, CoreRenderModel model)
		{
			container.Values["core_fontglyph"] = model.FontGlyph;
			container.Values["core_fontfamily"] = model.FontFamily;
			container.Values["core_title"] = model.Title;
			container.Values["core_subtitle"] = model.Subtitle;
			container.Values["core_enabled"] = model.IsEnabled;
			container.Values["core_is_html_enabled"] = model.IsHtmlTagEnabled;
			container.Values["core_is_abbreviation_enabled"] = model.IsAbbreviationEnabled;
			container.Values["core_is_autolink_enabled"] = model.IsAutoLinkEnabled;
			container.Values["core_is_bootstrap_enabled"] = model.IsBootstrapEnabled;
			container.Values["core_is_citation_enabled"] = model.IsCitationEnabled;
			container.Values["core_is_definitionlist_enabled"] = model.IsDefinitionListEnabled;
			container.Values["core_is_emojismiley_enabled"] = model.IsEmojiSmileyEnabled;
			container.Values["core_is_figure_enabled"] = model.IsFigureEnabled;
			container.Values["core_is_footer_enabled"] = model.IsFooterEnabled;
			container.Values["core_is_footnote_enabled"] = model.IsFootnoteEnabled;
			container.Values["core_is_meidalink_enabled"] = model.IsMediaLinkEnabled;
			container.Values["core_is_softlineashardlinebreak_enabled"] = model.IsSoftLineAsHardlineBreakEnabled;
			container.Values["core_is_smartypant_enabled"] = model.IsSmartyPantEnabled;
			container.Values["core_is_genericattribute_enabled"] = model.IsGenericAttributeEnabled;

			return true;
		}

		private bool SaveDiagramModel(ApplicationDataContainer container, DiagramRenderModel model)
		{
			container.Values["diagram_fontglyph"] = model.FontGlyph;
			container.Values["diagram_fontfamily"] = model.FontFamily;
			container.Values["diagram_title"] = model.Title;
			container.Values["diagram_subtitle"] = model.Subtitle;
			container.Values["diagram_enabled"] = model.IsEnabled;
			container.Values["diagram_is_mermaid_enabled"] = model.IsMermaidEnabled;
			container.Values["diagram_is_nomnoml_enabled"] = model.IsNomnomlEnabled;

			return true;
		}

		private bool SaveEmphasisExtraModel(ApplicationDataContainer container, EmphasisExtraModel model)
		{
			container.Values["emphasis_fontglyph"] = model.FontGlyph;
			container.Values["emphasis_fontfamily"] = model.FontFamily;
			container.Values["emphasis_title"] = model.Title;
			container.Values["emphasis_subtitle"] = model.Subtitle;
			container.Values["emphasis_enabled"] = model.IsEnabled;
			container.Values["emphasis_is_strikethrough_enabled"] = model.IsStrikethroughEnabled;
			container.Values["emphasis_is_supersubscript_enabled"] = model.IsSuperSubScriptEnabled;
			container.Values["emphasis_is_inserted_enabled"] = model.IsInsertedEnabled;
			container.Values["emphasis_is_marked_enabled"] = model.IsMarkedEnabled;

			return true;
		}

		private bool SaveListModel(ApplicationDataContainer container, ListRenderModel model)
		{
			container.Values["list_fontglyph"] = model.FontGlyph;
			container.Values["list_fontfamily"] = model.FontFamily;
			container.Values["list_title"] = model.Title;
			container.Values["list_subtitle"] = model.Subtitle;
			container.Values["list_enabled"] = model.IsEnabled;
			container.Values["list_is_tasklist_enabled"] = model.IsTaskListEnabled;
			container.Values["list_is_listextra_enabled"] = model.IsListExtraEnabled;

			return true;
		}

		private bool SaveMathModel(ApplicationDataContainer container, MathRenderModel model)
		{
			container.Values["math_fontglyph"] = model.FontGlyph;
			container.Values["math_fontfamily"] = model.FontFamily;
			container.Values["math_title"] = model.Title;
			container.Values["math_subtitle"] = model.Subtitle;
			container.Values["math_enabled"] = model.IsEnabled;

			return true;
		}

		private bool SaveSyntaxHighlightingModel(ApplicationDataContainer container, SyntaxHighlightingRenderModel model)
		{
			container.Values["syntaxhighlighting_fontglyph"] = model.FontGlyph;
			container.Values["syntaxhighlighting_fontfamily"] = model.FontFamily;
			container.Values["syntaxhighlighting_title"] = model.Title;
			container.Values["syntaxhighlighting_subtitle"] = model.Subtitle;
			container.Values["syntaxhighlighting_enabled"] = model.IsEnabled;

			return true;
		}

		private bool SaveTableModel(ApplicationDataContainer container, TableRenderModel model)
		{
			container.Values["table_fontglyph"] = model.FontGlyph;
			container.Values["table_fontfamily"] = model.FontFamily;
			container.Values["table_title"] = model.Title;
			container.Values["table_subtitle"] = model.Subtitle;
			container.Values["table_enabled"] = model.IsEnabled;

			return true;
		}
	}
}
