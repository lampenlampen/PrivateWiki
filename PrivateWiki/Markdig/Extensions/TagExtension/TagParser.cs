using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace PrivateWiki.Markdig.Extensions.TagExtension
{
	public class TagParser : InlineParser
	{
		/// <summary>
		/// The key used to store at the document level the pending <see cref="TagInline"/>
		/// </summary>
		public static readonly object DocumentKey = typeof(TagInline);
		
		public TagParser()
		{
			OpeningCharacters = new[] {'#'};
		}

		public override bool Match(InlineProcessor processor, ref StringSlice slice)
		{
			// TODO 
			
			var startPosition = slice.Start;

			var c = slice.CurrentChar;

			if (c != '#') return false;

			slice.NextChar();

			var buffer = StringBuilderCache.Local();

			while (true)
			{
				c = slice.CurrentChar;

				if (c == ' ' || c == ')' || c == ']' || c == '\0' || c == '\r' || c == '\n') break;

				buffer.Append(c);

				slice.NextChar();
			}

			var tagContent = buffer.ToString();

			var tag = new TagInline
			{
				Content = tagContent,
				Span = new SourceSpan(processor.GetSourcePosition(startPosition+1, out int line, out int column), processor.GetSourcePosition(slice.Start - 1)),
				Line = line,
				Column = column
			};

			// Maintain a list of all tags at document level
			var tags = processor.Document.GetData(DocumentKey) as TagGroup;
			if (tags == null)
			{
				tags = new TagGroup();
				processor.Document.SetData(DocumentKey, tags);
			}
			tags.Add(tag);
			
			// TODO add setting

			var linkInline = new LinkInline
			{
				Span = new SourceSpan(processor.GetSourcePosition(startPosition, out line, out column), processor.GetSourcePosition(slice.Start - 1)),
				Line = line,
				Column = column,
				Url = $"#{tagContent}",
				Label = tagContent,
				IsClosed = true,
				IsAutoLink = false
			};

			linkInline.UrlSpan = linkInline.Span;
			linkInline.AppendChild(new LiteralInline
			{
				Span = linkInline.Span,
				Line = line,
				Column = column,
				Content = new StringSlice(slice.Text, startPosition, startPosition + tagContent.Length),
				IsClosed = true
			});
			
			
			processor.Inline = linkInline;
			
			return true;
		}
	}
}