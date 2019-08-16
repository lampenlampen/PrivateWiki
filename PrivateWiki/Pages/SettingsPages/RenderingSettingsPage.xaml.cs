using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PrivateWiki.Controls.Settings.Rendering;
using PrivateWiki.Models;

#nullable enable

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Pages.SettingsPages
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
		}

		private void LoadMarkdownToHtmlListItems()
		{
			RenderMarkdownToHtmlModels.Add(new CoreRenderModel());
			RenderMarkdownToHtmlModels.Add(new EmphasisExtraModel());
			RenderMarkdownToHtmlModels.Add(new TableRenderModel());
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "List", Subtitle = "", Type = RenderMarkdownToHtmlType.List });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Mathematics", Subtitle = "", Type = RenderMarkdownToHtmlType.Mathmatics });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Yaml Frontmatter", Subtitle = "", Type = RenderMarkdownToHtmlType.YamlFrontMatter });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Syntax Highlighting", Subtitle = "", Type = RenderMarkdownToHtmlType.SyntaxHighlighting });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Diagrams", Subtitle = "", Type = RenderMarkdownToHtmlType.Diagram });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Globalization", Subtitle = "", Type = RenderMarkdownToHtmlType.Globalization });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Jira Links", Subtitle = "", Type = RenderMarkdownToHtmlType.JiraLinks });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Precise Source Location", Subtitle = "", Type = RenderMarkdownToHtmlType.PreciseSourceLocation });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Self Pipeline", Subtitle = "", Type = RenderMarkdownToHtmlType.SelfPipeline });
			RenderMarkdownToHtmlModels.Add(new RenderModel { Title = "Custom Container", Subtitle = "", Type = RenderMarkdownToHtmlType.CustomContainer });

		}

		private void SaveRenderingOptions()
		{

		}

		private void HtmlExpander_OnExpanded(object sender, EventArgs e)
		{
			MarkdownHtmlExpander.IsExpanded = false;
		}

		private void MarkdownHtmlExpander_OnExpanded(object sender, EventArgs e)
		{
			HtmlExpander.IsExpanded = false;
		}

		private void ListviewMarkdownHtml_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RenderingSettingsItemContent.Children.Clear();

			var item = (RenderModel)((ListView)sender).SelectedItem;

			switch (item)
			{
				case CoreRenderModel coreRenderModel:
					var coreControl = new CoreControl();
					coreControl.Init(coreRenderModel);
					RenderingSettingsItemContent.Children.Add(coreControl);
					break;
				case EmphasisExtraModel emphasisExtraModel:
					var emphasisExtraControl = new EmphasisExtraControl();
					emphasisExtraControl.Init(emphasisExtraModel);
					RenderingSettingsItemContent.Children.Add(emphasisExtraControl);
					break;
				case TableRenderModel tableModel:
					var tableControl = new TableControl();
					tableControl.Init(tableModel);
					RenderingSettingsItemContent.Children.Add(tableControl);
					break;
				default:
					break;
			}
		}

		private void ListviewHtml_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// TODO
		}
	}
}
