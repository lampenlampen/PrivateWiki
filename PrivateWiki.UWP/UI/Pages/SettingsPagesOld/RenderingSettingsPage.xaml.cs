using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.DataModels;
using PrivateWiki.DataModels.Settings;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;
using PrivateWiki.UWP.UI.Controls.Settings.Rendering;

#nullable enable

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.UWP.UI.Pages.SettingsPagesOld
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class RenderingSettingsPage : Page
	{
		private readonly IRenderingMarkdownSettingsService _markdown;

		private List<RenderModel> RenderMarkdownToHtmlModels = new List<RenderModel>();

		public RenderingSettingsPage()
		{
			this.InitializeComponent();

			_markdown = ServiceLocator.Container.GetInstance<IRenderingMarkdownSettingsService>();

			LoadMarkdownToHtmlListItems();

			ApplicationData.Current.DataChanged += OnRoamingDataChanged;
		}

		private void OnRoamingDataChanged(ApplicationData sender, object args)
		{
			LoadMarkdownToHtmlListItems();
		}

		private async Task LoadMarkdownToHtmlListItems()
		{
			var model = await _markdown.GetRenderingMarkdownSettingsModelAsync();

			var coreRenderModel = new CoreRenderModel
			{
				IsEnabled = true,
				IsHtmlTagEnabled = model.IsHtmlEnabled,
				IsAbbreviationEnabled = model.IsAbbreviationEnabled,
				IsAutoIdentifierEnabled = model.IsAutoIdentifierEnabled,
				IsAutoLinkEnabled = model.IsAutoLinkEnabled,
				IsBootstrapEnabled = model.IsBootstrapEnabled,
				IsCitationEnabled = model.IsCitationEnabled,
				IsDefinitionListEnabled = model.IsDefinitionListEnabled,
				IsEmojiSmileyEnabled = model.IsEmojiSmileyEnabled,
				IsFigureEnabled = model.IsFigureEnabled,
				IsFooterEnabled = model.IsFooterEnabled,
				IsFootnoteEnabled = model.IsFootnoteEnabled,
				IsMediaLinkEnabled = model.IsMedialinkEnabled,
				IsSoftLineAsHardlineBreakEnabled = model.IsSoftlineAsHardlineBreakEnabled,
				IsSmartyPantEnabled = model.IsSmartyPantEnabled,
				IsGenericAttributeEnabled = model.IsGenericAttributeEnabled
			};

			RenderMarkdownToHtmlModels.Add(coreRenderModel);

			var emphasisExtraRenderModel = new EmphasisExtraRenderModel
			{
				IsEnabled = model.IsEmphasisEnabled,
				IsStrikethroughEnabled = model.IsStrikethroughEnabled,
				IsSuperSubScriptEnabled = model.IsSuperSubScriptEnabled,
				IsInsertedEnabled = model.IsInsertedEnabled,
				IsMarkedEnabled = model.IsMarkedEnabled
			};
			RenderMarkdownToHtmlModels.Add(emphasisExtraRenderModel);

			var tableRenderModel = new TableRenderModel
			{
				IsEnabled = model.IsTableEnabled,
				IsGridTableEnabled = model.IsGridTableEnabled,
				IsPipeTableEnabled = model.IsPipeTableEnabled
			};

			RenderMarkdownToHtmlModels.Add(tableRenderModel);

			var listRenderModel = new ListRenderModel
			{
				IsEnabled = model.IsListEnabled,
				IsTaskListEnabled = model.IsTaskListEnabled,
				IsListExtraEnabled = model.IsListExtraEnabled
			};
			RenderMarkdownToHtmlModels.Add(listRenderModel);

			var mathRenderModel = new MathRenderModel
			{
				IsEnabled = model.IsMathEnabled
			};

			RenderMarkdownToHtmlModels.Add(mathRenderModel);

			var syntaxHighlightingRenderModel = new SyntaxHighlightingRenderModel
			{
				IsEnabled = model.IsSyntaxHighlightingEnabled
			};

			RenderMarkdownToHtmlModels.Add(syntaxHighlightingRenderModel);

			var diagramRenderModel = new DiagramRenderModel
			{
				IsEnabled = model.IsDiagramEnabled,
				IsMermaidEnabled = model.IsMermaidEnabled,
				IsNomnomlEnabled = model.IsNomnomlEnabled
			};

			RenderMarkdownToHtmlModels.Add(diagramRenderModel);
		}

		private async Task SaveRenderingMarkdownModel()
		{
			var model = new RenderingMarkdownSettingsModel();

			foreach (var renderModel in RenderMarkdownToHtmlModels)
			{
				switch (renderModel.Type)
				{
					case RenderMarkdownToHtmlType.Core:
					{
						var sourceModel = (CoreRenderModel) renderModel;
						model.IsHtmlEnabled = sourceModel.IsHtmlTagEnabled;
						model.IsAbbreviationEnabled = sourceModel.IsAbbreviationEnabled;
						model.IsAutoIdentifierEnabled = sourceModel.IsAutoIdentifierEnabled;
						model.IsAutoLinkEnabled = sourceModel.IsAutoLinkEnabled;
						model.IsBootstrapEnabled = sourceModel.IsBootstrapEnabled;
						model.IsCitationEnabled = sourceModel.IsCitationEnabled;
						model.IsDefinitionListEnabled = sourceModel.IsDefinitionListEnabled;
						model.IsEmojiSmileyEnabled = sourceModel.IsEmojiSmileyEnabled;
						model.IsFigureEnabled = sourceModel.IsFigureEnabled;
						model.IsFooterEnabled = sourceModel.IsFooterEnabled;
						model.IsFootnoteEnabled = sourceModel.IsFootnoteEnabled;
						model.IsMedialinkEnabled = sourceModel.IsMediaLinkEnabled;
						model.IsSoftlineAsHardlineBreakEnabled = sourceModel.IsSoftLineAsHardlineBreakEnabled;
						model.IsSmartyPantEnabled = sourceModel.IsSmartyPantEnabled;
						model.IsGenericAttributeEnabled = sourceModel.IsGenericAttributeEnabled;
						break;
					}
					case RenderMarkdownToHtmlType.CustomContainer:
					{
						break;
					}
					case RenderMarkdownToHtmlType.EmphasisExtra:
					{
						var sourceModel = (EmphasisExtraRenderModel) renderModel;

						model.IsEmphasisEnabled = sourceModel.IsEnabled;

						model.IsStrikethroughEnabled = sourceModel.IsStrikethroughEnabled;
						model.IsSuperSubScriptEnabled = sourceModel.IsSuperSubScriptEnabled;
						model.IsInsertedEnabled = sourceModel.IsInsertedEnabled;
						model.IsMarkedEnabled = sourceModel.IsMarkedEnabled;
						break;
					}
					case RenderMarkdownToHtmlType.List:
					{
						var sourceModel = (ListRenderModel) renderModel;

						model.IsListEnabled = sourceModel.IsEnabled;
						model.IsTaskListEnabled = sourceModel.IsTaskListEnabled;
						model.IsListExtraEnabled = sourceModel.IsListExtraEnabled;

						break;
					}
					case RenderMarkdownToHtmlType.Mathematics:
					{
						var sourceModel = (MathRenderModel) renderModel;

						model.IsMathEnabled = sourceModel.IsEnabled;

						break;
					}
					case RenderMarkdownToHtmlType.Table:
					{
						var sourceModel = (TableRenderModel) renderModel;

						model.IsTableEnabled = sourceModel.IsEnabled;

						model.IsGridTableEnabled = sourceModel.IsGridTableEnabled;
						model.IsPipeTableEnabled = sourceModel.IsPipeTableEnabled;

						break;
					}
					case RenderMarkdownToHtmlType.YamlFrontMatter:
						break;
					case RenderMarkdownToHtmlType.SyntaxHighlighting:
					{
						var sourceModel = (SyntaxHighlightingRenderModel) renderModel;

						model.IsSyntaxHighlightingEnabled = sourceModel.IsEnabled;
						break;
					}
					case RenderMarkdownToHtmlType.Diagram:
					{
						var sourceModel = (DiagramRenderModel) renderModel;

						model.IsDiagramEnabled = sourceModel.IsEnabled;

						model.IsMermaidEnabled = sourceModel.IsMermaidEnabled;
						model.IsNomnomlEnabled = sourceModel.IsNomnomlEnabled;

						break;
					}

					case RenderMarkdownToHtmlType.Globalization:
						break;
					case RenderMarkdownToHtmlType.JiraLinks:
						break;
					case RenderMarkdownToHtmlType.PreciseSourceLocation:
						break;
					case RenderMarkdownToHtmlType.SelfPipeline:
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(renderModel));
				}
			}

			await _markdown.SaveRenderingMarkdownSettingsModelAsync(model);

			RenderMarkdownToHtmlModels.Clear();

			await LoadMarkdownToHtmlListItems();
		}

		private void MarkdownHtmlExpander_OnExpanded(object sender, EventArgs e)
		{
			//HtmlExpander.IsExpanded = false;
		}

		private void ListviewMarkdownHtml_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RenderingSettingsItemContent.Children.Clear();

			var item = (RenderModel) ((ListView) sender).SelectedItem;

			switch (item)
			{
				case CoreRenderModel coreRenderModel:
					var coreControl = new CoreControl();
					coreControl.Init(coreRenderModel);
					RenderingSettingsItemContent.Children.Add(coreControl);
					break;
				case EmphasisExtraRenderModel emphasisExtraModel:
					var emphasisExtraControl = new EmphasisExtraControl();
					emphasisExtraControl.Init(emphasisExtraModel);
					RenderingSettingsItemContent.Children.Add(emphasisExtraControl);
					break;
				case TableRenderModel tableModel:
					var tableControl = new TableControl();
					tableControl.Init(tableModel);
					RenderingSettingsItemContent.Children.Add(tableControl);
					break;
				case ListRenderModel listModel:
					var listControl = new ListControl();
					listControl.Init(listModel);
					RenderingSettingsItemContent.Children.Add(listControl);
					break;
				case MathRenderModel mathModel:
					var mathControl = new MathControl();
					mathControl.Init(mathModel);
					RenderingSettingsItemContent.Children.Add(mathControl);
					break;
				case SyntaxHighlightingRenderModel syntaxHighlightingModel:
					var syntaxHighlightingControl = new SyntaxHighlightingControl();
					syntaxHighlightingControl.Init(syntaxHighlightingModel);
					RenderingSettingsItemContent.Children.Add(syntaxHighlightingControl);
					break;
				case DiagramRenderModel diagramModel:
					var diagramControl = new DiagramControl();
					diagramControl.Init(diagramModel);
					RenderingSettingsItemContent.Children.Add(diagramControl);
					break;
				default:
					// TODO Error UI model does not exist
					// TODO Analytics
					break;
			}
		}

		private void SettingsHeader_OnApplyClick(object sender, RoutedEventArgs e)
		{
			SaveRenderingMarkdownModel();
		}

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e)
		{
			LoadMarkdownToHtmlListItems();
		}
	}
}