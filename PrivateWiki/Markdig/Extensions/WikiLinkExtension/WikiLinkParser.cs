using System;
using System.Collections.Generic;
using System.Diagnostics;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax.Inlines;
using NLog;

namespace PrivateWiki.Markdig.Extensions.WikiLinkExtension
{
	public class WikiLinkParser : InlineParser
	{
		private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		public WikiLinkParser()
		{
			Logger.Debug("WikiLinkParser");
			OpeningCharacters = new[]
			{
				'['
			};
		}

		public override bool Match(InlineProcessor processor, ref StringSlice slice)
		{
			Logger.Debug($"Current Char: ${slice.CurrentChar}");

			// Check if next char is '['
			var nextChar = slice.NextChar();

			Logger.Debug($"Next Char: ${nextChar}");

			return false;
		}

		private static bool IsValidPreviousCharacter(char c)
		{
			// All such recognized WikiLinks can only come at the beginning of a line, after whitespace, or any of the delimiting characters *, _, ~, and (.
			return c.IsWhiteSpaceOrZero() || c == '*' || c == '_' || c == '~' || c == '(';
		}

		public bool Match2(InlineProcessor processor, ref StringSlice slice)
		{
			// Previous char must be a whitespace or a punctuation
			var previousChar = slice.PeekCharExtra(-1);
			if (!IsValidPreviousCharacter(previousChar)) return false;

			List<char> pendingEmphasis;
			// Check that an autolink is possible in the current context
			if (!IsAutoLinkValidInCurrentContext(processor, out pendingEmphasis)) return false;

			var startPosition = slice.Start;

			var c = slice.CurrentChar;
			// Precheck WikiLink

			if (!slice.MatchLowercase("[[", 0)) return false;

			/*
			switch (c)
			{
			    case 'h':
			        if (!slice.MatchLowercase("ttp://", 1) && !slice.MatchLowercase("ttps://", 1))
			        {
			            return false;
			        }
			        break;
			    case 'f':
			        if (!slice.MatchLowercase("tp://", 1))
			        {
			            return false;
			        }
			        break;
			    case 'm':
			        if (!slice.MatchLowercase("ailto:", 1))
			        {
			            return false;
			        }
			        break;

			    case 'w':
			        if (!slice.MatchLowercase("ww.", 1)) // We won't match http:/www. or /www.xxx
			        {
			            return false;
			        }
			        break;
			}
			*/

			// Parse URL
			var link = "";

			/*
			string link;
			if (!LinkHelper.TryParseUrl(ref slice, out link, true))
			{
			    return false;
			}
			*/


			var buffer = StringBuilderCache.Local();
			while (true)
			{
				if (slice.CurrentChar == ' ') break;

				buffer.Append(slice.CurrentChar);

				slice.NextChar();
			}

			link = buffer.ToString();

			// Cut the last ']]' of.
			link = link.Substring(0, link.Length - 2);


			// If we have any pending emphasis, remove any pending emphasis characters from the end of the link
			if (pendingEmphasis != null)
				for (var i = link.Length - 1; i >= 0; i--)
					if (pendingEmphasis.Contains(link[i]))
					{
						slice.Start--;
					}
					else
					{
						if (i < link.Length - 1) link = link.Substring(0, i + 1);

						break;
					}

			// Post-check URL

			if (link.Length == 0) return false;

			/*
			switch (c)
			{
			    case 'h':
			        if (string.Equals(link, "http://", StringComparison.OrdinalIgnoreCase) ||
			            string.Equals(link, "https://", StringComparison.OrdinalIgnoreCase))
			        {
			            return false;
			        }
			        break;
			    case 'f':
			        if (string.Equals(link, "ftp://", StringComparison.OrdinalIgnoreCase))
			        {
			            return false;
			        }
			        break;
			    case 'm':
			        if (string.Equals(link, "mailto:", StringComparison.OrdinalIgnoreCase) || !link.Contains("@"))
			        {
			            return false;
			        }
			        break;

			    case 'w':
			        // We require at least two .
			        if (link.Length <= "www.x.y".Length || link.IndexOf(".", 4, StringComparison.Ordinal) < 0)
			        {
			            return false;
			        }
			        break;
			}
			*/

			int line;
			int column;
			var inline = new LinkInline
			{
				Span =
				{
					Start = processor.GetSourcePosition(startPosition, out line, out column)
				},
				Line = line,
				Column = column,
				Url = link,
				IsClosed = true,
				IsAutoLink = true
			};

			inline.Span.End = inline.Span.Start + link.Length - 1;
			inline.UrlSpan = inline.Span;
			inline.AppendChild(new LiteralInline
			{
				Span = inline.Span,
				Line = line,
				Column = column,
				Content = new StringSlice(slice.Text, startPosition, startPosition + link.Length - 1),
				IsClosed = true
			});
			processor.Inline = inline;

			return true;
		}

		private bool IsAutoLinkValidInCurrentContext(InlineProcessor processor, out List<char> pendingEmphasis)
		{
			pendingEmphasis = null;

			// Case where there is a pending HtmlInline <a>
			var currentInline = processor.Inline;
			while (currentInline != null)
			{
				var htmlInline = currentInline as HtmlInline;
				if (htmlInline != null)
				{
					// If we have a </a> we don't expect nested <a>
					if (htmlInline.Tag.StartsWith("</a", StringComparison.OrdinalIgnoreCase)) break;

					// If there is a pending <a>, we can't allow a link
					if (htmlInline.Tag.StartsWith("<a", StringComparison.OrdinalIgnoreCase)) return false;
				}

				// Check previous sibling and parents in the tree 
				currentInline = currentInline.PreviousSibling ?? currentInline.Parent;
			}

			// Check that we don't have any pending brackets opened (where we could have a possible markdown link)
			// NOTE: This assume that [ and ] are used for links, otherwise autolink will not work properly
			currentInline = processor.Inline;
			var countBrackets = 0;
			while (currentInline != null)
			{
				var linkDelimiterInline = currentInline as LinkDelimiterInline;
				if (linkDelimiterInline != null && linkDelimiterInline.IsActive)
				{
					if (linkDelimiterInline.Type == DelimiterType.Open)
						countBrackets++;
					else if (linkDelimiterInline.Type == DelimiterType.Close) countBrackets--;
				}
				else
				{
					// Record all pending characters for emphasis
					var emphasisDelimiter = currentInline as EmphasisDelimiterInline;
					if (emphasisDelimiter != null)
					{
						if (pendingEmphasis == null) pendingEmphasis = new List<char>();

						if (!pendingEmphasis.Contains(emphasisDelimiter.DelimiterChar))
							pendingEmphasis.Add(emphasisDelimiter.DelimiterChar);
					}
				}

				currentInline = currentInline.Parent;
			}

			return countBrackets <= 0;
		}
	}
}