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
		public string Icon { get; set; } = "\uE11D";

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

		public bool IsCustomContainerEnabled { get; set; }

		public bool IsDefinitionListEnabled { get; set; }

		public bool IsEmojiSmileyEnabled { get; set; }

		public bool EmphasisExtraEnabled { get; set; }

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
			Icon = "\uF589";
		}
	}

	public enum RenderMarkdownToHtmlType
	{
		Core,
		Abbreviation,
		AutoLink,
		Bootstrap,
		Citation,
		CustomContainer,
		DefinitionList,
		EmojiSmiley,
		EmphasisExtra,
		Figure,
		Footer,
		Footnotes,

		/// <summary>
		/// ListExtra, TaskList
		/// </summary>
		List,
		Mathmatics,
		Media,
		SoftlineBreakAsHardlineBreak,
		SmartyPants,

		/// <summary>
		/// GidTable, PipeTable
		/// </summary>
		Table,
		GenericAttributes,
		YamlFrontMatter,
		SyntaxHighlighting,
		Diagram,
		Globalization,
		JiraLinks,
		PreciseSourceLocation,
		SelfPipeline
	}
}