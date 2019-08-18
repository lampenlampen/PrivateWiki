using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Markdig.Extensions.Tables;
using PrivateWiki.Models;

#nullable enable

namespace PrivateWiki.Settings
{
	class RenderingModelHandler
	{
		public bool SaveRenderingModels(IEnumerable<RenderModel> models)
		{
			var renderingContainer = GetRenderingSettingsContainer();

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
					case EmphasisExtraRenderModel emphasisExtraModel:
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
			container.Values["core_is_medialink_enabled"] = model.IsMediaLinkEnabled;
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

		private bool SaveEmphasisExtraModel(ApplicationDataContainer container, EmphasisExtraRenderModel model)
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

		private bool SaveSyntaxHighlightingModel(ApplicationDataContainer container,
			SyntaxHighlightingRenderModel model)
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
			container.Values["table_is_gridtable_enabled"] = model.IsGridTableEnabled;
			container.Values["table_is_pipetable_enabled"] = model.IsPipeTableEnabled;

			return true;
		}

		public IEnumerable<RenderModel> LoadRenderingModels()
		{
			var renderingContainer = GetRenderingSettingsContainer();

			var models = new List<RenderModel>
			{
				LoadCoreModel(renderingContainer),
				LoadDiagramModel(renderingContainer),
				LoadEmphasisExtraModel(renderingContainer),
				LoadListModel(renderingContainer),
				LoadMathModel(renderingContainer),
				LoadSyntaxHighlightingModel(renderingContainer),
				LoadTableModel(renderingContainer)
			};

			return models;
		}

		private CoreRenderModel LoadCoreModel(ApplicationDataContainer container)
		{
			var model = new CoreRenderModel
			{
				FontGlyph = (string) container.Values["core_fontglyph"],
				FontFamily = (string) container.Values["core_fontfamily"],
				Title = (string) container.Values["core_title"],
				Subtitle = (string) container.Values["core_subtitle"],
				IsEnabled = (bool) container.Values["core_enabled"],
				IsHtmlTagEnabled = (bool) container.Values["core_is_html_enabled"],
				IsAbbreviationEnabled = (bool) container.Values["core_is_abbreviation_enabled"],
				IsAutoLinkEnabled = (bool) container.Values["core_is_autolink_enabled"],
				IsBootstrapEnabled = (bool) container.Values["core_is_bootstrap_enabled"],
				IsCitationEnabled = (bool) container.Values["core_is_citation_enabled"],
				IsDefinitionListEnabled = (bool) container.Values["core_is_definitionlist_enabled"],
				IsEmojiSmileyEnabled = (bool) container.Values["core_is_emojismiley_enabled"],
				IsFigureEnabled = (bool) container.Values["core_is_figure_enabled"],
				IsFooterEnabled = (bool) container.Values["core_is_footer_enabled"],
				IsFootnoteEnabled = (bool) container.Values["core_is_footnote_enabled"],
				IsMediaLinkEnabled = (bool) container.Values["core_is_medialink_enabled"],
				IsSoftLineAsHardlineBreakEnabled =
					(bool) container.Values["core_is_softlineashardlinebreak_enabled"],
				IsSmartyPantEnabled = (bool) container.Values["core_is_smartypant_enabled"],
				IsGenericAttributeEnabled = (bool) container.Values["core_is_genericattribute_enabled"]
			};

			return model;
		}

		private DiagramRenderModel LoadDiagramModel(ApplicationDataContainer container)
		{
			var model = new DiagramRenderModel
			{
				FontGlyph = (string) container.Values["diagram_fontglyph"],
				FontFamily = (string) container.Values["diagram_fontfamily"],
				Title = (string) container.Values["diagram_title"],
				Subtitle = (string) container.Values["diagram_subtitle"],
				IsEnabled = (bool) container.Values["diagram_enabled"],
				IsMermaidEnabled = (bool) container.Values["diagram_is_mermaid_enabled"],
				IsNomnomlEnabled = (bool) container.Values["diagram_is_nomnoml_enabled"]
			};


			return model;
		}

		private EmphasisExtraRenderModel LoadEmphasisExtraModel(ApplicationDataContainer container)
		{
			var model = new EmphasisExtraRenderModel
			{
				FontGlyph = (string) container.Values["emphasis_fontglyph"],
				FontFamily = (string) container.Values["emphasis_fontfamily"],
				Title = (string) container.Values["emphasis_title"],
				Subtitle = (string) container.Values["emphasis_subtitle"],
				IsEnabled = (bool) container.Values["emphasis_enabled"],
				IsStrikethroughEnabled = (bool) container.Values["emphasis_is_strikethrough_enabled"],
				IsSuperSubScriptEnabled = (bool) container.Values["emphasis_is_supersubscript_enabled"],
				IsInsertedEnabled = (bool) container.Values["emphasis_is_inserted_enabled"],
				IsMarkedEnabled = (bool) container.Values["emphasis_is_marked_enabled"]
			};


			return model;
		}

		private ListRenderModel LoadListModel(ApplicationDataContainer container)
		{
			var model = new ListRenderModel
			{
				FontGlyph = (string) container.Values["list_fontglyph"],
				FontFamily = (string) container.Values["list_fontfamily"],
				Title = (string) container.Values["list_title"],
				Subtitle = (string) container.Values["list_subtitle"],
				IsEnabled = (bool) container.Values["list_enabled"],
				IsTaskListEnabled = (bool) container.Values["list_is_tasklist_enabled"],
				IsListExtraEnabled = (bool) container.Values["list_is_listextra_enabled"]
			};


			return model;
		}

		private MathRenderModel LoadMathModel(ApplicationDataContainer container)
		{
			var model = new MathRenderModel
			{
				FontGlyph = (string) container.Values["math_fontglyph"],
				FontFamily = (string) container.Values["math_fontfamily"],
				Title = (string) container.Values["math_title"],
				Subtitle = (string) container.Values["math_subtitle"],
				IsEnabled = (bool) container.Values["math_enabled"]
			};


			return model;
		}

		private SyntaxHighlightingRenderModel LoadSyntaxHighlightingModel(ApplicationDataContainer container)
		{
			var model = new SyntaxHighlightingRenderModel
			{
				FontGlyph = (string) container.Values["syntaxhighlighting_fontglyph"],
				FontFamily = (string) container.Values["syntaxhighlighting_fontfamily"],
				Title = (string) container.Values["syntaxhighlighting_title"],
				Subtitle = (string) container.Values["syntaxhighlighting_subtitle"],
				IsEnabled = (bool) container.Values["syntaxhighlighting_enabled"]
			};


			return model;
		}

		private TableRenderModel LoadTableModel(ApplicationDataContainer container)
		{
			var model = new TableRenderModel
			{
				FontGlyph = (string) container.Values["table_fontglyph"],
				FontFamily = (string) container.Values["table_fontfamily"],
				Title = (string) container.Values["table_title"],
				Subtitle = (string) container.Values["table_subtitle"],
				IsEnabled = (bool) container.Values["table_enabled"],
				IsGridTableEnabled = (bool) container.Values["table_is_gridtable_enabled"],
				IsPipeTableEnabled = (bool) container.Values["table_is_pipetable_enabled"]
			};

			return model;
		}

		private ApplicationDataContainer GetRenderingSettingsContainer()
		{
			var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
			var settingsContainer =
				roamingSettings.CreateContainer("settings", ApplicationDataCreateDisposition.Always);

			var renderingContainer =
				settingsContainer.CreateContainer("rendering", ApplicationDataCreateDisposition.Always);

			return renderingContainer;
		}
	}
}