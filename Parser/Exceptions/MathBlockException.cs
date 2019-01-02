using System.Collections.Generic;

namespace Parser.Exceptions
{
	public class MathBlockException : MarkupParserException
	{
		public MathBlockException(string message, List<string> parsedLines = null, int relativeLine = -1,
			int blockStartLine = -1) : base(message, parsedLines, relativeLine, blockStartLine)
		{
		}
	}
}