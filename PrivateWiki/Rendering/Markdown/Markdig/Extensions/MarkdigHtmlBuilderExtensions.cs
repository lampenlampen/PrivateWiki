using System;

namespace PrivateWiki.Rendering.Markdown.Markdig.Extensions
{
	public static class MarkdigHtmlBuilderExtensions
	{
		public static void UseMathjax(this HtmlBuilder builder)
		{
			builder.LoadScriptAsyncFromUrl(new Uri("https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js"));
		}

		public static void UseMermaid(this HtmlBuilder builder)
		{
			builder.LoadScriptFromUrl(new Uri("https://cdnjs.cloudflare.com/ajax/libs/mermaid/7.1.2/mermaid.min.js"));
			builder.WriteScriptTag("mermaid.initialize({startOnLoad: true});");
		}

		public static void UseNomnoml(this HtmlBuilder builder)
		{
			builder.LoadScriptFromUrl(new Uri("https://cdnjs.cloudflare.com/ajax/libs/nomnoml/0.7.1/nomnoml.min.js"));
			builder.WriteScriptTag(
				"function htmlDecode(input)\n{\n    var doc = new DOMParser().parseFromString(input, \"text/html\");\n    return doc.documentElement.textContent;\n}\nvar graphs = document.getElementsByClassName('nomnoml');\nfor (var i = 0; i < graphs.length; i++) {\n    graphs[i].innerHTML = nomnoml.renderSvg(htmlDecode(graphs[i].innerHTML));\n}");
		}

		public static void UseVSCodeMarkdownStylesheet(this HtmlBuilder builder)
		{
			builder.LoadStylesheetFromUrl(new Uri("https://cdn.jsdelivr.net/gh/Microsoft/vscode/extensions/markdown-language-features/media/markdown.css"));
		}

		public static void UsePrismSyntaxHighlighting(this HtmlBuilder builder)
		{
			builder.LoadStylesheetFromUrl(new Uri("https://cdnjs.cloudflare.com/ajax/libs/prism/1.20.0/themes/prism.min.css"));
			builder.LoadScriptFromUrl(new Uri("https://cdnjs.cloudflare.com/ajax/libs/prism/1.20.0/components/prism-core.min.js"));
			builder.LoadScriptFromUrl(new Uri("https://cdnjs.cloudflare.com/ajax/libs/prism/1.20.0/plugins/autoloader/prism-autoloader.min.js"));
		}
	}
}