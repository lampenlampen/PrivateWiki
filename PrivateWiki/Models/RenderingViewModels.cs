using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace PrivateWiki.Models
{
	public class RenderModel
	{
		// TODO Icon
		public string FontGlyph { get; set; } = "\uE11D";

		public string FontFamily { get; set; } = "Segoe MDL2 Assets";

		public string Title { get; set; }

		public string Subtitle { get; set; }

		public string Explanation { get; set; }

		public bool IsEnabled { get; set; }

		public RenderMarkdownToHtmlType Type { get; set; }
	}

	public class CoreRenderModel : RenderModel
	{
		public bool IsHtmlTagEnabled { get; set; }

		public bool IsAbbreviationEnabled { get; set; }

		public bool IsAutoLinkEnabled { get; set; }

		public bool IsBootstrapEnabled { get; set; }

		public bool IsCitationEnabled { get; set; }

		public bool IsDefinitionListEnabled { get; set; }

		public bool IsEmojiSmileyEnabled { get; set; }

		public bool IsFigureEnabled { get; set; }

		public bool IsFooterEnabled { get; set; }

		public bool IsFootnoteEnabled { get; set; }

		public bool IsMediaLinkEnabled { get; set; }

		public bool IsSoftLineAsHardlineBreakEnabled { get; set; }

		public bool IsSmartyPantEnabled { get; set; }

		public bool IsGenericAttributeEnabled { get; set; }

		public CoreRenderModel()
		{
			Title = "Core";
			Subtitle = "Basic Markdown Parser";
			Type = RenderMarkdownToHtmlType.Core;
			FontGlyph = "\uF589";
		}
	}

	public class EmphasisExtraRenderModel : RenderModel
	{
		public bool IsStrikethroughEnabled { get; set; }

		public bool IsSuperSubScriptEnabled { get; set; }

		public bool IsInsertedEnabled { get; set; }

		public bool IsMarkedEnabled { get; set; }

		public EmphasisExtraRenderModel()
		{
			Title = "Emphasis Extra";
			Subtitle = "Emphasis rendering";
			Type = RenderMarkdownToHtmlType.EmphasisExtra;
			FontGlyph = "\uE1C8";
			// TODO Icon
		}
	}

	public class TableRenderModel : RenderModel
	{
		public bool IsGridTableEnabled { get; set; }

		public bool IsPipeTableEnabled { get; set; }

		public TableRenderModel()
		{
			Title = "Table";
			Subtitle = "Advanced table markup";
			Type = RenderMarkdownToHtmlType.Table;
			FontGlyph = "\uE80A";
		}
	}

	public class ListRenderModel : RenderModel
	{
		public bool IsTaskListEnabled { get; set; }

		public bool IsListExtraEnabled { get; set; }

		public ListRenderModel()
		{
			Title = "List";
			Subtitle = "List rendering";
			Type = RenderMarkdownToHtmlType.List;
			FontGlyph = "\uE15C";
		}
	}

	public class MathRenderModel : RenderModel
	{
		public MathRenderModel()
		{
			Title = "Mathematics";
			Subtitle = "Math rendering";
			Type = RenderMarkdownToHtmlType.Mathematics;
			FontGlyph = "\u2211";
			FontFamily = "Segoe UI Symbol";
		}
	}

	public class SyntaxHighlightingRenderModel : RenderModel
	{
		public SyntaxHighlightingRenderModel()
		{
			Title = "Syntax Highlighting";
			Subtitle = "Code syntax highlighting";
			Type = RenderMarkdownToHtmlType.SyntaxHighlighting;
			FontGlyph = "\uE7E6";
		}
	}

	public class DiagramRenderModel : RenderModel
	{
		public bool IsMermaidEnabled { get; set; }

		public bool IsNomnomlEnabled { get; set; }

		public DiagramRenderModel()
		{
			Title = "Diagram";
			Subtitle = "Enable diagram rendering";
			Type = RenderMarkdownToHtmlType.Diagram;
			FontGlyph = "\uE9D9";
		}
	}

	public enum RenderMarkdownToHtmlType
	{
		Core,
		CustomContainer,
		EmphasisExtra,

		/// <summary>
		/// ListExtra, TaskList
		/// </summary>
		List,
		Mathematics,

		/// <summary>
		/// GridTable, PipeTable
		/// </summary>
		Table,
		YamlFrontMatter,
		SyntaxHighlighting,
		Diagram,
		Globalization,
		JiraLinks,
		PreciseSourceLocation,
		SelfPipeline
	}
}