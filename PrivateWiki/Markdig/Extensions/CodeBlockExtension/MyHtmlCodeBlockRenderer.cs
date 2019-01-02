using System.IO;
using System.Text;
using ColorCode;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.SyntaxHighlighting;

namespace PrivateWiki.Markdig.Extensions.CodeBlockExtension
{
	internal class MyHtmlCodeBlockRenderer : SyntaxHighlightingCodeBlockRenderer
	{
		private readonly IStyleSheet _customCss;
		private readonly CodeBlockRenderer _underlyingRenderer;

		public MyHtmlCodeBlockRenderer(CodeBlockRenderer underlyingRenderer = null, IStyleSheet customCss = null)
		{
			_underlyingRenderer = underlyingRenderer ?? new CodeBlockRenderer();
			_customCss = customCss;
		}

		protected override void Write(HtmlRenderer renderer, CodeBlock obj)
		{
			if (obj is CodeBlock && !(obj is FencedCodeBlock))
			{
				// Code Block is Indented

				renderer
					.Write("<div")
					.WriteAttributes(obj.TryGetAttributes() ?? new HtmlAttributes())
					.Write(">");

				string firstLine2;
				var code2 = GetCode(obj, out firstLine2);

				RenderCode(renderer, null, firstLine2, code2);

				renderer.WriteLine("</div>");
				return;
			}


			var fencedCodeBlock = obj as FencedCodeBlock;
			var parser = obj.Parser as FencedCodeBlockParser;

			if (fencedCodeBlock == null || parser == null)
			{
				_underlyingRenderer.Write(renderer, obj);
				return;
			}

			var attributes = obj.TryGetAttributes() ?? new HtmlAttributes();

			var languageMoniker = fencedCodeBlock.Info.Replace(parser.InfoPrefix, string.Empty);
			if (string.IsNullOrEmpty(languageMoniker))
			{
				_underlyingRenderer.Write(renderer, obj);
				return;
			}

			attributes.AddClass($"lang-{languageMoniker}");
			attributes.Classes.Remove($"language-{languageMoniker}");

			attributes.AddClass("editor-colors");

			renderer
				.Write("<div")
				.WriteAttributes(attributes)
				.Write(">");

			string firstLine;
			var code = GetCode(obj, out firstLine);

			if (languageMoniker.Equals("mermaid"))
				RenderMermaidDiagram(renderer, code);
			else
				RenderCode(renderer, languageMoniker, firstLine, code);

			renderer.WriteLine("</div>");
		}

		private void RenderMermaidDiagram(HtmlRenderer renderer, string code)
		{
			var html = "<div class=\"mermaid\">" +
			           $"{code}" +
			           "</div>";

			renderer.Write(html);
		}

		private void RenderCode(HtmlRenderer renderer, string languageMoniker, string firstLine, string code)
		{
			RenderCodeHeader(renderer, languageMoniker);

			var languageTypeAdapter = new LanguageTypeAdapter();
			var language = languageTypeAdapter.Parse(languageMoniker, firstLine);

			var markup = ApplySyntaxHighlighting(languageMoniker, firstLine, code);

			if (language != null) markup = markup.Substring(56, markup.Length - 56 - 12);

			var html = $"<pre style=\"{MarkdigStyleSheet.CodeBlock.GetCodeBlockBoxStyle()}\">\r\n" +
			           $"<code style=\"{MarkdigStyleSheet.CodeBlock.GetCodeBlockStyle()}\">" +
			           $"{markup}" +
			           "</code>" +
			           "</pre>";


			renderer.Write(html);
		}

		private void RenderCodeHeader(HtmlRenderer renderer, string languageMoniker)
		{
			var codeHeader =
				$"<div class=\"codeHeader\" style=\"{MarkdigStyleSheet.CodeBlockHeader.GetCodeBlockHeaderStyle()}\">\r\n" +
				$"<span class=\"language\" style=\"{MarkdigStyleSheet.CodeBlockHeader.GetCodeBlockLanguageSpanStyle()}\">{languageMoniker}</span>\r\n" +
				$"<button id=\"codeCopy\" onClick=\"codeCopyClickFunction\" class=\"action\" style=\"{MarkdigStyleSheet.CodeBlockHeader.GetCodeBlockButtonStyle()}\">\r\n" +
				$"<span style=\"{MarkdigStyleSheet.CodeBlockHeader.GetCodeBlockButtonIconStyle()}\"> </span>\r\n" +
				"<span>Copy</span>\r\n" +
				"</button>\r\n" +
				"</div>\r\n";
			renderer.Write(codeHeader);
		}

		private string ApplySyntaxHighlighting(string languageMoniker, string firstLine, string code)
		{
			var languageTypeAdapter = new LanguageTypeAdapter();
			var language = languageTypeAdapter.Parse(languageMoniker, firstLine);

			if (language == null) return code;

			var codeBuilder = new StringBuilder();
			var codeWriter = new StringWriter(codeBuilder);
			var styleSheet = _customCss ?? StyleSheets.Default;
			var colourizer = new CodeColorizer();
			colourizer.Colorize(code, language, Formatters.Default, styleSheet, codeWriter);
			return codeBuilder.ToString();
		}

		private static string GetCode(LeafBlock obj, out string firstLine)
		{
			var code = new StringBuilder();
			firstLine = null;
			foreach (var line in obj.Lines.Lines)
			{
				var slice = line.Slice;
				if (slice.Text == null) continue;

				var lineText = slice.Text.Substring(slice.Start, slice.Length);

				if (firstLine == null)
					firstLine = lineText;
				else
					code.AppendLine();

				code.Append(lineText);
			}

			return code.ToString();
		}
	}
}