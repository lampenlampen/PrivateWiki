using System;
using System.Reflection;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace DataAccessLibrary.PageAST.Blocks
{
	public class CodeBlock : IPageBlock
	{
		public Guid Id { get; set; }
		
		public string Code { get; set; }
		
		public string LanguageCode { get; set; }

		public bool IsSyntaxHighlightingEnabled = false;

		public CodeBlock(string code, string languageCode)
		{
			Id = Guid.NewGuid();
			Code = code;
			LanguageCode = languageCode;
		}
	}
}