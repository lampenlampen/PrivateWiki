using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Models;
using PrivateWiki.UWP.Settings;
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
		private List<RenderModel> RenderMarkdownToHtmlModels = new List<RenderModel>();

		private List<RenderModel> RenderHtmlModels = new List<RenderModel>();

		public RenderingSettingsPage()
		{
			this.InitializeComponent();
			LoadMarkdownToHtmlListItems();

			ApplicationData.Current.DataChanged += OnRoamingDataChanged;
		}

		private void OnRoamingDataChanged(ApplicationData sender, object args)
		{
			LoadMarkdownToHtmlListItems();
		}

		private void LoadMarkdownToHtmlListItems()
		{
			RenderMarkdownToHtmlModels.Clear();

			/*
			RenderMarkdownToHtmlModels.Add(new CoreRenderModel());
			RenderMarkdownToHtmlModels.Add(new EmphasisExtraRenderModel());
			RenderMarkdownToHtmlModels.Add(new TableRenderModel());
			RenderMarkdownToHtmlModels.Add(new ListRenderModel());
			RenderMarkdownToHtmlModels.Add(new MathRenderModel());
			RenderMarkdownToHtmlModels.Add(new SyntaxHighlightingRenderModel());
			RenderMarkdownToHtmlModels.Add(new DiagramRenderModel());
			*/


			/*
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Yaml Frontmatter", Subtitle = "", Type = RenderMarkdownToHtmlType.YamlFrontMatter });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Globalization", Subtitle = "", Type = RenderMarkdownToHtmlType.Globalization });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Jira Links", Subtitle = "", Type = RenderMarkdownToHtmlType.JiraLinks });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Precise Source Location", Subtitle = "", Type = RenderMarkdownToHtmlType.PreciseSourceLocation });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Self Pipeline", Subtitle = "", Type = RenderMarkdownToHtmlType.SelfPipeline });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Custom Container", Subtitle = "", Type = RenderMarkdownToHtmlType.CustomContainer });
			*/

			var handler = new RenderingModelHandler();

			try
			{
				var models = handler.LoadRenderingModels().ToList();

				RenderMarkdownToHtmlModels = models;
			}
			catch (NullReferenceException e)
			{
				Console.WriteLine(e);

				RenderMarkdownToHtmlModels.Clear();

				RenderMarkdownToHtmlModels.Add(new CoreRenderModel());
				RenderMarkdownToHtmlModels.Add(new EmphasisExtraRenderModel());
				RenderMarkdownToHtmlModels.Add(new TableRenderModel());
				RenderMarkdownToHtmlModels.Add(new ListRenderModel());
				RenderMarkdownToHtmlModels.Add(new MathRenderModel());
				RenderMarkdownToHtmlModels.Add(new SyntaxHighlightingRenderModel());
				RenderMarkdownToHtmlModels.Add(new DiagramRenderModel());
			}
		}

		private void SaveRenderingOptions()
		{
			var handler = new RenderingModelHandler();
			handler.SaveRenderingModels(RenderMarkdownToHtmlModels);
		}

		private void HtmlExpander_OnExpanded(object sender, EventArgs e)
		{
			MarkdownHtmlExpander.IsExpanded = false;
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

		private void ListviewHtml_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// TODO
		}

		private void SettingsHeader_OnApplyClick(object sender, RoutedEventArgs e)
		{
			SaveRenderingOptions();
		}

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e)
		{
			LoadMarkdownToHtmlListItems();
		}
	}
}