using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Exceptions
{
	public class MarkupParserException : Exception
	{
		protected MarkupParserException(string message, List<string> parsedLines, int relativeLine, int blockStartLine)
			: base(message)
		{
			if (relativeLine != -1 && blockStartLine != -1) Line = blockStartLine + relativeLine;
			ParsedLine = relativeLine;
			ParsedLines = parsedLines;
		}

		public int Line { get; set; } = -1;
		public List<string> ParsedLines { get; set; }
		public int ParsedLine { get; set; } = -1;

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"Error at Line {Line}.");
			builder.AppendLine($"Line: {ParsedLines[ParsedLine]}");
			builder.AppendLine("Block:");

			foreach (var line in ParsedLines) builder.AppendLine(line);

			builder.AppendLine($"Error Message: {Message}");

			return builder.ToString();
		}
	}
}