using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax.Inlines;

namespace PrivateWiki.Rendering.Markdown.Markdig.Extensions.WikiLinkExtension
{
	public class WikiLinkParser : InlineParser
	{
		public WikiLinkParser()
		{
			OpeningCharacters = new[]
			{
				'['
			};
		}

		public override bool Match(InlineProcessor processor, ref StringSlice slice)
		{
			var startPosition = slice.Start;

			// Check if next char is '['
			var nextChar = slice.NextChar();
			if (nextChar != '[') return false;
			slice.NextChar();

			var buffer = StringBuilderCache.Local();

			// Parse link
			while (true)
			{
				if (slice.CurrentChar == ' ' || slice.CurrentChar == '|' || slice.CurrentChar == '.' || slice.CurrentChar == ',' || slice.CurrentChar == '\r' || slice.CurrentChar == '\n' ||
				    slice.CurrentChar == '\0') break;

				buffer.Append(slice.CurrentChar);

				slice.NextChar();
			}

			var link = buffer.ToString();


			if (slice.CurrentChar == '|')
			{
				// TODO 
			}

			// Cut the last ']]' of.
			link = link.Substring(0, link.Length - 2);

			var inline = new LinkInline
			{
				Span =
				{
					Start = processor.GetSourcePosition(startPosition, out var line, out var column)
				},
				Line = line,
				Column = column,
				Url = $":{link}",
				Label = "huhu",
				IsClosed = true,
				IsAutoLink = false
			};

			inline.Span.End = inline.Span.Start + link.Length - 1;
			inline.UrlSpan = inline.Span;
			inline.AppendChild(new LiteralInline
			{
				Span = inline.Span,
				Line = line,
				Column = column,
				Content = new StringSlice(slice.Text, startPosition + 2, startPosition + 1 + link.Length),
				IsClosed = true
			});

			processor.Inline = inline;

			return true;
		}
	}
}